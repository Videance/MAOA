using System;
using System.Collections.Generic;
using UnityEngine;

public class S_Equilibrio : MonoBehaviour
{
    public GameObject pCentral;
    private Vector3 ultimaPos;
    private Vector3 inicialPos;
    private Vector3 JinicialPos;
    public S_jogador jogador;
    private S_energia energia;

    [Header("Valor do equilibrio")]
    public float dist = 0.0768f;
    public float XYdir = 0.74f;

    private Vector3 dir;

    [Header("Direcao do equilibrio")]
    public string direcaoEquilibrio;

    [Header("Cores")]
    public Color corNormal = Color.white;
    public Color corAtiva = Color.blue;
    public List<Renderer> blocos = new List<Renderer>();

    private void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
        energia = GetComponentInParent<S_energia>();
        ultimaPos = transform.position;
        inicialPos = pCentral.transform.position;
        JinicialPos = transform.position;
        blocos[0].material.color = corAtiva;
    }

    // Update is called once per frame
    void Update()
    {
        if (pCentral == null || energia.rodandoSS) return;

        Vector3 posAtual = transform.position;
        Vector3 dis = posAtual - ultimaPos;

        if (Vector3.Distance(posAtual, JinicialPos) < dist) pCentral.transform.position += new Vector3(dis.x, 0, dis.z);
        else
        {
            Vector3 dir = (posAtual - JinicialPos).normalized;
            pCentral.transform.position = inicialPos + new Vector3(dir.x, 0, dir.z) * dist;
        }

        ultimaPos = posAtual;

        if (Vector3.Distance(pCentral.transform.position, inicialPos) <= (dist * 0.52f) && direcaoEquilibrio != "c") TrocaEquilibrio("c", 0);
        else if (Vector3.Distance(pCentral.transform.position, inicialPos) >= (dist * 0.6f))
        {
            dir = (pCentral.transform.position - inicialPos).normalized;
            if (dir.x > XYdir && direcaoEquilibrio != "d") TrocaEquilibrio("d", 2);
            else if (dir.x < -XYdir && direcaoEquilibrio != "e") TrocaEquilibrio("e", 4);
            else if (dir.z > XYdir && direcaoEquilibrio != "f") TrocaEquilibrio("f", 3);
            else if (dir.z < -XYdir && direcaoEquilibrio != "t") TrocaEquilibrio("t", 1);
        }
    }

    public void TrocaEquilibrio(string letra, int index)
    {
        if (jogador.dirEqui == letra) return;
        direcaoEquilibrio = letra;
        jogador.dirEqui = letra;
        S_verificaGolpe.AcharGolpe(jogador, jogador.adversario);
        energia.energia -= 3;
        energia.energia = Mathf.Clamp(energia.energia, 0, energia.energiaMax);
        for (int i = 0; i < blocos.Count; i++)
        {
            if (i != index) blocos[i].material.color = corNormal;
            else blocos[i].material.color = corAtiva;
        }
    }
}
