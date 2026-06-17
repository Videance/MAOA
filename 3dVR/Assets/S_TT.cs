using UnityEngine;

public class S_TT : MonoBehaviour
{
    public static GameObject TTinstance;

    private void Awake()
    {
        if (TTinstance == null) TTinstance = gameObject;
        else Destroy(gameObject);
    }
}
