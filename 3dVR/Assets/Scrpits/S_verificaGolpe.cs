using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class S_verificaGolpe : MonoBehaviour
{
    public GameObject pDesequil;

    [Header("Lista de golpes")]
    [SerializeField] public List<C_golpes> golpes = new List<C_golpes>();
    private static C_golpes ataque;

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
                ataque = golpe;
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

        //cria o ponto
        Vector3 meio = (jog.IKs[0].transform.position + jog.IKs[1].transform.position) / 2f;

        GameObject pde = Instantiate(pDesequil, meio, pDesequil.transform.rotation);
        adv.GetComponentInChildren<S_segueC>().pDes = pde.GetComponent<S_rb>().rb;
        adv.GetComponentInChildren<S_segueC>().SpontoDes = pde.GetComponent<S_pontoDes>();
        S_pontoDes Spde = pde.GetComponent<S_pontoDes>();

        GameObject caminho = Instantiate(ataque.dirPdes, meio, ataque.dirPdes.transform.rotation);

        //pega energia
        S_energia jogEner = jog.GetComponent<S_energia>();
        S_energia advEner = adv.GetComponent<S_energia>();

        //adv - Ragdoll e desativa mão
        adv.Ragdoll(true);
        advEner.DesativaEnergia();

        //jog - Troca layer dos IK
        for (int i = 0; i < jog.iks.Length; i++) { jog.iks[i].gameObject.layer = LayerMask.NameToLayer("xG"); }

        //tempo lento:
        Time.timeScale = 0.2f;

        //controla o tempo máximo
        float tempo = 0f;
        S_IK ik = jog.GetComponentInChildren<S_IK>();
        while (tempo < 4f && !Spde.jogado && Spde.noCaminho)
        {
            tempo += Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(pde);
        Destroy(caminho);
        adv.Gravidade(true);

        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(10f);

        timeSlow = false;
    }
}
