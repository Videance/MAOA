using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_verificaGolpe : MonoBehaviour
{
    public GameObject pDesequil; //objeto de prfab
    public GameObject caminho;
    public GameObject pDes;
    public TextMesh textTempo;
    public static S_pontoDes Spde;
    public GameObject[] luzesNormal;
    public GameObject[] luzesDesequil;

    [Header("Lista de golpes")]
    [SerializeField] public List<C_golpes> golpes = new List<C_golpes>();
    private static C_golpes ataque;

    public static bool resetaCena = false;
    public static bool timeSlow = false;
    public static bool derrotou = false;
    public static S_verificaGolpe Vgolpe;

    public static float tempo = 0;

    public static bool emTutorial = false;

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
    }

    public void AcharGolpe(S_jogador jog, S_jogador adv)
    {
        if (timeSlow || derrotou) return;

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

    public void CriarPonto(int os2, S_jogador jog, S_jogador adv)
    {
        //cria o ponto e caminho
        Vector3 meio = (jog.IKs[0].transform.position + jog.IKs[1].transform.position) / 2f;
        Vector3 meio2 = (jog.transform.position + adv.transform.position) / 2f;
        Vector3 meio3 = new Vector3(meio.x, meio.y, meio2.z);

        if (os2 == 2)
        {
            caminho = Instantiate(ataque.dirPdes, meio3, ataque.dirPdes.transform.rotation);
        }

        pDes = Instantiate(pDesequil, meio3, pDesequil.transform.rotation);
        Spde = pDes.GetComponent<S_pontoDes>();
    }

    public IEnumerator TimeSlow(C_golpes golpe, S_jogador jog, S_jogador adv)
    {
        if (timeSlow || derrotou) yield break;

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
        CriarPonto(2, jog, adv);

        if (jog is Sbot_jogador) StartCoroutine(((Sbot_jogador)jog).MoverPdesequilibrio(pDes, caminho));

        //jog - Troca layer dos IK
        for (int i = 0; i < jog.IKs.Length; i++) jog.IKs[i].trocaEstado(S_IK.estadoMao.desativada);

        ConfigurarGrab(jog, false);
        ConfigurarGrab(adv, false);
        adv.GetComponent<S_energia>().DesativaEnergia(false);

        //tempo lento:
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        //controla luz
        foreach (GameObject luz in luzesNormal) luz.SetActive(false);
        foreach (GameObject luz in luzesDesequil) luz.SetActive(true);

        if (emTutorial) yield return new WaitUntil(() => emTutorial == false);
        else
        {
            //controla o tempo máximo
            tempo = 5.5f;
            while (tempo > 0.5f && Spde.noCaminho && !Spde.tocouClimax && adv.dirEqui != ataque.IdirEqui)
            {
                tempo -= Time.unscaledDeltaTime;
                textTempo.text = Mathf.RoundToInt(tempo).ToString();
                yield return null;
            }
            textTempo.text = "";
        }

        //tempo lento:
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        //controla luz
        foreach (GameObject luz in luzesNormal) luz.SetActive(true);
        foreach (GameObject luz in luzesDesequil) luz.SetActive(false);

        adv.gameObject.GetComponentInChildren<S_Equilibrio>().TrocarCor(adv.dirEqui);

        if (Spde.tocouClimax)
        {
            //destroi o ponto e caminho
            Destroy(pDes);
            Destroy(caminho);
            pDes = null;
            caminho = null;
            derrotou = true;
            Vgolpe.StartCoroutine(Vgolpe.Derrota(jog, adv));
            yield break;
        }
        else if (adv.dirEqui == ataque.IdirEqui)
        {
            jog.GetComponent<S_energia>().energia -= ataque.custoEnergia;

            //jog - Troca layer dos IK
            for (int i = 0; i < jog.PEs.Length; i++)
            {
                jog.IKs[i].Desconecta();
                jog.PEs[i].Mover(false);
            }
        }
        else
        {
            adv.GetComponent<S_energia>().energia -= ataque.custoEnergia / 2;
            jog.GetComponent<S_energia>().energia -= ataque.custoEnergia / 2;

            for (int i = 0; i < jog.IKs.Length; i++)
            {
                jog.IKs[i].Desconecta();
                jog.PEs[i].Mover(false);
                adv.IKs[i].Desconecta();
                adv.PEs[i].Mover(false);
            }
        }

        //destroi o ponto e caminho
        Destroy(pDes);
        Destroy(caminho);
        pDes = null;
        caminho = null;

        ConfigurarGrab(jog, true);
        ConfigurarGrab(adv, true);
        adv.GetComponent<S_energia>().DesativaEnergia(true);

        timeSlow = false;
    }

    public IEnumerator Derrota(S_jogador jog, S_jogador adv)
    {
        jog.GetComponent<S_energia>().DesativaEnergia(false);
        adv.GetComponent<S_energia>().DesativaEnergia(false);

        adv.Ragdoll(true);
        adv.Gravidade(true);

        S_segueC segue = adv.GetComponentInChildren<S_segueC>();
        segue.SpontoDes = Spde;

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

        ConfigurarGrab(jog, true);
        ConfigurarGrab(adv, true);

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
