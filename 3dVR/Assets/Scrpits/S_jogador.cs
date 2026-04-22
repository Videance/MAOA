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
    List<GameObject> conectores;

    [Header("ragdoll")]
    private Animator animator;
    public GameObject RIG;
    public List<Rigidbody> ragdollBodies = new List<Rigidbody>();

    private void Awake()
    {
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
        animator.enabled = !forma;
        RIG.gameObject.SetActive(!forma);

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !forma;
            rb.useGravity = forma;
        }
    }
}
