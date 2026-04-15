using System.Collections.Generic;
using UnityEngine;

public class S_Equilibrio : MonoBehaviour
{
    public GameObject pCentral;
    public S_jogador jogador;

    [Header("Valor do equilibrio")]
    public float dist = 0.2f; //0.3 até 0.2;
    public float XYdir = 0.6f; //0.75 até 0.6

    private Vector3 dir;

    [Header("Direcao do equilibrio")]
    public string direcaoEquilibrio;

    [Header("Cores")]
    public Color corNormal = Color.white;
    public Color corAtiva = Color.blue;
    public List<Renderer> blocos = new List<Renderer>();

    // Update is called once per frame
    void Update()
    {
        if (pCentral == null) return;

        if (Vector3.Distance(pCentral.transform.position, transform.position) < (dist * 0.9f)) TrocaEquilibrio("c", 0);
        else if (Vector3.Distance(pCentral.transform.position, transform.position) > dist)
        {
            dir = (transform.position - pCentral.transform.position).normalized;
            if (dir.x > XYdir) TrocaEquilibrio("d", 2);
            else if (dir.x < -XYdir) TrocaEquilibrio("e", 4);
            else if (dir.z > XYdir) TrocaEquilibrio("f", 1);
            else if (dir.z < -XYdir) TrocaEquilibrio("t", 3);
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
