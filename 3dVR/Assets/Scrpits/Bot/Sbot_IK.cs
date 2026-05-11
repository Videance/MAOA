using UnityEngine;
using System.Collections;

public class Sbot_IK : S_IK
{
    public string alvoC;
    public float speed = 0.15f;
    Sbot_energia Senergia;
    public bool movendo = false;
    public S_dis_boneGrab DisGrab;
    bool saindo = false;

    protected override void Awake()
    {
        base.Awake();

        jogador = GetComponentInParent<Sbot_jogador>();
        Senergia = GetComponentInParent<Sbot_energia>();
        DisGrab = GetComponent<S_dis_boneGrab>();

        speed = Mathf.Sqrt(((Sbot_jogador)jogador).dificuldade * speed) + speed;
    }

    public IEnumerator Move(Transform conector)
    {
        movendo = true;
        yield return new WaitUntil(() => saindo == false);

        if (conectado != null)
        {
            Desconecta();
            yield return new WaitUntil(() => saindo == false);
        }

        float te = 0f;
        while (conectado == null && te < 2.5f)
        {
            if (Senergia.energia <= 0)
            {
                movendo = false;
                yield break;
            }
            te += Time.unscaledDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, conector.position, speed * Time.deltaTime);
            yield return null;
        }

        if (conectado == null) ((Sbot_jogador)jogador).ProcuraGolpe(((Sbot_jogador)jogador).dificuldade);
        movendo = false;
    }

    public override void Conecta()
    {
        S_Conector Scone = conectado.GetComponent<S_Conector>();

        Scone.maoOcupando = this;

        if (ladoEsq)
        {
            jogador.imaoEsq = conectado.GetComponent<S_Conector>().localDoCorpo;
            ((Sbot_jogador)jogador).VerificaVar(3);
        }
        else
        {
            jogador.imaoDir = conectado.GetComponent<S_Conector>().localDoCorpo;
            ((Sbot_jogador)jogador).VerificaVar(2);
        }

        Scone.rend.material = materials[1];
        Scone.rend.material.SetColor("_Cor", corAtivada);

        trocaEstado(estadoMao.conectada);
    }

    public override void Desconecta()
    {
        S_Conector Scone = conectado.GetComponent<S_Conector>();

        Scone.rend.material = materials[1];
        Scone.rend.material.SetColor("_Cor", corAtivada);

        if (conectado != null)
        {
            Scone.maoOcupando = null;
            conectado = null;
        }

        if (ladoEsq)
        {
            jogador.imaoEsq = null;
            ((Sbot_jogador)jogador).VerificaVar(3);
        }
        else
        {
            jogador.imaoDir = null;
            ((Sbot_jogador)jogador).VerificaVar(2);
        }

        trocaEstado(estadoMao.livre);

        StartCoroutine(VoltarProPeito());
    }

    public IEnumerator VoltarProPeito()
    {
        saindo = true;
        Vector3 start = transform.position;
        Vector3 target = Vector3.Lerp(start, peito.position, 0.35f);

        float t = 0f;
        float duracao = 0.5f;

        while (t < 1f)
        {
            t += Time.deltaTime / duracao;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        saindo = false;
    }

    //---------- CONTROLE DE COLISÕES ----------
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("c") && !conectado && other.GetComponent<S_Conector>().localDoCorpo == alvoC && Senergia.energia > 0)
        {
            conectado = other.gameObject;
            Conecta();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("c") && conectado && other.GetComponent<S_Conector>().localDoCorpo == alvoC)
            Desconecta();
    }
}
