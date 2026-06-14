using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class S_pontos : MonoBehaviour
{
    public static S_pontos Spontos;
    public static List<Vector2> vitoriasXbot = new List<Vector2>(); //primeir é o nivel dele, o segundo a quantidade de virtórias.

    public S_jogador[] jogadores;

    public int pontos1;
    public int pontos2;

    void OnEnable()
    {
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    void AoCarregarCena(Scene scene, LoadSceneMode mode)
    {
        var encontrados = FindObjectsByType<S_jogador>(FindObjectsSortMode.None);

        jogadores = new S_jogador[2];

        foreach (var jog in encontrados)
        {
            if (jog.transform.position.z < 0)
                jogadores[0] = jog;
            else
                jogadores[1] = jog;
        }

        S_colisorPontinhos.podecolidir = true;
        S_colisorPontos.contaVitoria = true;
    }

    void Awake()
    {
        if (Spontos == null) Spontos = this;
        var encontrados = FindObjectsByType<S_jogador>(FindObjectsSortMode.None);

        jogadores = new S_jogador[2];

        foreach (var jog in encontrados)
        {
            if (jog.transform.position.z < 0)
                jogadores[0] = jog;
            else
                jogadores[1] = jog; //bot
        }

        S_colisorPontinhos.podecolidir = true;
        S_colisorPontos.contaVitoria = true;
    }
}
