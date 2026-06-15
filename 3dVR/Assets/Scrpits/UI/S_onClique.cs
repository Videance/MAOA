using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_onClique : MonoBehaviour
{
    public GameObject[] UIs;
    public GameObject historiaButtons;
    GameObject[] HB;
    S_verificaGolpe Svg;
    int faseAtual = 1;

    [Header("mover cabeça")]
    public GameObject CameraOffset;
    float camX = 0f;
    float camY = 0f;

    [Header("Textos")]
    public TextMeshPro TcamX;
    public TextMeshPro TcamY;
    public TextMeshPro Tdificuldade;

    [Header("Particulas")]
    public ParticleSystem[] bordas;

    private void Awake()
    {
        Svg = S_verificaGolpe.Vgolpe;
        HB = historiaButtons.GetComponentsInChildren<GameObject>();
    }

    public void MoverCamera(int dir)
    {
        if (dir == 0 && camX < 2f) camX += 0.1f;
        if (dir == 1 && camX > -2f) camX -= 0.1f;
        if (dir == 2 && camY < 2f) camY += 0.1f;
        if (dir == 3 && camY > -2f) camY -= 0.1f;

        TcamX.text = "" + camX;
        TcamY.text = "" + camY;
    }

    public void TrocaUI(int id) //sempre chamado quando troca a UI pra fechar todas
    {
        if (UIs.Length == 0) return;

        for (int i = 0; i < UIs.Length; i++)
        {
            if (i == id) UIs[i].SetActive(true);
            else UIs[i].SetActive(false);
        }

        if (id == 8) foreach (ParticleSystem p in bordas) p.Play();
        else if (id == 0) foreach (ParticleSystem p in bordas) p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void DificuldadeBot(bool sobe)
    {
        if (sobe && Sbot_jogador.dificuldade < 99) Sbot_jogador.dificuldade += 1;
        else if (!sobe && Sbot_jogador.dificuldade > 2) Sbot_jogador.dificuldade -= 1;

        Tdificuldade.text = "" + Sbot_jogador.dificuldade;
    }

    public void PlayBot()
    {
        SceneManager.LoadScene("MAOA vdd", LoadSceneMode.Additive);
        S_controleCena.modo = S_controleCena.ModoJogo.PvE;
        TrocaUI(8);
    }

    public void PlayHistory(int i)
    {
        if (i == 0) S_controleCena.modo = S_controleCena.ModoJogo.Tutorial;
        else if (i != 0)
        {
            S_controleCena.modo = S_controleCena.ModoJogo.Historia;

            int n = (i * 5) - 1;
            for (int j = 0; j < 5; j++) S_modoHistoria.listaGolpes[j] = Svg.golpes[n - j];
        }
        SceneManager.LoadScene("MAOA vdd", LoadSceneMode.Additive);
        TrocaUI(8);
    }

    public void PassarFase()
    {
        Button butao = HB[faseAtual].GetComponent<Button>();
        if (butao != null && S_controleCena.modo != S_controleCena.ModoJogo.PvE)
        {
            faseAtual += 1;
            butao.interactable = true;
        }
        else if (S_controleCena.modo == S_controleCena.ModoJogo.PvE)
        {
            float p = 0;
            int lv = faseAtual * 2;
            foreach (Vector2 v in S_pontos.vitoriasXbot) 
                if (v.x >= lv)
                {
                    p += v.y;
                    if (p > 2)
                    {
                        faseAtual += 1;
                        PassarFase();
                        break;
                    }
                }
        }
    }

    IEnumerator ensinaGolpes()
    {
        //yield return new WaitUntil(() => Cena "MAOA vdd" reze);
        //espera o evengo

        yield return null;
    }

    // - - - - - - - - - - J O G O - - - - - - - - - - //
    public void SairPartida()
    {
        TrocaUI(0);
        SceneManager.UnloadSceneAsync("MAOA vdd");
    }

    public void PassaDialogo() { S_controleTutorial.passa = true; }
}
