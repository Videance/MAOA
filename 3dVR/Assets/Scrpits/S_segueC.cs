using UnityEngine;

public class S_segueC : MonoBehaviour
{
    public S_pontoDes SpontoDes;
    private S_jogador jogador;
    public Rigidbody pDes;
    private Rigidbody rb;
    private S_colisorPontos Scp;
    private float gravidadeExtra = 1.7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        jogador = GetComponentInParent<S_jogador>();
        Scp = GetComponentInChildren<S_colisorPontos>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpontoDes == null) return;

        if (jogador.emRagdoll && SpontoDes.noCaminho) rb.linearVelocity += pDes.linearVelocity / 7f;
        if (SpontoDes.tocouClimax)
        {
            Scp.contaVitoria = true;
            rb.linearVelocity += rb.linearVelocity * 4f;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravidadeExtra, ForceMode.Acceleration);
    }
}
