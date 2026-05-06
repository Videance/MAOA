using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class S_controleTutorial : MonoBehaviour
{
    S_jogador jogador;

    [Header("PRIMEIRA PARTE")]
    bool Pparte = false;
    public GameObject[] RIGperna;
    public GameObject pngPostura;

    [Header("SEGUNDA PARTE")]
    bool Sparte = false;
    public GameObject[] RIGimao;

    [Header("TERCEIRA PARTE")]
    bool Tparte = false;
    public GameObject[] discoEquilibrio;
    public S_Equilibrio Sequilibrio;

    [Header("QUARTA PARTE")]
    bool Qparte = false;
    public S_energia energia;

    void Awake()
    {
        jogador = GetComponent<S_jogador>();
    }

    void Update()
    {
        
    }

    IEnumerator PrimeiraParte()
    {
        Pparte = true;
        foreach (GameObject rig in RIGperna) rig.SetActive(true);

        // "Este é o seu MAOÁ. Estamos vendo ele através de imagens de um satélite especial equipado neste robô.
        // "Normalmente, seria apenas uma tela transmitindo algo, mas nossa tecnologia permite atraversarmos ela!
        // "coloque suas mãos para frente, através da tela, como se quisesse tocar no MAOÁ."

        pngPostura.SetActive(true);
    }

    IEnumerator SegundaParte()
    {
        foreach (GameObject rig in RIGimao) rig.SetActive(true);
    }

    IEnumerator TerceiraParte()
    {
        foreach (GameObject disco in discoEquilibrio) disco.SetActive(true);
        Sequilibrio.enabled = true;
    }

    IEnumerator QuartaParte()
    {
        energia.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
