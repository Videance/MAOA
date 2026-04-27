using UnityEngine;

public class S_rb : MonoBehaviour
{
    public Rigidbody rb;
    void Start() {  rb = GetComponent<Rigidbody>(); }
}
