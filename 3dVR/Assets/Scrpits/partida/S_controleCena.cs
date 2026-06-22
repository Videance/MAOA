using UnityEngine;

public class S_controleCena : MonoBehaviour
{
    public S_controleTutorial Sct;

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

            jogador.GetComponentInChildren<Sbot_jogador>().enabled = true;

            if (Sct.enabled == true) Sct.PegarVar();

            S_pontos.Spontos.CataJogadores();
        }
    }
}
