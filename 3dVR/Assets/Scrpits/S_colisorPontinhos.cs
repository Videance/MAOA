using UnityEngine;

public class S_colisorPontinhos : S_colisorPontos
{
    bool podecolidir = true;
    float t = 0f;

    private void Start()
    {
        t = 0f;
        jogador = GetComponentInParent<S_jogador>();
    }

    private void Update()
    {
        if (Time.timeScale != 1f && !S_verificaGolpe.timeSlow) return;

        t += Time.unscaledDeltaTime;

        if (t >= 10f)
        {
            jogador.jogPontos += 1;
            S_verificaGolpe.resetaCena = true;
            podecolidir = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ch") && podecolidir)
        {
            podecolidir = false;
            jogador.jogPontos += 1;
            S_verificaGolpe.resetaCena = true;
        }
    }
}
