using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_controleCena : MonoBehaviour
{
    //cria um evento cm isso

    public static ModoJogo modo = ModoJogo.Tutorial;
    public GameObject JogadoresPrefab;
    public GameObject Jogadores; 

    public enum ModoJogo
    {
        Historia,
        PvE,
        Tutorial
    }

    public void ColocarMAOA(bool recria)
    {
        if (Jogadores != null) Destroy(Jogadores);

        if (recria)
        {
            GameObject jogador = Instantiate(JogadoresPrefab);
            Jogadores = jogador;
        }
    }
}
