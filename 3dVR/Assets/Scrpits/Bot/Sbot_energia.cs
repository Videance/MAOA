using System;
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
        energiaMax = 110 + (Mathf.Pow(jogador.dificuldade, 1.5f) * 9);
        energia = energiaMax;
        Renderer = GetComponentsInChildren<SpriteRenderer>().Take(1).ToArray();
        IK = GetComponentsInChildren<S_IK>().Take(2).ToArray();
        texto = GetComponentsInChildren<TextMesh>();
        maos = GetComponentsInChildren<XRBaseInteractor>().Take(4).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (S_verificaGolpe.timeSlow) return;

        // troca imagem da bateria
        if (energia > (energiaMax * 0.80f)) TrocaSprite(0);
        else if (energia > (energiaMax * 0.60f)) TrocaSprite(n + 1);
        else if (energia > (energiaMax * 0.40f)) TrocaSprite(n + 2);
        else if (energia > (energiaMax * 0.20f)) TrocaSprite(n + 3);
        else if (energia > 0) TrocaSprite(n + 4);
        else if (energia == 0) TrocaSprite(5);

        if (rodandoSS) return;

        if (energia > 0)
        {
            energia -= Time.deltaTime / 3;

            int q = 0;
            foreach (var i in IK) if (i.conectado) { q += 2; }
            if (jogador.pernaAberta) q += 1;
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

    IEnumerator SemStamina()
    {
        if (S_verificaGolpe.timeSlow) yield break;

        rodandoSS = true;
        foreach (var i in IK) if (i.conectado) i.Desconecta();

        n = 5;

        yield return new WaitForSeconds(2f);

        while (energia < energiaMax)
        {
            energia += (energiaMax * 0.20f);
            if (energia < energiaMax) yield return new WaitForSeconds(0.25f);
        }

        energia = Mathf.Clamp(energia, 0, energiaMax);
        n = 0;
        rodandoSS = false;
    }
}
