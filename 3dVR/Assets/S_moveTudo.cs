using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_moveTudo : MonoBehaviour
{
    public static float J1dirX = 0f;
    public static float J1dirY = 0f;

    public static float J2dirX = 0f;
    public static float J2dirY = 0f;

    public static GameObject quadra;

    Vector3 posInicial;
    Quaternion rotInicial;

    private void Awake()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>()) if (t.gameObject.GetNamedChild("Arena")) quadra = t.gameObject;
    }

    private void Start()
    {
        posInicial = transform.position;
        rotInicial = quadra.transform.rotation;
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
        quadra.transform.rotation = rotInicial;
    }

    private void Update()
    {
        if (S_verificaGolpe.timeSlow || quadra == null || S_controleTutorial.emTutorial) return;

        float dirX = J1dirX + J2dirX;
        float dirY = J1dirY + J2dirY;

        if (dirX != 0f) transform.position += new Vector3(0, 0, dirX) * Time.unscaledDeltaTime;
        if (dirY != 0f) quadra.transform.RotateAround(quadra.transform.position, Vector3.up, dirY * Time.unscaledDeltaTime);
    }
}
