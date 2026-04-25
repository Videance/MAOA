using System.Collections.Generic;
using UnityEngine;

public class S_jogador : MonoBehaviour
{
    public S_jogador adversario;

    public string imaoEsq;
    public string imaoDir;

    public string dirEqui = "c";

    public bool pernaAberta;

    [Header("conectores")]
    public List<GameObject> conectores;

    [Header("ragdoll")]
    public bool emRagdoll = false;
    public Animator animator;
    public GameObject RIG;
    public List<Rigidbody> ragdollBodies = new List<Rigidbody>();

    private void Awake()
    {
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
        conectores = new List<GameObject>();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("c")) conectores.Add(t.gameObject);
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
            Gravidade(forma);
        }
    }

    public void Gravidade(bool ativada) { foreach (Rigidbody rb in ragdollBodies) rb.useGravity = ativada; }
}
