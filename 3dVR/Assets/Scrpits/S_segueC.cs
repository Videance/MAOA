using UnityEngine;

public class S_segueC : MonoBehaviour
{
    public S_pontoDes SpontoDes;
    private Rigidbody rb;
    private S_colisorPontos Scp;
    private float gravidadeExtra = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
        Scp = GetComponentInChildren<S_colisorPontos>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpontoDes == null) return;

        if (SpontoDes.tocouClimax)
        {
            Scp.contaVitoria = true;

            Debug.Log("TA ME CHAMANDO MEN´´O?");
            rb.linearVelocity = SpontoDes.dirFinal * 30f;

            SpontoDes.tocouClimax = false;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravidadeExtra, ForceMode.Acceleration);
    }
}
