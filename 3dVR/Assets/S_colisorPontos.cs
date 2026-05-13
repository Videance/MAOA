using UnityEngine;

public class S_colisorPontos : MonoBehaviour
{
    public static bool contaVitoria = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("ch") && contaVitoria && S_verificaGolpe.derrotou)
        {
            contaVitoria = false;
            S_colisorPontinhos.podecolidir = false;
            S_verificaGolpe.resetaCena = true;
        }
    }
}
