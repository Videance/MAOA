using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_onClique : MonoBehaviour
{
    S_controleCena controleCena;
    public GameObject[] UIs;
    public GameObject historiaButtons;
    public GameObject[] HB;
    S_verificaGolpe Svg;
    int faseAtual = 0;
    bool passandoT = false;
    public static float T = 0;

    [Header("mover cabeça")]
    public GameObject CameraOffset;

    [Header("Textos")]
    public TextMeshPro TcamX;
    public TextMeshPro TcamY;
    public TextMeshPro Tdificuldade;
    public GameObject camOffset;
    public TextMeshPro[] TextosTelao;

    [Header("Particulas")]
    public ParticleSystem[] bordas;

    private void Awake()
    {
        Svg = S_verificaGolpe.Vgolpe;
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

        for (int i = 0;i < TextosTelao.Length; i++) TextosTelao[i].text = "Tempo de jogo:" + "\n" + T;
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

        if (id == 7)
        {
            foreach (ParticleSystem p in bordas) p.Play();
            S_pontos.Spontos.pontos1 = 0;
            S_pontos.Spontos.pontos2 = 0;
            if (S_controleCena.modo == S_controleCena.ModoJogo.PvE) passandoT = true;
        }
        else if (id == 0) foreach (ParticleSystem p in bordas) p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Tdificuldade.text = "" + Sbot_jogador.dificuldade;
    }

    public void DificuldadeBot(bool sobe)
    {
        if (sobe && Sbot_jogador.dificuldade < 99) Sbot_jogador.dificuldade += 1;
        else if (!sobe && Sbot_jogador.dificuldade > 2) Sbot_jogador.dificuldade -= 1;

        Tdificuldade.text = "" + Sbot_jogador.dificuldade;
    }

    public void PlayBot(bool teste)
    {
        controleCena.ColocarMAOA(true);
        if (!teste) Sbot_jogador.naoMover = false;
        else Sbot_jogador.naoMover = true;
        S_controleCena.modo = S_controleCena.ModoJogo.PvE;
        TrocaUI(7);
    }

    public void PlayHistory(int i)
    {
        if (i == 0)
        {
            S_controleCena.modo = S_controleCena.ModoJogo.Tutorial;
            Sbot_jogador.naoMover = true;
            FindAnyObjectByType<S_controleTutorial>().enabled = true;
        }
        else if (i == -1)
        {
            S_controleCena.modo = S_controleCena.ModoJogo.Tutorial;
            Sbot_jogador.naoMover = true;
            FindAnyObjectByType<S_controleTutorial>().enabled = true;
        }
        else if (i != 0)
        {
            S_controleCena.modo = S_controleCena.ModoJogo.Historia;

            int n = (i * 4) - 1;
            for (int j = 0; j < 4; j++)
            {
                S_modoHistoria.listaGolpes[j] = Svg.golpes[n - j];
                Debug.Log(Svg.golpes[n - j].nome);
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
        }
        else if (S_controleCena.modo == S_controleCena.ModoJogo.PvE)
        {
            float p = 0;
            int lv = faseAtual * 2;
            foreach (Vector3 v in S_pontos.vitoriasXbot)
            {
                Debug.Log(v);

                if (v.x >= lv)
                {
                    p += v.y;
                    if (p > 3)
                    {
                        faseAtual += 1;
                        PassarFase();
                        break;
                    }
                }
            }
        }
    }

    // - - - - - - - - - - J O G O - - - - - - - - - - //
    public void SairPartida()
    {
        S_pontos.Spontos.pontos1 = 0;
        S_pontos.Spontos.pontos2 = 0;
        TrocaUI(0);
        controleCena.ColocarMAOA(false);
    }

    public void PassaDialogo() { S_controleTutorial.passa = true; }
}
