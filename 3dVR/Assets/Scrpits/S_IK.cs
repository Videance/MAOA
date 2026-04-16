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
    public S_jogador jogador;

    [Header("PEGADA")]
    public bool segurando;
    public GameObject conectado; // S_dis_boneGrab = mão que tem que pegar
    public InputActionReference botao;
    private Rigidbody rb;
    public XRGrabInteractable grab;
    private List<S_Conector> cNoAlcance;
    public S_verificaGolpe Vgolpes;

    [Header("CONTROLE DA STAMINA")]
    public S_imaos conector;

    private void Awake()
    {
        conector = GetComponent<S_imaos>();
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        cNoAlcance = new List<S_Conector>();
    }

    // Update is called once per frame
    void Update()
    {
        // conexão por botão
        if (conectado == null && botao.action.WasPressedThisFrame() && cNoAlcance.Count > 0)
        {
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
        Vgolpes.AcharGolpe();
        grab.trackPosition = false;
        grab.trackRotation = false;
        conector.tempo = 0;
    }

    public void Desconecta()
    {
        grab.trackPosition = true;
        grab.trackRotation = true;
        conector.tempo = 0;
        conectado = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(cJ)) return;

        Debug.Log("entrou");
        cNoAlcance.Add(other.GetComponent<S_Conector>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(cJ)) return;

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
