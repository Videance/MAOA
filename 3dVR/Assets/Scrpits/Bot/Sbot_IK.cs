using UnityEngine;
using System.Collections;

public class Sbot_IK : S_IK
{
    public string alvoC;
    public float speed = 0.05f;

    protected override void Awake()
    {
        base.Awake();

        jogador = GetComponentInParent<Sbot_jogador>();

        speed = Mathf.Sqrt(((Sbot_jogador)jogador).dificuldade * speed);
    }

    public IEnumerator Move(Transform conector)
    {
        if (conectado != null)
        {
            Desconecta();
            float t = 2 / ((((Sbot_jogador)jogador).dificuldade + 1) / 2);
            yield return new WaitForSeconds(t);
        }

        float te = 0f;
        while (conectado == null && te < 3f)
        {
            Debug.Log("eu");
            te += Time.unscaledDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, conector.position, speed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("fim");
    }

    public override void Conecta()
    {
        if (ladoEsq) jogador.imaoEsq = conectado.GetComponent<S_Conector>().localDoCorpo;
        else jogador.imaoDir = conectado.GetComponent<S_Conector>().localDoCorpo;  

        conectado.GetComponent<S_Conector>().maoOcupando = this;
    }

    public override void Desconecta()
    {
        if (ladoEsq) jogador.imaoEsq = null;
        else jogador.imaoDir = null;

        conectado.GetComponent<S_Conector>().maoOcupando = null;
        conectado = null;

        Vector3 dir = (transform.position - peito.position).normalized;
        rb.linearVelocity = dir * 200;
    }

    //---------- CONTROLE DE COLISÕES ----------
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("c") && !conectado && other.GetComponent<S_Conector>().localDoCorpo == alvoC)
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
