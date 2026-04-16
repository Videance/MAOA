using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_imaos : MonoBehaviour //controla apenas stamina e solta o S_dis_BoneGrab
{
    [Header("SPRITES")]
    public SpriteRenderer Renderer;
    public List<Sprite> renBateria = new List<Sprite>();

    [Header("STAMINA")]
    public int staminaMax = 100;
    public int stamina = 100;
    public float tempo = 0;

    [Header("CONEXÃO")]
    public XRBaseInteractor nearfar;
    private S_IK IK;

    private bool rodandoSS;

    private void Start()
    {
        IK = GetComponent<S_IK>();
    }

    private void Update()
    {
        if (rodandoSS || stamina >= staminaMax) return;

        tempo += Time.deltaTime;
        if (stamina <= 0)
        {
            tempo = 0;
            StartCoroutine(SemStamina());
        }
        else if (tempo >= 0.25f && IK.conectado == null)
        {
            tempo = 0;
            stamina += 2;
        }
        else if (tempo >= 1f)
        {
            tempo = 0;
            stamina -= 1;
        }

        if (stamina > staminaMax || stamina < 0) stamina = Mathf.Clamp(stamina, 0, staminaMax);

        // troca imagem da bateria
        if (stamina > 80) Renderer.sprite = renBateria[0];
        else if (stamina > 60) Renderer.sprite = renBateria[1];
        else if (stamina > 40) Renderer.sprite = renBateria[2];
        else if (stamina > 20) Renderer.sprite = renBateria[3];
        else if (stamina > 0) Renderer.sprite = renBateria[4];
        else if (stamina == 0) Renderer.sprite = renBateria[5];
    }

    IEnumerator SemStamina()
    {
        rodandoSS = true;
        IK.Desconecta();
        nearfar.allowSelect = false;

        yield return new WaitForSeconds(3f);
        int i = 6;

        while (stamina < staminaMax)
        {
            stamina += 20;
            Renderer.sprite = renBateria[i];
            if (i < 9) i++;
            if (stamina < staminaMax) yield return new WaitForSeconds(0.25f);
        }

        Renderer.sprite = renBateria[0];
        stamina = Mathf.Clamp(stamina, 0, staminaMax);
        nearfar.allowSelect = true;
        rodandoSS = false;
    }
}
