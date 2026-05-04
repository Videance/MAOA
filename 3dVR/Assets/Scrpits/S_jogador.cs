using System.Collections.Generic;
using UnityEngine;

public class S_jogador : MonoBehaviour
{
    public S_jogador adversario;

    [Header("Pos atual")]
    public string imaoEsq = null;
    public string imaoDir = null;
    public string dirEqui = "c";
    public bool pernaAberta = false;

    [Header("Partes do corpo")]
    public List<S_Conector> conectores;
    public S_dis_boneGrab[] pernas;
    public S_IK[] IKs;
    public S_dis_pe[] PEs;

    [Header("ragdoll")]
    public bool emRagdoll = false;
    public Animator animator;
    public GameObject RIG;
    public List<Rigidbody> ragdollBodies = new List<Rigidbody>();

    [Header("pontuacao")]
    public int jogPontos = 0;

    protected virtual void Awake()
    {
        pernas = GetComponentsInChildren<S_dis_pe>();

        IKs = GetComponentsInChildren<S_IK>();

        Collider[] colliders = GetComponentsInChildren<Collider>();
        Collider[] advC = adversario.GetComponentsInChildren<Collider>();
        // 1. Ignorar colisão interna
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j]);
            }
        }
        // 2. Regras com adversário
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

        animator = GetComponentInChildren<Animator>();
        conectores = new List<S_Conector>();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("c")) conectores.Add(t.GetComponent<S_Conector>());
            if (t.CompareTag("p") && t.gameObject.GetComponent<Rigidbody>() == true) ragdollBodies.Add(t.gameObject.GetComponent<Rigidbody>());
        }
        Ragdoll(false);
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
