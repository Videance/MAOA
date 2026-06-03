using UnityEngine;

public class S_Postura : MonoBehaviour
{
    public S_jogador jogador;

    public GameObject pDireita;
    public GameObject pEsquerda;

    public float distZ;

    public string posPerna;

    void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
    }

    void Update()
    {
        if (pDireita == null || pEsquerda == null || pEsquerda.transform.position == pDireita.transform.position)
            return;

        Vector3 pDir = new Vector3(0f, 0f, pDireita.transform.position.z);
        Vector3 pEsq = new Vector3(0f, 0f, pEsquerda.transform.position.z);

        distZ = Vector3.Distance(pDir, pEsq);

        if (distZ < 0.9f) TrocaPostura("F");

        else if (distZ > 1.5f)
        {
            if (jogador.transform.position.z < 0)
            {
                if (pDireita.transform.position.z > jogador.transform.position.z) TrocaPostura("Ad");
                if (pEsquerda.transform.position.z > jogador.transform.position.z) TrocaPostura("Ae");
            }
            else
            {
                if (pDireita.transform.position.z > jogador.transform.position.z) TrocaPostura("Ae");
                if (pEsquerda.transform.position.z > jogador.transform.position.z) TrocaPostura("Ad");
            }
        }
    }

    void TrocaPostura(string postura)
    {
        if (jogador.posPerna == postura) return;
        jogador.posPerna = postura;

        if (jogador is Sbot_jogador) ((Sbot_jogador)jogador).VerificaVar(1);
        else S_verificaGolpe.Vgolpe.AcharGolpe(jogador, jogador.adversario);
    }
}