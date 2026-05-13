using System.Collections.Generic;
using UnityEngine;
using System;

public class S_Equilibrio : MonoBehaviour
{
    public GameObject pCentral;
    protected Vector3 inicialPos;
    protected Vector3 JinicialPos;
    public S_jogador jogador;
    protected S_energia energia;

    [Header("Valor do equilibrio")]
    protected float dist = 0.576f;
    protected float XYdir = 0.74f;
    public float multiplicador = 1f;
    private float speedi = 15f;

    [Header("Direcao do equilibrio")]
    public string direcaoEquilibrio;
    protected bool primeira = true;
    protected string dirFulga = null;
    protected float tempoTroca = 0.75f;
    public string equilibrioCandidato = null;
    protected float contadorTroca = 0f;

    [Header("Cores")]
    public Color corNormal = Color.white;
    public Color corAtiva = Color.blue;
    public Color corFuga = Color.red;
    public List<Renderer> blocos = new List<Renderer>();

    protected virtual void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
        energia = GetComponentInParent<S_energia>();
        inicialPos = pCentral.transform.position;
        JinicialPos = transform.position;
        TrocaEquilibrio("c", 0);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (pCentral == null || energia.rodandoSS) return;

        Vector3 offset = transform.position - JinicialPos;
        offset.y = 0;

        float mag = offset.magnitude;
        if (mag < 0.0001f) return;

        float distancia = Mathf.Min(mag * multiplicador, dist);
        Vector3 alvo = inicialPos + offset.normalized * distancia;

        pCentral.transform.position = Vector3.Lerp(pCentral.transform.position, alvo, Time.deltaTime * speedi);

        // --------------------------------------------------------------
        float porcentagemEnergia = energia.energia / energia.energiaMax;
        tempoTroca = Mathf.Lerp(1f, 0.25f, porcentagemEnergia);

        string novoEquilibrio = null;
        float distanciaCentro = Vector3.Distance(pCentral.transform.position, inicialPos);

        if (distanciaCentro <= (dist * 0.52f))
        {
            novoEquilibrio = "c";
        }
        else if (distanciaCentro >= (dist * 0.6f))
        {
            Vector3 dir = (pCentral.transform.position - inicialPos).normalized;

            if (dir.x > XYdir) novoEquilibrio = "d";
            else if (dir.x < -XYdir) novoEquilibrio = "e";
            else if (dir.z > XYdir) novoEquilibrio = "f";
            else if (dir.z < -XYdir) novoEquilibrio = "t";
        }

        // se não encontrou equilíbrio válido ou é igual
        if (novoEquilibrio == null || novoEquilibrio == direcaoEquilibrio)
        {
            equilibrioCandidato = null;
            contadorTroca = 0f;
            return;
        }

        // começou novo candidato
        if (equilibrioCandidato != novoEquilibrio)
        {
            equilibrioCandidato = novoEquilibrio;
            contadorTroca = tempoTroca;
        }
        else
        {
            contadorTroca -= Time.deltaTime;

            if (contadorTroca <= 0f)
            {
                int index = 0;

                if (novoEquilibrio == "c") index = 0;
                if (novoEquilibrio == "t") index = 1;
                if (novoEquilibrio == "d") index = 2;
                if (novoEquilibrio == "f") index = 3;
                if (novoEquilibrio == "e") index = 4;

                TrocaEquilibrio(novoEquilibrio, index);

                equilibrioCandidato = null;
            }
        }
    }

    public virtual void TrocaEquilibrio(string letra, int index)
    {
        if (jogador.dirEqui == letra) return;
        direcaoEquilibrio = letra;
        jogador.dirEqui = letra;

        if (dirFulga != null)
        {
            if (letra != dirFulga) return;
            else dirFulga = null;
        }

        S_verificaGolpe.Vgolpe.AcharGolpe(jogador, jogador.adversario);

        if (primeira) primeira = false;
        else if (!S_verificaGolpe.timeSlow) energia.energia -= 5;
        energia.energia = Mathf.Clamp(energia.energia, 0, energia.energiaMax);

        TrocarCor(letra);
    }

    public virtual void PlacaFuga(string letra)
    {
        dirFulga = letra;

        blocos[0].material.color = letra == "c" ? corFuga : corNormal;
        blocos[1].material.color = letra == "t" ? corFuga : corNormal;
        blocos[2].material.color = letra == "d" ? corFuga : corNormal;
        blocos[3].material.color = letra == "f" ? corFuga : corNormal;
        blocos[4].material.color = letra == "e" ? corFuga : corNormal;
    }

    public void TrocarCor(string letra)
    {
        int index = 0;
        if (letra == "c") index = 0;
        if (letra == "t") index = 1;
        if (letra == "d") index = 2;
        if (letra == "f") index = 3;
        if (letra == "e") index = 4;

        for (int i = 0; i < blocos.Count; i++)
        {
            if (i != index) blocos[i].material.color = corNormal;
            else blocos[i].material.color = corAtiva;
        }
    }
}
