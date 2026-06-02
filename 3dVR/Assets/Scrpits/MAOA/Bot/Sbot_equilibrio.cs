using System.Collections;
using UnityEngine;

public class Sbot_equilibrio : S_Equilibrio
{
    public bool movendo = false;
    public string fugaQualPlaca = null;
    public float speed = 2f;
    public float speedMax = 2f;

    protected override void Start()
    {
        if (Sbot_jogador.dificuldade != 1)
            speedMax += Mathf.Sqrt(Sbot_jogador.dificuldade);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        AtualizaEstadoEnergia();
        AtualizarEnergiaVisual();

        if (pCentral == null || energia.rodandoSS)
            return;

        float porcentagemEnergia = energia.energia / energia.energiaMax;
        tempoTroca = Mathf.Lerp(1f, 0.25f, porcentagemEnergia);

        string novoEquilibrio = null;
        float distanciaCentro = Vector3.Distance(pCentral.transform.position, inicialPos);

        if (distanciaCentro <= (dist * 0.52f))
        {
            novoEquilibrio = "c";
        }
        else if (distanciaCentro >= (dist * 0.6f))
        {
            Vector3 dir = (pCentral.transform.position - inicialPos).normalized;

            if (dir.x > XYdir) novoEquilibrio = "e";
            else if (dir.x < -XYdir) novoEquilibrio = "d";
            else if (dir.z > XYdir) novoEquilibrio = "t";
            else if (dir.z < -XYdir) novoEquilibrio = "f";
        }

        // se não encontrou equilíbrio válido ou é igual
        if (novoEquilibrio == null || novoEquilibrio == direcaoEquilibrio)
        {
            equilibrioCandidato = null;
            contadorTroca = 0f;
            return;
        }

        // começou novo candidato
        if (equilibrioCandidato != novoEquilibrio)
        {
            equilibrioCandidato = novoEquilibrio;
            contadorTroca = tempoTroca;
        }
        else
        {
            contadorTroca -= Time.deltaTime;

            if (contadorTroca <= 0f)
            {
                int index = 0;

                if (novoEquilibrio == "c") index = 0;
                if (novoEquilibrio == "t") index = 1;
                if (novoEquilibrio == "d") index = 2;
                if (novoEquilibrio == "f") index = 3;
                if (novoEquilibrio == "e") index = 4;

                TrocaEquilibrio(novoEquilibrio, index);

                equilibrioCandidato = null;
            }
        }
    }

    public override void TrocaEquilibrio(string letra, int index)
    {
        if (jogador.dirEqui == letra) return;
        direcaoEquilibrio = letra;
        jogador.dirEqui = letra;

        if (dirFulga != null)
        {
            if (letra != dirFulga) return;
            else dirFulga = null;
        }

        if (primeira) primeira = false;
        else if (!S_verificaGolpe.timeSlow) energia.energia -= 5;
        energia.energia = Mathf.Clamp(energia.energia, 0, energia.energiaMax);

        ((Sbot_jogador)jogador).VerificaVar(0);

        TrocarCor(letra, false);
    }

    public IEnumerator mover(Vector3 final)
    {
        movendo = true;

        final.y = pCentral.transform.position.y;

        while (Vector3.Distance(pCentral.transform.position, final) > 0.0005f)
        {
            if (energia.rodandoSS)
            {
                movendo = false;
                yield break;
            }
            pCentral.transform.position = Vector3.Lerp(pCentral.transform.position, final, Time.unscaledDeltaTime * speed);
            yield return null;
        }
        pCentral.transform.position = final;

        movendo = false;
    }
}
