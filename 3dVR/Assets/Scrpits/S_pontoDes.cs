using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_pontoDes : MonoBehaviour
{
    public bool tocouClimax = false;
    public bool jogado = false;
    public bool noCaminho = true;
    XRGrabInteractable grab;
    private bool primera = false;

    private void Start() { grab = GetComponent<XRGrabInteractable>(); }
    private void Update()
    {
        if (grab.isSelected) primera = true;
        else if (primera == true) jogado = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tt"))
        {
            tocouClimax = true;
            jogado = true;
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
