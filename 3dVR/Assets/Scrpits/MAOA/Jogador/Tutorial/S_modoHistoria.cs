using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_modoHistoria : MonoBehaviour
{
    S_jogador jogador;
    public SpriteRenderer render;
    public static List<C_golpes> listaGolpes = new();
    public static List<C_golpes> aprendidos = new();

    private void Awake()
    {
        if (S_controleCena.modo != S_controleCena.ModoJogo.Historia) enabled = false;
        jogador = GetComponent<S_jogador>();
        jogador.adversario.GetComponent<Sbot_jogador>().enabled = false;
    }
}
