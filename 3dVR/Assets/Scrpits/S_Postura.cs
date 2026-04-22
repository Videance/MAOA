using Unity.Mathematics;
using UnityEngine;

public class S_Postura : MonoBehaviour
{
    private S_energia energia;
    private S_jogador jogador;
    public GameObject pDireita;
    public GameObject pEsquerda;

    public float distZ;

    public float distEquilibrio;
    public float XYdirEquilibrio;

    private void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
        energia = GetComponentInParent<S_energia>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pDireita == null || pEsquerda == null || pEsquerda.transform.position == pDireita.transform.position) return;

        Vector3 pDir = new Vector3(0f, 0f, pDireita.transform.position.z);
        Vector3 pEsq = new Vector3(0f, 0f, pEsquerda.transform.position.z);
        distZ = Vector3.Distance(pDir, pEsq);

        if (distZ > 0.15f) TrocaPostura(true);
        else TrocaPostura(false);
    }

    void TrocaPostura(bool aberta)
    {
        if (jogador.pernaAberta == aberta) return;
        jogador.pernaAberta = aberta;
        S_verificaGolpe.AcharGolpe(jogador, jogador.adversario);
        energia.energia -= 3;
    }
}
