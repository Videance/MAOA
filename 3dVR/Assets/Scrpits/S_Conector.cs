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
    private CharacterJoint bone;
    public string localDoCorpo;
    public bool ocupado;

    private void Awake()
    { 
        rb = GetComponent<Rigidbody>(); 
        bone = GetComponentInParent<CharacterJoint>();
    }
}
