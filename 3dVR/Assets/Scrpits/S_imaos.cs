using System.Collections;
using UnityEngine;

public class S_imaos : MonoBehaviour //controla apenas stamina e solta o S_dis_BoneGrab
{
    [Header("STAMINA")]
    public int staminaMax = 100;
    public int stamina = 100;
    public float tempo = 0;

    [Header("CONEXÃO")]
    public string maoDoLado;
    public S_IK disBoneGrab;

    private bool rodandoSS;

    private void Update()
    {
        if (rodandoSS || stamina >= staminaMax) return;

        tempo += Time.deltaTime;
        if (stamina <= 0)
        {
            tempo = 0;
            StartCoroutine(SemStamina());
        }
        else if (tempo >= 0.25f && disBoneGrab.conectado == null)
        {
            tempo = 0;
            stamina += 2;
        }
        else if (tempo >= 1f)
        {
            tempo = 0;
            stamina += 1;
        }

        if (stamina > staminaMax || stamina < 0) stamina = Mathf.Clamp(stamina, 0, staminaMax);
    }

    IEnumerator SemStamina()
    {
        var originalLayer = disBoneGrab.grab.interactionLayers;

        rodandoSS = true;
        disBoneGrab.Desconecta();
        disBoneGrab.grab.interactionLayers = 10;
        yield return new WaitForSeconds(0.25f);

        while (stamina < staminaMax)
        {
            stamina += 10;
            yield return new WaitForSeconds(0.25f);
        }

        stamina = Mathf.Clamp(stamina, 0, staminaMax);
        disBoneGrab.grab.interactionLayers = originalLayer;
        rodandoSS = false;
    }
}
