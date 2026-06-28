using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_jogador : MonoBehaviour
{
    public S_jogador adversario;
    public bool jog1;
    protected float tSeMove = 0;

    [Header("POSICAO")]
    public string imaoEsq = null;
    public string imaoDir = null;
    public string dirEqui = "c";
    public string posPerna = null;
    public bool seMovendo = false;
    public bool vulneravel = false;

    [Header("PARTES DO CORPO")]
    public List<S_Conector> conectores;
    public S_IK[] IKs;
    public S_dis_pe[] PEs;
    public S_Equilibrio Sequilibrio;
    public S_energia Senergia;
    public List<Renderer> CorpoTodoRend = new List<Renderer>();
    public GameObject escudo;

    [Header("RAGDOLL")]
    public bool emRagdoll = false;
    public Animator animator;
    public GameObject RIG;
    public List<Rigidbody> ragdollBodies = new List<Rigidbody>();

    [Header("PARTICULAS")]
    public ParticleSystem[] falhou;

    protected virtual void Awake()
    {
        PEs = GetComponentsInChildren<S_dis_pe>();

        IKs = GetComponentsInChildren<S_IK>();

        Sequilibrio = GetComponent<S_Equilibrio>();
        Senergia = GetComponent<S_energia>();

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

        if (transform.position.z < 0) jog1 = true;

        Ragdoll(false);
    }

    protected IEnumerator esperaSeMove()
    {
        if (seMovendo) yield break;
        seMovendo = true;

        while (tSeMove < 0.75f)
        {
            tSeMove += Time.unscaledDeltaTime;
            if (tSeMove >= 0.75f)
            {
                seMovendo = false;
            }
            yield return null;
        }
    }

    private void Update()
    {
        if (!S_verificaGolpe.timeSlow && !S_verificaGolpe.derrotou)
        {
            if (IKs[0].estado == S_IK.estadoMao.segurando || IKs[1].estado == S_IK.estadoMao.segurando ||
                PEs[0].segurando || PEs[1].segurando || Sequilibrio.equilibrioCandidato != null || vulneravel || Senergia.rodandoSS)
            {
                tSeMove = 0;
                StartCoroutine(esperaSeMove());
            }
                

            if (!S_controleTutorial.tutorial1)
            {
                if (seMovendo && escudo.active == true) escudo.SetActive(false);
                if (!seMovendo && escudo.active == false) escudo.SetActive(true);
            }
        }

        if (dirEqui == "c" || posPerna.Contains("F"))
        {
            if (jog1)
            {
                S_moveTudo.J1dirX = 0f;
                S_moveTudo.J1dirY = 0f;
            }
            else
            {
                S_moveTudo.J2dirX = 0f;
                S_moveTudo.J2dirY = 0f;
            }
        }
        else if (posPerna.Contains("A"))
        {
            if (jog1)
            {
                if (dirEqui == "f") S_moveTudo.J1dirX = -0.4f;
                if (dirEqui == "t") S_moveTudo.J1dirX = 0.4f;
                if (dirEqui == "d") S_moveTudo.J1dirY = -2f;
                if (dirEqui == "e") S_moveTudo.J1dirY = 2f;

                if (dirEqui == "e" || dirEqui == "d") S_moveTudo.J1dirX = 0;
                if (dirEqui == "f" || dirEqui == "t") S_moveTudo.J1dirY = 0;
            }
            else
            {
                if (dirEqui == "f") S_moveTudo.J2dirX = 0.4f;
                if (dirEqui == "t") S_moveTudo.J2dirX = -0.4f;
                if (dirEqui == "d") S_moveTudo.J2dirY = 2f;
                if (dirEqui == "e") S_moveTudo.J2dirY = -2f;

                if (dirEqui == "e" || dirEqui == "d") S_moveTudo.J2dirX = 0;
                if (dirEqui == "f" || dirEqui == "t") S_moveTudo.J2dirY = 0;
            }
        }
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

    public IEnumerator Fragil()
    {
        vulneravel = true;
        Senergia.energia -= 15f;
        yield return new WaitForSecondsRealtime(3f);
        vulneravel = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!S_verificaGolpe.timeSlow && other.CompareTag("ch")) StartCoroutine(S_verificaGolpe.Vgolpe.Derrota(adversario, this));
    }
}
