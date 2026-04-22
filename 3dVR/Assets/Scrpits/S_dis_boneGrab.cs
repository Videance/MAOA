using UnityEngine;

public class S_dis_boneGrab : MonoBehaviour
{
    private S_IK ik;
    private Rigidbody rb;

    [Header("CALCULO DA DIS MAX")]
    public GameObject pai;
    public GameObject GOinicial;
    public GameObject GOponta;
    public GameObject maos;
    private float dis;

    private void Awake()
    {
        ik = GetComponent<S_IK>();
        rb = GetComponent<Rigidbody>();
        dis = Vector3.Distance(GOponta.transform.position, GOinicial.transform.position) * 1.05f;
    }

    // Update is called once per frame
    void Update()
    {
        // recolar o parent que o nearfar tira
        if (transform.parent == null) transform.SetParent(pai.transform);
        
        if (ik != null && ik.conectado)
        {
            transform.position = ik.conectado.transform.position;
            rb.angularVelocity = ik.conectado.GetComponent<Rigidbody>().angularVelocity;

            if (Vector3.Distance(ik.conectado.transform.position, GOinicial.transform.position) >= (dis * 1.1f)) ik.Desconecta();
        }

        // MOVIMENTO COM LIMITE MÁXIMO
        Vector3 dir = (transform.position - GOinicial.transform.position).normalized;

        if (Vector3.Distance(transform.position, GOinicial.transform.position) >= dis)
            transform.position = GOinicial.transform.position + dir * dis;
        if (Vector3.Distance(transform.position, GOinicial.transform.position) <= (dis * 0.3f))
            transform.position = GOinicial.transform.position + dir * (dis * 0.3f);
    }
}
