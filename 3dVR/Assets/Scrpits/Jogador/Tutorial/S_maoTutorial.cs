using UnityEngine;

public class S_maoTutorial : MonoBehaviour
{
    S_controleTutorial Stutorial;
    public bool ladoDir;

    private void Awake()
    {
        Stutorial = GetComponentInParent<S_controleTutorial>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Stutorial.Sparte == true && other.gameObject.CompareTag("IK"))
        {
            if (ladoDir) Stutorial.tocou[0] = true;
            else if (!ladoDir) Stutorial.tocou[1] = true;
        }
    }
}
