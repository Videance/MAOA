using System.Collections;
using UnityEngine;

public class S_modoHistoria : MonoBehaviour
{
    S_jogador jogador;
    public SpriteRenderer render;
    public static C_golpes[] listaGolpes;

    private void Awake()
    {
        if (S_controleCena.modo != S_controleCena.ModoJogo.Historia) enabled = false;
        jogador = GetComponent<S_jogador>();
        jogador.adversario.GetComponent<Sbot_jogador>().enabled = false;
    }

    public IEnumerator EnsinarGolpes()
    {
        //render.sprite = imagem;
        yield return null;
    }
}
