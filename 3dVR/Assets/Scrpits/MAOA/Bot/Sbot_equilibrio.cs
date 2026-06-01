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
        base.Start();

        if (Sbot_jogador.dificuldade != 1)
            speedMax += Mathf.Sqrt(Sbot_jogador.dificuldade);
    }

    // Update is called once per frame
    protected override void Update()
    {
        AtualizaEstadoEnergia();

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
        string equilibrioAnterior = direcaoEquilibrio;

        base.TrocaEquilibrio(letra, index);

        if (equilibrioAnterior != direcaoEquilibrio)
            ((Sbot_jogador)jogador).VerificaVar(0);
    }

    public IEnumerator mover(Vector3 final)
    {
        movendo = true;

        // POSSIVELMENTE SERA RETIRADO
        if (final == Vector3.zero)
        {
            float pcX = inicialPos.x;
            float pcZ = inicialPos.z;

            float distancia = dist;

            Vector2 rand = Random.insideUnitCircle * distancia;

            float x = pcX + rand.x;
            float z = pcZ + rand.y;

            final = new Vector3(x, 0, z);
        }
        //

        final.y = pCentral.transform.position.y;

        while (Vector3.Distance(pCentral.transform.position, final) > 0.0005f)
        {
            if (energia.energia <= 0)
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

    public override void TrocarCor(string letra, bool emFuga)
    {
        base.TrocarCor(letra, emFuga);
        fugaQualPlaca = letra;
    }
}
