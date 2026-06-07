using UnityEngine;

public class S_maoTutorial : MonoBehaviour
{
    S_controleTutorial Stutorial;
    public bool ladoDir;

    private void Awake()
    {
        Stutorial = GetComponentInParent<S_controleTutorial>();
    }
}
