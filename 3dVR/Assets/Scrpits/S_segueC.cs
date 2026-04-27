using UnityEngine;

public class S_segueC : MonoBehaviour
{
    public S_pontoDes SpontoDes;
    private S_jogador jogador;
    public Rigidbody pDes;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        jogador = GetComponentInParent<S_jogador>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jogador.emRagdoll && SpontoDes.noCaminho) rb.linearVelocity += pDes.linearVelocity / 4f;
        if (SpontoDes.tocouClimax) rb.linearVelocity += rb.linearVelocity * 4;
    }
}
