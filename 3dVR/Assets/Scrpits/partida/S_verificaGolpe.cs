using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_verificaGolpe : MonoBehaviour
{
    public GameObject pDesequil; //objeto de prfab
    S_controleCena controleCena;
    public GameObject caminho;
    public GameObject pDes; // 1 normal , 2 time , 3 derrota
    public TextMeshPro textTempo;
    public static S_pontoDes Spde;
    public GameObject[] luzes;
    public TextMeshPro[] textInfo;
    public S_onClique Sclique;
    public NearFarInteractor[] nearFarInteractors;
    public static bool derrotaPorLimite = false;

    Vector3 dir;

    [Header("Lista de golpes")]
    [SerializeField] public List<C_golpes> golpes = new List<C_golpes>();
    private static C_golpes ataque;
    public List<Image> golpes3 = new List<Image>();

    public static bool resetaCena = false;
    public static bool timeSlow = false;
    public static bool derrotou = false;
    public static S_verificaGolpe Vgolpe;

    public static float tempo = 0;

    [Header("Pro tutorial")]
    public static bool esperaDerrota = false;
    public static bool esperaTime = false;
    public GameObject Botiprefab;

    [Header("Partículas")]
    public ParticleSystem[] fogos = new ParticleSystem[4];

    private void Awake()
    {
        if (Vgolpe == null)
        {
            Vgolpe = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (textTempo != null)  textTempo.text = "";

        controleCena = FindAnyObjectByType<S_controleCena>();
    }

    public void AcharGolpe(S_jogador jog, S_jogador adv)
    {
        if (timeSlow || derrotou) return;
        var ranking = new List<(C_golpes golpe, int pontos)>();

        Debug.Log("chamei" + jog);

        foreach (var golpe in Vgolpe.golpes)
        {
            if (S_controleCena.modo == S_controleCena.ModoJogo.Historia && S_modoHistoria.listaGolpes.Length > 0)
            {
                if (!S_modoHistoria.listaGolpes.Contains(golpe))
                    continue;
            }

            int pontos = 0;

            if (golpe.conectorImaoDir == jog.imaoDir) pontos++;
            if (golpe.conectorImaoEsq == jog.imaoEsq) pontos++;
            if (golpe.JdirEqui == jog.dirEqui) pontos++;
            if (golpe.pernaAberta == jog.posPerna) pontos++;

            if (pontos == 4)
            {
                ataque = golpe;
                Vgolpe.StartCoroutine(Vgolpe.TimeSlow(golpe, jog, adv));
                break;
            }

            ranking.Add((golpe, pontos));
        }

        var top3 = ranking.OrderByDescending(x => x.pontos).Take(3).ToList();

        for (int i = 0; i < 3; i++) golpes3[i].sprite = top3[i].golpe.imagem;
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


        if (!esperaTime && jog is Sbot_jogador) StartCoroutine(((Sbot_jogador)jog).MoverPdesequilibrio(pDes, caminho));
    }

    public IEnumerator TimeSlow(C_golpes golpe, S_jogador jog, S_jogador adv)
    {
        if (timeSlow || derrotou) yield break;

        if (!S_controleTutorial.tutorial1 && !adv.seMovendo)
        {
            jog.Fragil();

            for (int i = 0; i < jog.IKs.Length; i++)
            {
                jog.IKs[i].Desconecta();
                jog.PEs[i].Mover(false, false);
            }

            yield break;
        }

        ataque = golpe;

        if (adv.dirEqui == ataque.IdirEqui)
        {
            jog.GetComponent<S_energia>().energia -= ataque.custoEnergia;
            for (int i = 0; i < jog.IKs.Length; i++) jog.IKs[i].Desconecta();

            yield break;
        }

        timeSlow = true;

        //ativa a fuga do adv
        S_Equilibrio advEqui = adv.gameObject.GetComponent<S_Equilibrio>();

        advEqui.TrocarCor(ataque.IdirEqui, true);

        //cria o ponto e caminho
        CriarPonto(2, jog, adv);

        luzes[0].SetActive(false);
        luzes[1].SetActive(true);

        if (jog is Sbot_jogador) foreach (NearFarInteractor n in nearFarInteractors)
            {
                n.enabled = false;
            }

        ConfigurarGrab(jog, false);
        ConfigurarGrab(adv, false);
        adv.GetComponent<S_energia>().DesativaEnergia(false);

        //tempo lento:
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (esperaTime) yield return new WaitUntil(() => esperaTime == false);
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

        advEqui.TrocarCor(ataque.IdirEqui, false);
        advEqui.TrocarCor(adv.dirEqui, false);

        if (Spde.tocouClimax)
        {
            dir = Spde.dirFinal;

            //destroi o ponto e caminho
            Destroy(pDes);
            Destroy(caminho);
            pDes = null;
            caminho = null;

            Vgolpe.StartCoroutine(Vgolpe.Derrota(jog, adv));
            yield break;
        }
        else if (adv.dirEqui == ataque.IdirEqui)
        {
            jog.GetComponent<S_energia>().energia -= ataque.custoEnergia;
            foreach (ParticleSystem p in jog.falhou) p.Play();

            //jog - Troca layer dos IK
            for (int i = 0; i < jog.PEs.Length; i++)
            {
                jog.IKs[i].Desconecta();
                jog.PEs[i].Mover(false, false);
            }
        }
        else
        {
            adv.GetComponent<S_energia>().energia -= ataque.custoEnergia / 2;
            advEqui.dirFulga = null;
            jog.GetComponent<S_energia>().energia -= ataque.custoEnergia / 2;

            for (int i = 0; i < jog.IKs.Length; i++)
            {
                jog.IKs[i].Desconecta();
                jog.PEs[i].Mover(false, false);
                adv.IKs[i].Desconecta();
                adv.PEs[i].Mover(false, false);
            }
        }

        //controla luz
        luzes[0].SetActive(true);
        luzes[1].SetActive(false);

        //destroi o ponto e caminho
        Destroy(pDes);
        Destroy(caminho);
        pDes = null;
        caminho = null;

        ConfigurarGrab(jog, true);
        ConfigurarGrab(adv, true);
        adv.GetComponent<S_energia>().DesativaEnergia(true);

        foreach (NearFarInteractor n in nearFarInteractors)
        {
            n.enabled = true;
        }

        timeSlow = false;
    }

    public IEnumerator Derrota(S_jogador jog, S_jogador adv)
    {
        if (derrotou) yield break;
        derrotou = true;

        luzes[0].SetActive(false);
        luzes[1].SetActive(false);
        luzes[2].SetActive(true);
        foreach (S_holofotes Hluz in luzes[2].GetComponentsInChildren<S_holofotes>())
        {
            Hluz.seguirAlvo = true;
            Hluz.alvo = adv.GetComponentInChildren<S_segueC>().transform;
        }

        jog.GetComponent<S_energia>().DesativaEnergia(false);
        adv.GetComponent<S_energia>().DesativaEnergia(false);

        adv.Ragdoll(true);
        adv.Gravidade(true);

        adv.GetComponentInChildren<S_segueC>().Joga(dir);

        tempo = 7f;
        while (tempo > 0f && !resetaCena)
        {
            tempo -= Time.unscaledDeltaTime;
            yield return null;
        }

        if (jog == S_pontos.Spontos.jogadores[0])
        {
            if (resetaCena) S_pontos.Spontos.pontos1 = 2;
            else S_pontos.Spontos.pontos1 += 1;
        }
        else
        {
            if (resetaCena) S_pontos.Spontos.pontos2 = 2;
            else S_pontos.Spontos.pontos2 += 1;
        }

        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        foreach (S_holofotes Hluz in luzes[2].GetComponentsInChildren<S_holofotes>())
        {
            Hluz.seguirAlvo = true;
            Hluz.alvo = null;
        }

        foreach (ParticleSystem p in fogos) p.Play();

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        ConfigurarGrab(jog, true);
        ConfigurarGrab(adv, true);

        timeSlow = false;
        resetaCena = false;
        tempo = 0;
        derrotou = false;
        derrotaPorLimite = false;

        luzes[2].SetActive(false);
        luzes[0].SetActive(true);

        if (S_pontos.Spontos.pontos2 >= 2 || S_pontos.Spontos.pontos1 >= 2)
        {
            if (adv is Sbot_jogador)
            {
                //ganhou
                if (S_pontos.vitoriasXbot.Count > 0)
                {
                    for (int i = 0; i < S_pontos.vitoriasXbot.Count; i++)
                        if (S_pontos.vitoriasXbot[i].x == Sbot_jogador.dificuldade)
                        {
                            S_pontos.vitoriasXbot[i] = new Vector2(S_pontos.vitoriasXbot[i].x, S_pontos.vitoriasXbot[i].y + 1);
                            break;
                        }

                    Sclique.PassarFase();
                }
                else S_pontos.vitoriasXbot.Add(new Vector2(Sbot_jogador.dificuldade, 1f));
            }
            else
            {
                //perde
            }

            S_pontos.Spontos.pontos1 = 0;
            S_pontos.Spontos.pontos2 = 0;

            controleCena.ColocarMAOA(false);
            FindAnyObjectByType<S_onClique>().TrocaUI(0);
            yield break;
        }

        if (textInfo != null && textInfo.Length > 0)
        {
            for (int i = 0; i < textInfo.Length; i++)
            {
                if (i <= 5) textInfo[i].text =
            "-- P L A C A R --\n" +
            "Jogador: " + S_pontos.Spontos.pontos1 + "\n" +
            "BOT: " + S_pontos.Spontos.pontos2;
                else textInfo[i].text = "Dificulade =" + Sbot_jogador.dificuldade;
            }
        }

        foreach (NearFarInteractor n in nearFarInteractors)
        {
            enabled = true;
        }

        controleCena.ColocarMAOA(true);
    }

    void ConfigurarGrab(S_jogador jog, bool ativo)
    {
        for (int i = 0; i < jog.IKs.Length; i++)
        {
            if (jog == null) break;

            jog.IKs[i].grab.trackPosition = ativo;
            jog.IKs[i].grab.trackRotation = ativo;
            jog.IKs[i].grab.enabled = ativo;

            jog.PEs[i].grab.trackPosition = ativo;
            jog.PEs[i].grab.trackRotation = ativo;
            jog.PEs[i].grab.enabled = ativo;
        }
    }
}
