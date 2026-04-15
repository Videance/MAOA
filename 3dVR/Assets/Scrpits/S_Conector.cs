using UnityEngine;

public class S_Conector : MonoBehaviour
{
    [Header("A - antebraço")]
    [Header("B - braço")]
    [Header("P - peito")]
    [Header("O - ombro (pescoço)")]
    [Header("C - costas")]
    [Header("- - - - -")]
    [Header("d - direito")]
    [Header("e - esquerdo")]
    [Header("")]
    public Rigidbody rb;
    public string localDoCorpo;
    public bool ocupado;

    private void Awake() { rb = GetComponent<Rigidbody>(); }
}
