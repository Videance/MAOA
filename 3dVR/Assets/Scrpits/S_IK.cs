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
    private Rigidbody rb;
    private XRGrabInteractable grab;
    private List<S_Conector> cNoAlcance;

    [Header("CONTROLE DA STAMINA")]
    private S_energia energia;

    private void Awake()
    {
        jogador = GetComponentInParent<S_jogador>();
        energia = GetComponent<S_energia>();
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        cNoAlcance = new List<S_Conector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (botao.action.WasPressedThisFrame()) Debug.Log("cliclou");
        // conexão por botão
        if (conectado == null && botao.action.WasPressedThisFrame() && cNoAlcance.Count > 0)
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
        else if (conectado != null && botao.action.WasPressedThisFrame()) Desconecta();
    }

    public void Conecta()
    {
        if (ladoEsq) jogador.imaoEsq = conectado.GetComponent<S_Conector>().localDoCorpo;
        else jogador.imaoDir = conectado.GetComponent<S_Conector>().localDoCorpo;
        S_verificaGolpe.AcharGolpe(jogador, jogador.adversario);
        grab.trackPosition = false;
        grab.trackRotation = false;
    }

    public void Desconecta()
    {
        if (ladoEsq) jogador.imaoEsq = null;
        else jogador.imaoDir = null;
        grab.trackPosition = true;
        grab.trackRotation = true;
        conectado = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (conectado != null) return;
        if (!other.gameObject.CompareTag("c") || jogador.conectores.Contains(other.gameObject)) return;

        Debug.Log("entrou");
        cNoAlcance.Add(other.GetComponent<S_Conector>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (conectado != null) return;
        if (!other.gameObject.CompareTag("c") || !cNoAlcance.Contains(other.gameObject.GetComponent<S_Conector>())) return;

        Debug.Log("saiu");
        cNoAlcance.Remove(other.GetComponent<S_Conector>());
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
    {
        segurando = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        segurando = false;
    }
}
