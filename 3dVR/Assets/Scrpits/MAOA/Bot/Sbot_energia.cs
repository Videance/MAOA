
using FMODUnity;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Sbot_energia : S_energia
{
    Sbot_jogador jogador;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jogador = GetComponent<Sbot_jogador>();
        energiaMax = 30f + 60f * (1f + 9f * Mathf.Pow(Sbot_jogador.dificuldade / 100f, 2f));
        energia = energiaMax;
        IK = GetComponentsInChildren<S_IK>().Take(2).ToArray();
        maos = GetComponentsInChildren<XRBaseInteractor>().Take(4).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (S_verificaGolpe.timeSlow) return;

        if (rodandoSS) return;

        if (energia > 0)
        {
            energia -= Time.deltaTime / 3;

            if (!S_controleTutorial.tutorial1)
            {
                float q = 0.35f;
                foreach (var i in IK) if (i.aberto) { q += 0.5f; }
                if (jogador.posPerna.Contains("A")) q += 1;
                energia -= Time.deltaTime * q;
            }
        }
        else StartCoroutine(SemStamina());

        if (energia > energiaMax || energia < 0) energia = Mathf.Clamp(energia, 0, energiaMax);
    }

    IEnumerator SemStamina()
    {
        if (S_verificaGolpe.timeSlow) yield break;

        S_onClique.PlayOneShot(caindoEnergia);
        atordoado.Play(false);

        rodandoSS = true;
        foreach (var i in IK) if (i.conectado) i.Desconecta();

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
        rodandoSS = false;
    }
}
