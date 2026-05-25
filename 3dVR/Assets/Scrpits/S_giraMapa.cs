using UnityEngine;

public class S_giraMapa : MonoBehaviour
{
    private Vector3 offsetInicial;

    void Start()
    {
        // Guarda a posição relativa inicial em relação à quadra
        offsetInicial = transform.position - S_moveTudo.quadra.transform.position;
    }

    void Update()
    {
        float dirX = S_moveTudo.J1dirX + S_moveTudo.J2dirX;

        // Movimento lateral normal
        if (dirX != 0f)
            transform.position += new Vector3(dirX, 0, 0) * Time.deltaTime;

        // Aplica a rotação da quadra ao offset
        Vector3 novoOffset = S_moveTudo.quadra.transform.rotation * offsetInicial;

        // Reposiciona ao redor da quadra
        transform.position = S_moveTudo.quadra.transform.position + novoOffset;

        // Opcional: também copia a rotação
        transform.rotation = S_moveTudo.quadra.transform.rotation;
    }
}