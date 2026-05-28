using UnityEngine;

public class S_gira : MonoBehaviour
{
    public Vector3 eixo = new Vector3(0, 1, 0); // eixo de rotação (Y por padrão)
    public float velocidade = 100f;

    void Update() { transform.Rotate(eixo * velocidade * Time.deltaTime); }
}
