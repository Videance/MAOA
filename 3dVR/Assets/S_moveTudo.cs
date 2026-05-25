using UnityEngine;

public class S_moveTudo : MonoBehaviour
{
    public static float J1dirX = 0f;
    public static float J1dirY = 0f;

    public static float J2dirX = 0f;
    public static float J2dirY = 0f;

    public static GameObject quadra;

    private void Awake()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>()) if (t.gameObject.CompareTag("ch")) quadra = t.gameObject;
    }

    private void Update()
    {
        if (S_verificaGolpe.timeSlow || quadra == null) return;

        float dirX = J1dirX + J2dirX;
        float dirY = J1dirY + J2dirY;

        if (dirX != 0f) transform.position += new Vector3(dirX, 0, 0) * Time.unscaledDeltaTime;
        if (dirY != 0f) quadra.transform.RotateAround(quadra.transform.position, Vector3.up, dirY * Time.unscaledDeltaTime);
    }
}
