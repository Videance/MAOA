using UnityEngine;

public class S_segueC : MonoBehaviour
{
    private Rigidbody rb;
    private float gravidadeExtra = 1.4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    public void Joga(Vector3 dir)
    {
        rb.linearVelocity = dir * 18f;
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravidadeExtra, ForceMode.Acceleration);
    }
}
