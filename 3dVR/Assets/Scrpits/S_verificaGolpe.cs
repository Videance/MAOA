using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class S_verificaGolpe : MonoBehaviour
{
    [SerializeField] public List<C_golpes> golpes = new List<C_golpes>();

    public static bool timeSlow = false;

    public static S_verificaGolpe Vgolpe;
    private void Awake() { Vgolpe = this; }
    private float tempo = 0;

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

            jog.GetComponent<S_energia>().energia -= golpe.custoEnergia;
        }
    }

    public IEnumerator TimeSlow(C_golpes move, S_jogador jog, S_jogador adv)
    {
        if (timeSlow) yield break;
        timeSlow = true;

        adv.Ragdoll(true);
        adv.GetComponent<S_energia>().DesativaStamina();

        tempo = 0f;
        S_IK ik = jog.GetComponentInChildren<S_IK>();

        while (tempo < 5f && ik.conectado != null)
        {
            tempo += Time.unscaledDeltaTime;
            yield return null;
        }

        adv.Gravidade(true);

        yield return new WaitForSecondsRealtime(10f);

        timeSlow = false;
    }
}
