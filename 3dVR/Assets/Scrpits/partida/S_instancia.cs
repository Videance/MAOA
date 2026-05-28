using UnityEngine;

public class S_instancia : MonoBehaviour
{
    public GameObject luzes;
    public GameObject chao;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(luzes);
        DontDestroyOnLoad(chao);
        DontDestroyOnLoad(gameObject);
    }
}
