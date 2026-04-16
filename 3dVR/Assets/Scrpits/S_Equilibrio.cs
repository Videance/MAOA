using System.Collections.Generic;
using UnityEngine;

public class S_Equilibrio : MonoBehaviour
{
    public GameObject pCentral;
    private Vector3 ultimaPos;
    private Vector3 inicialPos;
    private Vector3 JinicialPos;
    public S_jogador jogador;

    [Header("Valor do equilibrio")]
    public float dist = 0.2f; //0.3 até 0.2;
    public float XYdir = 0.55f; //0.75 até 0.6

    private Vector3 dir;

    [Header("Direcao do equilibrio")]
    public string direcaoEquilibrio;

    [Header("Cores")]
    public Color corNormal = Color.white;
    public Color corAtiva = Color.blue;
    public List<Renderer> blocos = new List<Renderer>();

    private void Start()
    {
        ultimaPos = transform.position;
        inicialPos = pCentral.transform.position;
        JinicialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pCentral == null) return;

        if (Vector3.Distance(transform.position, JinicialPos) <= dist)
        {
            Vector3 dis = transform.position - ultimaPos;
            pCentral.transform.position += new Vector3(dis.x, 0, dis.z);
            ultimaPos = transform.position;
        }
        else if (Vector3.Distance(transform.position, JinicialPos) >= dist)
        {
            Vector3 dir = (transform.position - JinicialPos).normalized;
            pCentral.transform.position = inicialPos + new Vector3(dir.x, 0, dir.z) * dist;
        }

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
        direcaoEquilibrio = letra;
        jogador.dirEqui = letra;
        for (int i = 0; i < blocos.Count; i++)
        {
            if (i != index) blocos[i].material.color = corNormal;
            else blocos[i].material.color = corAtiva; ;
        }
    }
}
