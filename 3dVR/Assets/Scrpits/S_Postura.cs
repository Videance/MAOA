using UnityEngine;

public class S_Postura : MonoBehaviour
{
    private S_energia energia;
    public S_jogador jogador;
    public GameObject pDireita;
    public GameObject pEsquerda;

    public Sprite[] sprites = new Sprite[2];
    public SpriteRenderer render;

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
        else if (distZ < 0.1f) TrocaPostura(false);
    }

    void TrocaPostura(bool aberta)
    {
        if (jogador.pernaAberta == aberta) return;
        jogador.pernaAberta = aberta;
        if (aberta) render.sprite = sprites[1];
        else render.sprite = sprites[0];

        if (jogador is Sbot_jogador) ((Sbot_jogador)jogador).VerificaVar(1);
        else S_verificaGolpe.Vgolpe.AcharGolpe(jogador, jogador.adversario);
    }
}
