using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_onClique : MonoBehaviour
{
    S_controleCena controleCena;
    public GameObject[] UIs;
    public GameObject historiaButtons;
    public GameObject[] HB;
    S_verificaGolpe Svg;
    int faseAtual = 1;

    [Header("mover cabeça")]
    public GameObject CameraOffset;

    [Header("Textos")]
    public TextMeshPro TcamX;
    public TextMeshPro TcamY;
    public TextMeshPro Tdificuldade;
    public GameObject camOffset;

    [Header("Particulas")]
    public ParticleSystem[] bordas;

    private void Awake()
    {
        Svg = S_verificaGolpe.Vgolpe;
        controleCena = GetComponentInParent<S_controleCena>();
    }

    public void MoverCamera(int dir)
    {
        Vector3 cam = camOffset.transform.position;

        if (dir == 0 && camOffset.transform.position.x < 2f) cam.x += 0.1f;
        if (dir == 1 && camOffset.transform.position.x > -2f) cam.x -= 0.1f;
        if (dir == 2 && camOffset.transform.position.y < 2f) cam.y += 0.1f;
        if (dir == 3 && camOffset.transform.position.y > -2f) cam.y -= 0.1f;

        cam.x = Mathf.Round(cam.x * 10f) / 10f;
        cam.y = Mathf.Round(cam.y * 10f) / 10f;

        camOffset.transform.position = cam;

        TcamX.text = "" + cam.x;
        TcamY.text = "" + cam.y;
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
        controleCena.ColocarMAOA(true);
        S_controleCena.modo = S_controleCena.ModoJogo.PvE;
        TrocaUI(7);
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
        controleCena.ColocarMAOA(true);
        TrocaUI(7);
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
        controleCena.ColocarMAOA(false);
    }

    public void PassaDialogo() { S_controleTutorial.passa = true; }
}
