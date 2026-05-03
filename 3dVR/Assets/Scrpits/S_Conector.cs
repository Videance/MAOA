using UnityEngine;

public class S_Conector : MonoBehaviour
{
    [Header("C - cotovelo")]
    [Header("J - joelho")]
    [Header("P - pescoço")]
    [Header("O - ombro (pescoço)")]
    [Header("Q - quadril")]
    [Header("- - - - -")]
    [Header("d - direito")]
    [Header("e - esquerdo")]
    [Header("m - esquerdo")]
    [Header("")]
    public Rigidbody rb;
    public Renderer rend;
    public string localDoCorpo;
    public S_IK maoOcupando;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>(); 
        rend = GetComponent<Renderer>();
    }
}
