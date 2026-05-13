using System.Collections.Generic;
using UnityEngine;

public class S_jogador : MonoBehaviour
{
    public S_jogador adversario;

    [Header("POSICAO")]
    public string imaoEsq = null;
    public string imaoDir = null;
    public string dirEqui = "c";
    public bool pernaAberta = false;
    public bool seMovendo = false;

    [Header("PARTES DO CORPO")]
    public List<S_Conector> conectores;
    public S_IK[] IKs;
    public S_dis_pe[] PEs;
    public S_Equilibrio Sequilibrio;

    [Header("RAGDOLL")]
    public bool emRagdoll = false;
    public Animator animator;
    public GameObject RIG;
    public List<Rigidbody> ragdollBodies = new List<Rigidbody>();

    protected virtual void Awake()
    {
        PEs = GetComponentsInChildren<S_dis_pe>();

        IKs = GetComponentsInChildren<S_IK>();

        Sequilibrio = GetComponentInChildren<S_Equilibrio>();

        Collider[] colliders = GetComponentsInChildren<Collider>();
        if (adversario != null)
        {
            Collider[] advC = adversario.GetComponentsInChildren<Collider>();

            // 1. Regras com adversário
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("IK")) // só mãos
                {
                    for (int j = 0; j < advC.Length; j++)
                    {
                        if (!advC[j].CompareTag("c")) // ignora tudo menos "c"
                        {
                            Physics.IgnoreCollision(colliders[i], advC[j]);
                        }
                    }
                }
            }
        }
        // 2. Ignorar colisão interna
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j]);
            }
        }

        animator = GetComponentInChildren<Animator>();
        conectores = new List<S_Conector>();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("c")) conectores.Add(t.GetComponent<S_Conector>());
            if (t.CompareTag("p") && t.gameObject.GetComponent<Rigidbody>() == true) ragdollBodies.Add(t.gameObject.GetComponent<Rigidbody>());
        }
        Ragdoll(false);
    }

    private void Update()
    {
        seMovendo = (IKs[0].estado == S_IK.estadoMao.segurando || IKs[1].estado == S_IK.estadoMao.segurando ||
            PEs[0].segurando || IKs[1].segurando || Sequilibrio.equilibrioCandidato != null) ? true : false;
    }

    public void Ragdoll(bool forma) //true vira ragdoll
    {
        emRagdoll = forma;
        animator.enabled = !forma;
        RIG.gameObject.SetActive(!forma);

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !forma;
            Gravidade(false);
        }
    }

    public void Gravidade(bool ativada)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            if (rb.name == "B Peitoral") rb.useGravity = ativada;
            else rb.useGravity = false;
        }
    }
}
