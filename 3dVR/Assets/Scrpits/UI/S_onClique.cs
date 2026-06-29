using FMOD.Studio;
using FMODUnity;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class S_onClique : MonoBehaviour
{
    S_controleCena controleCena;
    public GameObject[] UIs;
    public GameObject historiaButtons;
    public GameObject[] HB;
    int faseAtual = 0; //troca pra 0 dps
    bool passandoT = false;
    public static float T = 0; 
    public static bool naoAvanca = false;
    bool TparaVozes = false;
    public GameObject MAOAfake;

    [Header("mover cabeça")]
    public GameObject CameraOffset;

    [Header("Textos")]
    public TextMeshPro TcamX;
    public TextMeshPro TcamY;
    public TextMeshPro Tdificuldade;
    public GameObject camOffset;
    public TextMeshPro[] TextosTelao;
    public TextMeshPro TresDosUm;

    [Header("Particulas")]
    public ParticleSystem[] bordas;

    [Header("SONS")]
    public static float volume = 1f;
    public Slider slider;

    // Loops
    public EventReference musicaMenu;
    public EventReference musicaBatalha;
    public EventReference plateia;

    // OneShots
    public EventReference clique;
    public EventReference confirmar;
    public EventReference vozes;

    // Instâncias
    private EventInstance menuInstance;
    private EventInstance batalhaInstance;
    private EventInstance plateiaInstance;

    PLAYBACK_STATE estado;

    public void MudarVolume()
    {
        volume = slider.value;
        menuInstance.setVolume(volume);
        batalhaInstance.setVolume(volume);
        plateiaInstance.setVolume(volume);
    }

    private void Awake()
    {
        controleCena = GetComponentInParent<S_controleCena>();
        foreach (GameObject b in HB)
        {
            Button but = b.GetComponent<Button>();
            if (but != null) but.interactable = false;
        }
    }

    private void Start()
    {
        if (S_modoHistoria.aprendidos.Count == 0) for (int i = 0; i < 4; i++) S_modoHistoria.aprendidos.Add(S_verificaGolpe.Vgolpe.golpes[i]);
        PassarFase();

        menuInstance = RuntimeManager.CreateInstance(musicaMenu);
        batalhaInstance = RuntimeManager.CreateInstance(musicaBatalha);
        plateiaInstance = RuntimeManager.CreateInstance(plateia);

        menuInstance.start();
        plateiaInstance.start();
    }

    private void Update()
    {
        if (passandoT && !S_verificaGolpe.derrotou && !S_verificaGolpe.timeSlow) T += Time.unscaledDeltaTime;
        if (T >= 240)
        {
            T = 0;
            passandoT = false;
            SairPartida();
        }
        if (T > 5 && !TparaVozes)
        {
            StartCoroutine(Vozes());
        }

        for (int i = 0;i < TextosTelao.Length; i++) TextosTelao[i].text = "Tempo de jogo:" + "\n" + Mathf.RoundToInt(T);
    }

    IEnumerator TresDoisUm(bool qual)
    {
        GameObject maoa = Instantiate(MAOAfake);
        TresDosUm.gameObject.SetActive(true);
        TresDosUm.text = "3";
        yield return new WaitForSecondsRealtime(1);
        TresDosUm.text = "2";
        yield return new WaitForSecondsRealtime(1);
        TresDosUm.text = "1";
        yield return new WaitForSecondsRealtime(1);
        TresDosUm.text = "GO";
        yield return new WaitForSecondsRealtime(1);
        TresDosUm.gameObject.SetActive(false);
        Destroy(maoa);

        controleCena.ColocarMAOA(qual);
    }

    IEnumerator Vozes()
    {
        TparaVozes = true;

        float v = Random.Range(20, 35);
        float t = 0f;

        while (t < v)
        {
            if (!(UIs[7].activeInHierarchy))
            {
                yield break;
            }

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        S_onClique.PlayOneShot(vozes);

        TparaVozes = false;
    }

    public static void PlayOneShot(EventReference evento)
    {
        EventInstance instancia = RuntimeManager.CreateInstance(evento);
        instancia.setVolume(volume);
        instancia.start();
        instancia.release();
    }

    public void TrocaUI(int id) //sempre chamado quando troca a UI pra fechar todas
    {
        if (UIs.Length == 0) return;

        for (int i = 0; i < UIs.Length; i++)
        {
            if (i == id) UIs[i].SetActive(true);
            else UIs[i].SetActive(false);
        }

        T = 0;
        passandoT = false;

        S_onClique.PlayOneShot(clique);

        if (id == 7)
        {
            foreach (ParticleSystem p in bordas) p.Play();
            S_pontos.Spontos.pontos1 = 0;
            S_pontos.Spontos.pontos2 = 0;
            if (S_controleCena.modo == S_controleCena.ModoJogo.PvE) passandoT = true;
            menuInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            plateiaInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            batalhaInstance.start();
        }
        else if (id == 0)
        {
            foreach (ParticleSystem p in bordas) p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            menuInstance.getPlaybackState(out estado);
            if (!(estado == PLAYBACK_STATE.PLAYING))
            {
                menuInstance.start();
                plateiaInstance.start();
                batalhaInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        Tdificuldade.text = "" + Sbot_jogador.dificuldade;
    }

    public void DificuldadeBot(bool sobe)
    {
        S_onClique.PlayOneShot(clique);
        if (sobe && Sbot_jogador.dificuldade < 99) Sbot_jogador.dificuldade += 1;
        else if (!sobe && Sbot_jogador.dificuldade > 2) Sbot_jogador.dificuldade -= 1;

        Tdificuldade.text = "" + Sbot_jogador.dificuldade;
    }

    public void PlayBot(string teste)
    {
        S_onClique.PlayOneShot(confirmar);
        Sbot_jogador.naoMover = teste.Contains("t") ? true : false;
        if (teste.Contains("a")) Sbot_jogador.dificuldade = faseAtual * 2;
        naoAvanca = teste.Contains("a") ? false : true;

        Debug.Log(Sbot_jogador.naoMover);

        StartCoroutine(TresDoisUm(true));
        S_controleCena.modo = S_controleCena.ModoJogo.PvE;
        TrocaUI(7);
    }

    public void PlayHistory(int i)
    {
        S_onClique.PlayOneShot(confirmar);
        Sbot_jogador.naoMover = true;

        if (i == 0)
        {
            S_controleCena.modo = S_controleCena.ModoJogo.Tutorial;
            FindAnyObjectByType<S_controleTutorial>().enabled = true;
        }
        else if (i == -1)
        {
            S_controleCena.modo = S_controleCena.ModoJogo.Tutorial;
            FindAnyObjectByType<S_controleTutorial>().enabled = true;
        }
        else if (i >= 2)
        {
            S_controleCena.modo = S_controleCena.ModoJogo.Historia;

            int n = (i * 2) + 1;
            for (int j = 0; j < 2; j++)
            {
                S_modoHistoria.listaGolpes.Add(S_verificaGolpe.Vgolpe.golpes[n - j]);
                Debug.Log(S_verificaGolpe.Vgolpe.golpes[n - j].nome);
                Debug.Log(S_modoHistoria.listaGolpes.Count);
            }
        }
        controleCena.ColocarMAOA(true);
        TrocaUI(7);

        if (i == -1) StartCoroutine(FindAnyObjectByType<S_controleTutorial>().SprimeiraParte());
    }

    public void PassarFase()
    {
        Button butao = HB[faseAtual].GetComponent<Button>();
        if (butao != null)
        {
            faseAtual += 1;
            butao.interactable = true;

            if (faseAtual == 2 || faseAtual == 3 || faseAtual == 4) HB[faseAtual - 2].GetComponent<Button>().interactable = false;
        }
    }

    // - - - - - - - - - - J O G O - - - - - - - - - - //
    public void SairPartida()
    {
        S_onClique.PlayOneShot(clique);
        S_pontos.Spontos.pontos1 = 0;
        S_pontos.Spontos.pontos2 = 0;
        TrocaUI(0);
        naoAvanca = false;
        StartCoroutine(TresDoisUm(false));
    }

    public void PassaDialogo()
    {
        S_onClique.PlayOneShot(clique);
        S_controleTutorial.passa = true; 
    }
}
