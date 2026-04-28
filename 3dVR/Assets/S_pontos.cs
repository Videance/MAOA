using UnityEngine;

public class S_pontos : MonoBehaviour
{
    public static S_pontos Spontos;

    S_jogador[] jogadores = new S_jogador[2];

    public int pontos1;
    public int pontos2;

    void Awake()
    {
        if (Spontos == null)
        {
            Spontos = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        var objs = FindObjectsByType<S_jogador>(FindObjectsSortMode.None);

        foreach (var obj in objs)
        {
            if (jogadores[0] == null)
                jogadores[0] = obj;
            else if (obj != jogadores[0])
            {
                jogadores[1] = obj;
                break;
            }
        }
    }

    private void Update()
    {
        pontos1 = jogadores[0].jogPontos;
        pontos2 = jogadores[1].jogPontos;
    }
}
