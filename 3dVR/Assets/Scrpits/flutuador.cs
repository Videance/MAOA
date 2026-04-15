using UnityEngine;

public class flutuador : MonoBehaviour
{
    public float altura = 0.1f;
    public float velocidade = 2f;

    private Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * velocidade) * altura;
        transform.position = posInicial + new Vector3(0, y, 0);
    }
}
