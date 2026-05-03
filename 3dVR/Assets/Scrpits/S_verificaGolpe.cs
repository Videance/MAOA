using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_verificaGolpe : MonoBehaviour
{
    public GameObject pDesequil;
    public TextMesh textTempo;

    [Header("Lista de golpes")]
    [SerializeField] public List<C_golpes> golpes = new List<C_golpes>();
    private static C_golpes ataque;

    public static bool resetaCena = false;
    public static bool timeSlow = false;
    private Light luz;
    public static S_verificaGolpe Vgolpe;

    public static float tempo = 0;

    private void Awake()
    {
        if (Vgolpe == null)
        {
            Vgolpe = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (textTempo != null)  textTempo.text = "";
        luz = FindAnyObjectByType<Light>();
    }

    void OnEnable() { SceneManager.sceneLoaded += AoCarregarCena; }
    void OnDisable() { SceneManager.sceneLoaded -= AoCarregarCena; }
    void AoCarregarCena(Scene scene, LoadSceneMode mode)
    {
        luz = FindAnyObjectByType<Light>();
    }

    public void AcharGolpe(S_jogador jog, S_jogador adv)
    {
        if (timeSlow) return;

        foreach (var golpe in Vgolpe.golpes)
        {
            int pontos = 0;

            if (golpe.conectorImaoDir == jog.imaoDir) pontos++;
            if (golpe.conectorImaoEsq == jog.imaoEsq) pontos++;
            if (golpe.JdirEqui == jog.dirEqui) pontos++;
            if (golpe.IdirEqui == adv.dirEqui) pontos++;
            if (golpe.pernaAberta == jog.pernaAberta) pontos++;

            Debug.Log(pontos);

            if (pontos == 5)
            {
                ataque = golpe;
                Vgolpe.StartCoroutine(Vgolpe.TimeSlow(golpe, jog, adv));
                break;
            }
            else if (pontos == 4 && golpe.IdirEqui != adv.dirEqui) jog.GetComponent<S_energia>().energia -= golpe.custoEnergia;
        }
    }

    public IEnumerator TimeSlow(C_golpes move, S_jogador jog, S_jogador adv)
    {
        if (timeSlow) yield break;
        timeSlow = true;

        //cria o ponto
        Vector3 meio = (jog.IKs[0].transform.position + jog.IKs[1].transform.position) / 2f;
        Vector3 meio2 = (jog.transform.position - adv.transform.position) / 2f;

        GameObject pde = Instantiate(pDesequil, new Vector3(meio.x, meio.y, meio2.z), pDesequil.transform.rotation);
        adv.GetComponentInChildren<S_segueC>().pDes = pde.GetComponent<S_rb>().rb;
        adv.GetComponentInChildren<S_segueC>().SpontoDes = pde.GetComponent<S_pontoDes>();
        S_pontoDes Spde = pde.GetComponent<S_pontoDes>();

        GameObject caminho = Instantiate(ataque.dirPdes, new Vector3(meio.x, meio.y, meio2.z), ataque.dirPdes.transform.rotation);

        //pega energia
        S_energia jogEner = jog.GetComponent<S_energia>();
        S_energia advEner = adv.GetComponent<S_energia>();

        //adv - Ragdoll e desativa mão
        adv.Ragdoll(true);
        advEner.DesativaEnergia();

        //jog - Troca layer dos IK
        for (int i = 0; i < jog.iks.Length; i++) { jog.iks[i].gameObject.layer = LayerMask.NameToLayer("xG"); }

        //tempo lento:
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        //controla luz
        luz.enabled = false;

        //controla o tempo máximo
        float t = 3.5f;
        S_IK ik = jog.GetComponentInChildren<S_IK>();
        while (t > 0.5f && !Spde.jogado && Spde.noCaminho)
        {
            t -= Time.unscaledDeltaTime;
            if(textTempo != null) textTempo.text = Mathf.RoundToInt(tempo).ToString();
            yield return null;
        }

        if (textTempo != null) textTempo.text = "";
        Destroy(pde);
        Destroy(caminho);
        adv.Gravidade(true);

        luz.enabled = true;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        tempo = 10f;
        while (tempo > 0f)
        {
            tempo -= Time.unscaledDeltaTime;
            if (resetaCena)
            {
                jog.jogPontos = 1;
                continue;
            }
            yield return null;
        }

        jog.jogPontos += 1;

        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        timeSlow = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);      
    }
}
