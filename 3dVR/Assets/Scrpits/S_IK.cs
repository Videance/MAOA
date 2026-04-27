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
    private S_jogador jogador;

    [Header("PEGADA")]
    public bool segurando;
    public GameObject conectado; // S_dis_boneGrab = mão que tem que pegar
    public InputActionReference botao;
    public Rigidbody rb;
    private XRGrabInteractable grab;
    private List<S_Conector> cNoAlcance;
    public SphereCollider coll;

    public enum estadoMao
    {
        livre,
        segurando,
        conectada,
        desativada
    }
    public estadoMao estado;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        jogador = GetComponentInParent<S_jogador>();
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        cNoAlcance = new List<S_Conector>();
        estado = estadoMao.livre;
    }

    // Update is called once per frame
    void Update()
    {
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

    public void trocaEstado(estadoMao es)
    {
        estado = es;
        if (es == estadoMao.desativada || es == estadoMao.conectada)
        {
            grab.trackPosition = false;
            grab.trackRotation = false;
            grab.enabled = false;
        }
        if (es == estadoMao.livre)
        {
            grab.trackPosition = true;
            grab.trackRotation = true;
            grab.enabled = true;
        }

        if (es == estadoMao.segurando) segurando = true;
        else segurando = false;
    }

    public void Conecta()
    {
        if (grab.isSelected)
        {
            var interactor = grab.firstInteractorSelecting;
            grab.interactionManager.SelectExit(interactor, grab);
        }

        if (ladoEsq) jogador.imaoEsq = conectado.GetComponent<S_Conector>().localDoCorpo;
        else jogador.imaoDir = conectado.GetComponent<S_Conector>().localDoCorpo;

        S_verificaGolpe.AcharGolpe(jogador, jogador.adversario);

        conectado.GetComponent<S_Conector>().maoOcupando = this;

        trocaEstado(estadoMao.conectada);
    }

    public void Desconecta()
    {
        if (ladoEsq) jogador.imaoEsq = null;
        else jogador.imaoDir = null;

        conectado.GetComponent<S_Conector>().maoOcupando = null;
        conectado = null;

        trocaEstado(estadoMao.livre);
    }

    //---------- CONTROLE DE COLISÕES ----------
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("c") || jogador.conectores.Contains(other.gameObject)) return;

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
