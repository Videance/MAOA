using UnityEngine;

public class S_instancia : MonoBehaviour
{
    public static S_instancia StextNum;
    public TextMesh texto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (StextNum == null)
        {
            StextNum = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        texto = GetComponent<TextMesh>();
    }
}
