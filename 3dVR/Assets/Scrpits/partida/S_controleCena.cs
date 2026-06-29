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

    private void Awake()
    {
        SaveManager.Carregar();
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

        S_giraMapa[] move = FindObjectsOfType<S_giraMapa>();
        for (int i = 0; i < move.Length; i++) move[i].ResetaMapa();
        S_moveTudo[] move2 = FindObjectsOfType<S_moveTudo>();
        for (int i = 0; i < move2.Length; i++) move2[i].ResetaMapa();
    }
}
