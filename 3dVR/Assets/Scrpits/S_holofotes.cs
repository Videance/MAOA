using UnityEngine;

public class S_holofotes : MonoBehaviour
{
    [Header("Velocidade")]
    public float velocidadeRotacao = 25f;

    [Header("Limites")]
    public float limiteHorizontal = 90f;
    public float limiteVertical = 40f;

    [Header("Tempo")]
    public float tempoTrocaDirecao = 0.6f;

    [Header("Alvo")]
    public Transform alvo;

    [Header("Config")]
    public bool seguirAlvo = true;

    private Quaternion rotacaoAlvo;
    private float timer;

    void Start()
    {
        NovaDirecao();
    }

    void Update()
    {
        // ===== MODO SEGUIR ALVO =====
        if (seguirAlvo && alvo != null)
        {
            Vector3 direcao = alvo.position - transform.position;

            Quaternion alvoRot = Quaternion.LookRotation(direcao);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                alvoRot,
                velocidadeRotacao * 3 * Time.deltaTime
            );
        }

        // ===== MODO ALEATÓRIO =====
        else
        {
            if (S_colisorPontinhos.podecolidir || S_colisorPontos.contaVitoria)
                return;

            timer -= Time.deltaTime;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacaoAlvo,
                velocidadeRotacao * Time.deltaTime
            );

            if (timer <= 0f)
            {
                NovaDirecao();
            }
        }
    }

    void NovaDirecao()
    {
        timer = Random.Range(
            tempoTrocaDirecao * 0.2f,
            tempoTrocaDirecao * 0.5f
        );

        float rotY = Random.Range(-limiteHorizontal, limiteHorizontal);
        float rotX = Random.Range(-limiteVertical, limiteVertical);

        rotacaoAlvo = Quaternion.Euler(rotX, rotY, 0f);
    }
}