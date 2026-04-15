using System.Collections.Generic;
using UnityEngine;

public class S_jogador : MonoBehaviour
{
    public string imaoEsq;
    public string imaoDir;

    public string dirEqui;

    public bool pernaAberta;

    [Header("ragdoll")]
    private Animator animator;
    public GameObject RIG;
    public List<Rigidbody> ragdollBodies = new List<Rigidbody>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Ragdoll(false);
    }

    public void Ragdoll(bool forma) //true vira ragdoll
    {
        animator.enabled = !forma;
        RIG.gameObject.SetActive(!forma);

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !forma;
            rb.useGravity = forma;
        }
    }
}
