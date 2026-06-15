using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_energia : MonoBehaviour //controla apenas stamina e solta o S_dis_BoneGrab
{
    S_jogador Jogador;

    [Header("%")]
    public TextMesh[] texto;

    [Header("STAMINA")]
    protected int n = 0;
    public float energiaMax = 100;
    public float energia;
    public bool rodandoSS;

    [Header("CONEXÃO")]
    public S_IK[] IK;
    public XRBaseInteractor[] maos;

    public ParticleSystem atordoado;

    private void Start()
    {
        energia = energiaMax;
        Jogador = GetComponent<S_jogador>();
        IK = GetComponentsInChildren<S_IK>().Take(2).ToArray();
        texto = GetComponentsInChildren<TextMesh>();
        maos = GetComponentsInChildren<XRBaseInteractor>().Take(4).ToArray();
    }

    private void Update()
    {
        if (S_verificaGolpe.timeSlow) return;

        if (rodandoSS) return;

        if (energia > 0)
        {
            energia -= Time.deltaTime / 3;

            if (!S_controleTutorial.emTutorial)
            {
                int q = 0;
                foreach (var i in IK) if (i.aberto) { q += 1; }
                if (Jogador.posPerna.Contains("A")) q += 1;
                if (q > 0) energia -= Time.deltaTime * q;
            }
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

    IEnumerator SemStamina()
    {
        if (S_verificaGolpe.timeSlow) yield break;

        atordoado.Play();

        rodandoSS = true;
        foreach (var i in IK)
        {
            i.trocaEstado(S_IK.estadoMao.desativada);
            if (i.conectado) i.Desconecta();
        }
        foreach (var i in maos) i.GetComponent<XRBaseInteractor>().allowSelect = false; 
        
        n = 5;

        yield return new WaitForSeconds(3.25f);

        while (energia < energiaMax)
        {
            energia += energiaMax * 0.25f;
            if (energia < energiaMax) yield return new WaitForSeconds(0.25f);
        }

        energia = Mathf.Clamp(energia, 0, energiaMax);
        n = 0;
        foreach (var i in IK) i.trocaEstado(S_IK.estadoMao.livre);
        foreach (var i in maos) i.GetComponent<XRBaseInteractor>().allowSelect = true;
        rodandoSS = false;
    }

    public void DesativaEnergia(bool ativada)
    {
        foreach (var i in IK) if (i.conectado) i.Desconecta();
        foreach (var i in maos) i.GetComponent<XRBaseInteractor>().allowSelect = ativada;
    }
}
