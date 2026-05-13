using UnityEngine;

public class S_pontoDes : MonoBehaviour
{
    public bool tocouClimax = false;
    public bool noCaminho = true;

    private Vector3 posInicial;
    public Vector3 dirFinal;

    void Start()
    {
        posInicial = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tt"))
        {
            tocouClimax = true;

            // direção do início até a posição atual
            dirFinal = transform.position - posInicial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("t"))
            noCaminho = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("t"))
            noCaminho = true;
    }
}
