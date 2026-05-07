using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_verificaGolpe : MonoBehaviour
{
    public GameObject pDesequil;
    GameObject pDes;
    public TextMesh textTempo;

    [Header("Lista de golpes")]
    [SerializeField] public List<C_golpes> golpes = new List<C_golpes>();
    private static C_golpes ataque;

    public static bool resetaCena = false;
    public static bool timeSlow = false;
    public static bool derrotou = false;
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
            if (golpe.pernaAberta == jog.pernaAberta) pontos++;

            if (pontos == 4)
            {
                ataque = golpe;
                Vgolpe.StartCoroutine(Vgolpe.TimeSlow(golpe, jog, adv));
                break;
            }
        }
    }

    public IEnumerator TimeSlow(C_golpes move, S_jogador jog, S_jogador adv)
    {
        if (timeSlow) yield break;

        if (jog is Sbot_jogador) ataque = ((Sbot_jogador)jog).golpe;

        if (adv.dirEqui == ataque.IdirEqui)
        {
            jog.GetComponent<S_energia>().energia -= ataque.custoEnergia;
            for (int i = 0; i < jog.IKs.Length; i++) jog.IKs[i].Desconecta();

            yield break;
        }

        timeSlow = true;

        //ativa a fuga do adv
        adv.gameObject.GetComponentInChildren<S_Equilibrio>().PlacaFuga(ataque.IdirEqui);

        //cria o ponto e caminho
        Vector3 meio = (jog.IKs[0].transform.position + jog.IKs[1].transform.position) / 2f;
        Vector3 meio2 = (jog.transform.position + adv.transform.position) / 2f;
        Vector3 meio3 = new Vector3(meio.x, meio.y, meio2.z);

        GameObject pde = Instantiate(pDesequil, meio3, pDesequil.transform.rotation);
        pDes = pde;
        S_pontoDes Spde = pde.GetComponent<S_pontoDes>();

        GameObject caminho = Instantiate(ataque.dirPdes, meio3, ataque.dirPdes.transform.rotation);

        //jog - Troca layer dos IK
        for (int i = 0; i < jog.IKs.Length; i++) jog.IKs[i].trocaEstado(S_IK.estadoMao.desativada);

        ConfigurarGrab(jog, false);
        ConfigurarGrab(adv, false);

        //tempo lento:
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        //controla luz
        luz.enabled = false;

        //controla o tempo máximo
        tempo = 3.5f;
        while (tempo > 0.5f && !Spde.jogado && Spde.noCaminho && adv.dirEqui != ataque.IdirEqui)
        {
            tempo -= Time.unscaledDeltaTime;
            textTempo.text = Mathf.RoundToInt(tempo).ToString();
            yield return null;
        }
        textTempo.text = "";

        //destroi o ponto e caminho
        Destroy(pde);
        Destroy(caminho);

        //tempo lento:
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        //controla luz
        luz.enabled = true;

        if (adv.dirEqui != ataque.IdirEqui) Vgolpe.StartCoroutine(Vgolpe.Derrota(jog, adv));
        else
        {
            jog.GetComponent<S_energia>().energia -= ataque.custoEnergia;

            //jog - Troca layer dos IK
            for (int i = 0; i < jog.IKs.Length; i++)
            {
                jog.IKs[i].Desconecta();
                jog.IKs[i].trocaEstado(S_IK.estadoMao.livre);
            }

            ConfigurarGrab(jog, true);
            ConfigurarGrab(adv, true);

            timeSlow = false;
        }
    }

    public IEnumerator Derrota(S_jogador jog, S_jogador adv)
    {
        derrotou = true;

        jog.GetComponent<S_energia>().DesativaEnergia();
        adv.GetComponent<S_energia>().DesativaEnergia();

        adv.Ragdoll(true);
        adv.Gravidade(true);

        S_segueC segue = adv.GetComponentInChildren<S_segueC>();
        segue.pDes = pDes.GetComponent<S_rb>().rb;
        segue.SpontoDes = pDes.GetComponent<S_pontoDes>();

        tempo = 8f;
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

        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        timeSlow = false;
        resetaCena = false;
        tempo = 0;
        derrotou = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ConfigurarGrab(S_jogador jog, bool ativo)
    {
        for (int i = 0; i < jog.IKs.Length; i++)
        {
            jog.IKs[i].grab.trackPosition = ativo;
            jog.IKs[i].grab.trackRotation = ativo;
            jog.IKs[i].grab.enabled = ativo;

            jog.PEs[i].grab.trackPosition = ativo;
            jog.PEs[i].grab.trackRotation = ativo;
            jog.PEs[i].grab.enabled = ativo;
        }
    }
}
