using UnityEngine;

public class S_colisorPontinhos : S_colisorPontos
{
    public static bool podecolidir = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ch") && podecolidir && S_verificaGolpe.derrotou)
        {
            contaVitoria = false;
            podecolidir = false;
            S_verificaGolpe.tempo = 0f;
        }
    }
}
