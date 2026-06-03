using UnityEngine;

public class flutuador : MonoBehaviour
{
    public float altura = 0.1f;
    public float velocidade = 2f;

    void Update()
    {
        float y = Mathf.Sin(Time.time * velocidade) * altura;
        transform.position = new Vector3(0, -0.23f, 0) + new Vector3(0, y, 0);
    }
}
