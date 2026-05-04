using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_energia : MonoBehaviour //controla apenas stamina e solta o S_dis_BoneGrab
{
    [Header("SPRITES")]
    public SpriteRenderer[] Renderer;
    public List<Sprite> renBateria = new List<Sprite>();

    [Header("%")]
    public TextMesh[] texto;

    [Header("STAMINA")]
    private int n = 0;
    public int energiaMax = 100;
    public float energia;
    public bool rodandoSS;

    [Header("CONEXÃO")]
    public S_IK[] IK;
    public XRBaseInteractor[] maos;

    private void Start()
    {
        energia = energiaMax;
        Renderer = GetComponentsInChildren<SpriteRenderer>().Take(2).ToArray();
        IK = GetComponentsInChildren<S_IK>().Take(2).ToArray();
        texto = GetComponentsInChildren<TextMesh>();
        maos = GetComponentsInChildren<XRBaseInteractor>().Take(4).ToArray();
    }

    private void Update()
    {
        if (S_verificaGolpe.timeSlow) return;

        // troca imagem da bateria
        if (energia > 80) TrocaSprite(0);
        else if (energia > 60) TrocaSprite(n+1);
        else if (energia > 40) TrocaSprite(n+2);
        else if (energia > 20) TrocaSprite(n+3);
        else if (energia > 0) TrocaSprite(n+4);
        else if (energia == 0) TrocaSprite(5);

        if (rodandoSS) return;

        if (energia > 0)
        {
            energia -= Time.deltaTime / 3;

            int q = 0;
            foreach (var i in IK) if (i.conectado) { q+=2; }
            if (q > 0) energia -= Time.deltaTime * q;
        }
        else StartCoroutine(SemStamina());

        if (energia > energiaMax || energia < 0) energia = Mathf.Clamp(energia, 0, energiaMax);

        //troca o texto da bateria
        foreach (var i in texto)
        {
            i.text = Mathf.RoundToInt(energia).ToString() + "%";
            if (energia > 0 && i.text == "0%") i.text = "1%";
            if (energia == 0) i.text = "out";
        }
    }

    void TrocaSprite(int cargas) { foreach (var render in Renderer) { render.sprite = renBateria[cargas]; } }

    IEnumerator SemStamina()
    {
        if (S_verificaGolpe.timeSlow) yield break;

        rodandoSS = true;
        foreach (var i in IK)
        {
            i.trocaEstado(S_IK.estadoMao.desativada);
            if (i.conectado) i.Desconecta();
        }
        foreach (var i in maos) i.GetComponent<XRBaseInteractor>().allowSelect = false; 
        
        n = 5;

        yield return new WaitForSeconds(2f);

        while (energia < energiaMax)
        {
            energia += 20;
            if (energia < energiaMax) yield return new WaitForSeconds(0.25f);
        }

        energia = Mathf.Clamp(energia, 0, energiaMax);
        n = 0;
        foreach (var i in IK) i.trocaEstado(S_IK.estadoMao.livre);
        foreach (var i in maos) i.GetComponent<XRBaseInteractor>().allowSelect = true;
        rodandoSS = false;
    }

    public void DesativaEnergia()
    {
        foreach (var i in IK) if (i.conectado) i.Desconecta();
        foreach (var i in maos) i.GetComponent<XRBaseInteractor>().allowSelect = false;
    }
}
