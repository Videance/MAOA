using UnityEngine;

public class S_colisorPontos : MonoBehaviour
{
    public bool contaVitoria = false;
    protected S_jogador jogador;

    private void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ch") && contaVitoria)
        {
            jogador.jogPontos = 2;
            S_verificaGolpe.resetaCena = true;
            contaVitoria = false;
        }
    }
}
