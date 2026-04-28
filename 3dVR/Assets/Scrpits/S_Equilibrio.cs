using System.Collections.Generic;
using UnityEngine;

public class S_Equilibrio : MonoBehaviour
{
    public GameObject pCentral;
    protected Vector3 inicialPos;
    protected Vector3 JinicialPos;
    public S_jogador jogador;
    protected S_energia energia;

    [Header("Valor do equilibrio")]
    public float dist = 0.0768f;
    public float XYdir = 0.74f;
    public float multiplicador = 1f;

    [Header("Direcao do equilibrio")]
    public string direcaoEquilibrio;
    protected bool primeira = true;

    [Header("Cores")]
    public Color corNormal = Color.white;
    public Color corAtiva = Color.blue;
    public List<Renderer> blocos = new List<Renderer>();

    private void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
        energia = GetComponentInParent<S_energia>();
        inicialPos = pCentral.transform.position;
        JinicialPos = transform.position;
        TrocaEquilibrio("c", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (pCentral == null || energia.rodandoSS) return;

        Vector3 offset = transform.position - JinicialPos;
        offset.y = 0;

        float mag = offset.magnitude;

        if (mag < 0.0001f) return;

        float distancia = Mathf.Min(mag * multiplicador, dist);

        Vector3 alvo = inicialPos + offset.normalized * distancia;

        pCentral.transform.position = Vector3.Lerp(
            pCentral.transform.position,
            alvo,
            Time.deltaTime * 20f
        );

        if (Vector3.Distance(pCentral.transform.position, inicialPos) <= (dist * 0.52f) && direcaoEquilibrio != "c") TrocaEquilibrio("c", 0);
        else if (Vector3.Distance(pCentral.transform.position, inicialPos) >= (dist * 0.6f))
        {
            Vector3 dir = (pCentral.transform.position - inicialPos).normalized;
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
        S_verificaGolpe.Vgolpe.AcharGolpe(jogador, jogador.adversario);
        if (primeira) primeira = false;
        else energia.energia -= 3;
        energia.energia = Mathf.Clamp(energia.energia, 0, energia.energiaMax);
        for (int i = 0; i < blocos.Count; i++)
        {
            if (i != index) blocos[i].material.color = corNormal;
            else blocos[i].material.color = corAtiva;
        }
    }
}
