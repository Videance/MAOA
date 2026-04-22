using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class S_verificaGolpe : MonoBehaviour
{
    [SerializeField] public List<C_golpes> golpes = new List<C_golpes>();

    public static bool timeSlow = false;

    public static S_verificaGolpe Vgolpe;
    private void Awake() { Vgolpe = this; }

    public static void AcharGolpe(S_jogador jog, S_jogador adv)
    {
        if (timeSlow) return;

        foreach (var golpe in Vgolpe.golpes)
        {
            int pontos = 0;

            if (golpe.conectorImaoDir == jog.imaoDir) pontos++;
            if (golpe.conectorImaoEsq == jog.imaoEsq) pontos++;
            if (golpe.JdirEqui == jog.dirEqui) pontos++;
            if (golpe.IdirEqui == adv.dirEqui) pontos++;
            if (golpe.pernaAberta == jog.pernaAberta) pontos++;

            Debug.Log(pontos);

            if (pontos == 5)
            {
                Vgolpe.StartCoroutine(Vgolpe.TimeSlow(golpe, jog, adv));
                break;
            }
        }
    }

    public IEnumerator TimeSlow(C_golpes move, S_jogador jog, S_jogador adv)
    {
        timeSlow = true;

        
        adv.Ragdoll(true);

        yield return new WaitForSecondsRealtime(10f); //espera e 'reseta' partida

        timeSlow = false;
    }
}
