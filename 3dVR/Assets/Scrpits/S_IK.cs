using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_IK : MonoBehaviour
{
    [Header("JOGADOR 1 OU 2")]
    public string cJ;
    public bool ladoEsq;
    protected S_jogador jogador;

    [Header("PEGADA")]
    public bool segurando;
    public GameObject conectado; // S_dis_boneGrab = mão que tem que pegar
    public InputActionReference botao;
    public Rigidbody rb;
    public XRGrabInteractable grab;
    protected List<S_Conector> cNoAlcance;
    public Renderer rend;
    public SphereCollider coll;
    public Transform peito;

    [Header("MATERIAIS E CORES")]
    protected Color corBase;
    protected Color corAtivada;
    protected Color corDesligado;
    public Material[] materials; // 0 Base | 1 Conectada

    public enum estadoMao
    {
        livre,
        segurando,
        conectada,
        desativada
    }
    public estadoMao estado;

    protected virtual void Awake()
    {
        coll = GetComponent<SphereCollider>();
        jogador = GetComponentInParent<S_jogador>();
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        cNoAlcance = new List<S_Conector>();

        ColorUtility.TryParseHtmlString("#BA1AB8", out corBase);
        ColorUtility.TryParseHtmlString("#00B3FF", out corAtivada);
        ColorUtility.TryParseHtmlString("#111011", out corDesligado);

        trocaEstado(estadoMao.livre);
    }

    // Update is called once per frame
    void Update()
    {
        if (S_verificaGolpe.timeSlow) trocaEstado(estadoMao.desativada);

        if (botao.action.WasPressedThisFrame() && estado != estadoMao.desativada)
        {
            Debug.Log("cliclou");
            
            if (estado == estadoMao.segurando && cNoAlcance.Count > 0)
            {
                Debug.Log("conectou");

                if (cNoAlcance.Count == 1) conectado = cNoAlcance[0].gameObject;
                else if (cNoAlcance.Count >= 2)
                {
                    int index = 0;
                    float disA = 9999;
                    for (int i = 0; i < cNoAlcance.Count; i++)
                    {
                        float dis = Vector3.Distance(transform.position, cNoAlcance[i].transform.position);
                        if (dis < disA)
                        {
                            disA = dis;
                            index = i;
                        }
                    }
                    conectado = cNoAlcance[index].gameObject;
                }

                Conecta();
            }
            else if (estado == estadoMao.conectada) Desconecta();
        }
    }

    public virtual void trocaEstado(estadoMao es)
    {
        estado = es;

        if (es == estadoMao.conectada)
        {
            rend.material = materials[1];
            rend.material.SetColor("_Cor", corAtivada);

            grab.trackPosition = false;
            grab.trackRotation = false;
            grab.enabled = false;
        }
        if (es == estadoMao.desativada)
        {
            rend.material = materials[0];
            rend.material.SetColor("_Cor", corDesligado);
            rend.material.SetFloat("_transparencia", 1f);

            grab.trackPosition = false;
            grab.trackRotation = false;
            grab.enabled = false;
        }
        if (es == estadoMao.livre)
        {
            rend.material = materials[0];
            rend.material.SetColor("_Cor", corBase);
            rend.material.SetFloat("_transparencia", 0.6f);

            grab.trackPosition = true;
            grab.trackRotation = true;
            grab.enabled = true;
        }
        if (es == estadoMao.segurando)
        {
            rend.material = materials[0];
            rend.material.SetColor("_Cor", corBase);
            rend.material.SetFloat("_transparencia", 0.6f);

            segurando = true;
        }
        else segurando = false;
    }

    public virtual void Conecta()
    {
        if (estado == estadoMao.desativada) return;

        if (grab.isSelected)
        {
            var interactor = grab.firstInteractorSelecting;
            grab.interactionManager.SelectExit(interactor, grab);
        }

        if (ladoEsq) jogador.imaoEsq = conectado.GetComponent<S_Conector>().localDoCorpo;
        else jogador.imaoDir = conectado.GetComponent<S_Conector>().localDoCorpo;

        S_verificaGolpe.Vgolpe.AcharGolpe(jogador, jogador.adversario);

        S_Conector Scone = conectado.GetComponent<S_Conector>();

        Scone.maoOcupando = this;
        Scone.rend.material = materials[1];
        Scone.rend.material.SetColor("_Cor", corAtivada);

        trocaEstado(estadoMao.conectada);
    }

    public virtual void Desconecta()
    {
        if (conectado == null) return; 

        if (ladoEsq) jogador.imaoEsq = null;
        else jogador.imaoDir = null;

        S_Conector Scone = conectado.GetComponent<S_Conector>();

        Scone.rend.material = materials[0];
        Scone.rend.material.SetColor("_Cor", corBase);
        Scone.maoOcupando = null;

        conectado = null;

        trocaEstado(estadoMao.livre);

        Vector3 dir = (transform.position - peito.position).normalized;
        rb.linearVelocity = dir * 2;
    }

    //---------- CONTROLE DE COLISÕES ----------
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("c") || jogador.conectores.Contains(other.gameObject.GetComponent<S_Conector>())) return;

        Debug.Log("entrou");
        if (!cNoAlcance.Contains(other.GetComponent<S_Conector>())) cNoAlcance.Add(other.GetComponent<S_Conector>());
    }

    private void OnTriggerExit(Collider other)
    {
        S_Conector sOther = other.gameObject.GetComponent<S_Conector>();

        if (!other.gameObject.CompareTag("c") || !cNoAlcance.Contains(sOther)) return;

        Debug.Log("saiu");
        if (cNoAlcance.Contains(sOther))
        {
            if (conectado != null && sOther == conectado.GetComponent<S_Conector>()) return;
            else cNoAlcance.Remove(sOther);
        }
    }


    // ---------- CONTROLE DE VARIÁVEL QUANDO SEGURANDO OU NÃO ----------
    private void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args) 
    { estado = estadoMao.segurando; }

    private void OnRelease(SelectExitEventArgs args) 
    { if (estado == estadoMao.segurando) estado = estadoMao.livre; }
}
