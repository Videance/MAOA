using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class S_pontos : MonoBehaviour
{
    public static S_pontos Spontos;
    public static List<Vector3> vitoriasXbot = new List<Vector3>(); //primeir é o nivel dele, o segundo a quantidade de virtórias, terceiro é tempo

    public S_jogador[] jogadores;

    public int pontos1; // jog
    public int pontos2; // bot

    void Awake()
    {
        if (Spontos == null) Spontos = this;
        CataJogadores();
    }

    public void CataJogadores()
    {
        var encontrados = FindObjectsByType<S_jogador>(FindObjectsSortMode.None);

        jogadores = new S_jogador[2];

        foreach (var jog in encontrados)
        {
            if (jog is Sbot_jogador) jogadores[1] = jog;
            else jogadores[0] = jog;
        }

        S_colisorPontinhos.podecolidir = true;
        S_colisorPontos.contaVitoria = true;
    }
}
