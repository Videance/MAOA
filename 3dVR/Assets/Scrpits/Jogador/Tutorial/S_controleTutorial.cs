using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class S_controleTutorial : MonoBehaviour
{
    S_jogador jogador;

    [Header("PRIMEIRA PARTE")]
    bool Pparte = false;
    public GameObject[] discoEquilibrio;
    public S_Equilibrio Sequilibrio;

    [Header("SEGUNDA PARTE")]
    bool Sparte = false;
    public GameObject[] RIGimao;

    [Header("TERCEIRA PARTE")]
    bool Tparte = false;
    bool tocou = false;
    public GameObject[] RIGperna;
    public GameObject pngPostura;

    [Header("QUARTA PARTE")]
    bool Qparte = false;
    public S_energia energia;

    void Awake()
    {
        jogador = GetComponent<S_jogador>();
    }

    private void Start()
    {
        StartCoroutine(PrimeiraParte());
    }

    void Update()
    {
        
    }

    IEnumerator PrimeiraParte()
    {
        Pparte = true;

        // "Este é o seu MAOÁ. Estamos vendo ele através de imagens de um satélite especial equipado neste robô.
        yield return new WaitForSeconds(4f);
        // "Ele é composto de 3 partes principais: Cabeça, Imãos e Pés. Vamos aprender uma de cada vez, começando pela Cabeça!
        yield return new WaitForSeconds(4f);

        foreach (GameObject disco in discoEquilibrio) disco.SetActive(true);
        Sequilibrio.enabled = true;

        // "Em baixo do seu MAOÁ tem um disco dividido em 5 partes, e em cima dele, um círculo laranja."
        yield return new WaitForSeconds(4f);
        // "Quando você move seu Oculos VR em alguma direção, o círculo laranja se moverá junto com ele"
        yield return new WaitForSeconds(4f);
        // "E quando ele estiver em cima de uma das partes, ela ficará brilhante, definindo o seu equilíbrio"

        Pparte = true;
        yield return null;
    }

    IEnumerator SegundaParte()
    {
        foreach (GameObject rig in RIGimao) rig.SetActive(true);

        // "Normalmente, seria apenas uma tela transmitindo algo, mas nossa tecnologia permite atraversarmos ela!
        yield return new WaitForSeconds(4f);
        // "coloque suas mãos para frente, através da tela, como se quisesse tocar no MAOÁ."
        tocou = false;
        yield return new WaitUntil(() => tocou == true);

        yield return null;
    }

    IEnumerator TerceiraParte()
    {
        foreach (GameObject rig in RIGperna) rig.SetActive(true);

        pngPostura.SetActive(true);

        yield return null;
    }

    IEnumerator QuartaParte()
    {
        energia.enabled = true;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
