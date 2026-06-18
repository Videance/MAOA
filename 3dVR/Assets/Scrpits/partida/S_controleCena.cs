using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_controleCena : MonoBehaviour
{
    //cria um evento cm isso

    public static ModoJogo modo = ModoJogo.Tutorial;
    public GameObject JogadorPrefab;
    public GameObject AdversarioPrefab;

    List<GameObject> vivos = new List<GameObject>();

    public enum ModoJogo
    {
        Historia,
        PvE,
        Tutorial
    }

    public void ColocarMAOA(bool recria)
    {
        if (vivos.Count > 0) for (int i = 0;  i < vivos.Count; i++)
            {
                Destroy(vivos[i]);
                vivos.RemoveAt(i);
            }

        if (recria)
        {
            GameObject jogador = Instantiate(JogadorPrefab);
            vivos.Add(jogador);
            GameObject adversario = Instantiate(AdversarioPrefab);
            vivos.Add(adversario);
        }
    }
}
