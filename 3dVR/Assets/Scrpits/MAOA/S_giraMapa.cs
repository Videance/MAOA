using UnityEngine;
using UnityEngine.SceneManagement;

public class S_giraMapa : MonoBehaviour
{
    private Vector3 offsetInicial;
    private Quaternion rotacaoInicial;
    Vector3 posInicial;

    private void Awake()
    {
        posInicial = transform.position;
    }

    void Start()
    {
        // Guarda posição relativa inicial
        offsetInicial = transform.position - S_moveTudo.quadra.transform.position;

        // Guarda rotação original
        rotacaoInicial = transform.rotation;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    void AoCarregarCena(Scene scene, LoadSceneMode mode)
    {
        transform.position = posInicial;
    }

    void Update()
    {
        if (S_verificaGolpe.timeSlow || S_moveTudo.quadra == null || S_controleTutorial.emTutorial || this == S_moveTudo.quadra) return;

        float dirX = S_moveTudo.J1dirX + S_moveTudo.J2dirX;

        // Movimento lateral
        if (dirX != 0f) transform.position += new Vector3(0, 0, dirX) * Time.deltaTime;

        // Rotaciona o offset junto da quadra
        Vector3 novoOffset = S_moveTudo.quadra.transform.rotation * offsetInicial;

        // Atualiza posição
        transform.position = S_moveTudo.quadra.transform.position + novoOffset;

        // Rotação da quadra + rotação original do objeto
        transform.rotation = S_moveTudo.quadra.transform.rotation * rotacaoInicial;
    }
}