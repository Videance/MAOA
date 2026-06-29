using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_moveTudo : MonoBehaviour
{
    public static float J1dirX = 0f;
    public static float J1dirY = 0f;

    public static float J2dirX = 0f;
    public static float J2dirY = 0f;

    public static GameObject quadra;

    public List<GameObject> objetosQandam = new List<GameObject>();

    private void Awake()
    {
        quadra = objetosQandam[0];
    }

    public void ResetaMapa()
    {
        quadra.transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (S_verificaGolpe.timeSlow || quadra == null || S_controleTutorial.tutorial1 || S_controleCena.Jogadores == null) return;

        float dirX = J1dirX + J2dirX;
        float dirY = J1dirY + J2dirY;

        if (dirX != 0f) for (int i = 0; i < objetosQandam.Count; i++) objetosQandam[i].transform.position += new Vector3(0, 0, dirX) * Time.unscaledDeltaTime;
        if (dirY != 0f) quadra.transform.RotateAround(quadra.transform.position, Vector3.up, dirY * Time.unscaledDeltaTime);
    }
}
