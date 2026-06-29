using UnityEngine;

public class S_colisorPontos : MonoBehaviour
{
    public static bool contaVitoria = true;
    public ParticleSystem estrelas;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("ch") && contaVitoria && S_verificaGolpe.derrotou && contaVitoria)
        {
            contaVitoria = false;
            S_colisorPontinhos.podecolidir = false;
            S_verificaGolpe.resetaCena = true;

            estrelas.Play(false);
        }
    }
}
