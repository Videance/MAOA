using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class S_verificaGolpe : MonoBehaviour
{
    [SerializeField] public List<C_golpes> golpes = new List<C_golpes>();
    public S_jogador jogador;
    public S_jogador adversario;

    public bool timeSlow = false;

    public void AcharGolpe()
    {
        foreach (var golpe in golpes)
        {
            int pontos = 0;

            if (golpe.conectorImaoDir == jogador.imaoDir) pontos++;
            if (golpe.conectorImaoEsq == jogador.imaoEsq) pontos++;
            if (golpe.JdirEqui == jogador.dirEqui) pontos++;
            if (golpe.IdirEqui == adversario.dirEqui) pontos++;
            if (golpe.pernaAberta == jogador.pernaAberta) pontos++;

            Debug.Log(pontos);

            if (pontos == 5)
            {
                StartCoroutine(TimeSlow(golpe));
                break;
            }
        }
    }

    public IEnumerator TimeSlow(C_golpes move)
    {
        timeSlow = true;

        adversario.Ragdoll(true);

        yield return new WaitForSecondsRealtime(10f); //espera e 'reseta' partida

        timeSlow = false;
    }
}
