using System.Collections;
using FMODUnity;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class S_energia : MonoBehaviour //controla apenas stamina e solta o S_dis_BoneGrab
{
    S_jogador Jogador;

    [Header("STAMINA")]
    protected int n = 0;
    public float energiaMax = 100;
    public float energia;
    public bool rodandoSS;

    [Header("CONEXÃO")]
    public S_IK[] IK;
    public XRBaseInteractor[] maos;

    public ParticleSystem atordoado;

    [Header("SONS")]
    public EventReference caindoEnergia;
    public EventReference subindoEnergia;

    private void Start()
    {
        energia = energiaMax;
        Jogador = GetComponent<S_jogador>();
        IK = GetComponentsInChildren<S_IK>().Take(2).ToArray();
        maos = GetComponentsInChildren<XRBaseInteractor>().Take(4).ToArray();
    }

    private void Update()
    {
        if (S_verificaGolpe.timeSlow) return;

        if (rodandoSS) return;

        if (energia > 0)
        {
            if (!S_controleTutorial.emTutorial)
            {
                float q = 0.35f;
                foreach (var i in IK) if (i.aberto) { q += 0.5f; }
                if (Jogador.posPerna.Contains("A")) q += 1;
                energia -= Time.deltaTime * q;
            }
        }
        else StartCoroutine(SemStamina());

        if ((energia > energiaMax || energia < 0) && energia < 99999) energia = Mathf.Clamp(energia, 0, energiaMax);
    }

    IEnumerator SemStamina()
    {
        if (S_verificaGolpe.timeSlow) yield break;

        S_onClique.PlayOneShot(caindoEnergia);

        atordoado.Play(false);

        rodandoSS = true;
        foreach (var i in IK)
        {
            if (i.conectado) i.Desconecta();
            i.trocaEstado(S_IK.estadoMao.desativada);
        }
        foreach (var i in maos) i.GetComponent<XRBaseInteractor>().allowSelect = false; 
        
        n = 5;

        yield return new WaitForSeconds(3.25f);

        S_onClique.PlayOneShot(subindoEnergia);
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
