using UnityEngine;

public class S_colisorPontinhos : S_colisorPontos
{
    bool podecolidir = true;

    private void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ch") && podecolidir && S_verificaGolpe.timeSlow)
        {
            podecolidir = false;
            S_verificaGolpe.tempo = 0f;
        }
    }
}
