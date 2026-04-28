using System.Collections.Generic;
using UnityEngine;

public class Sbot_jogador : S_jogador
{
    C_golpes golpe;
    List<bool> golpeP; //0 - equi | 1 - perna | 2 - mao D | 3 - mao E

    override protected void Awake()
    {
        base.Awake();

        golpeP = new List<bool>();
        for (int i = 0; i < golpeP.Count; i++) { golpeP[i] = false; }

        golpe = S_verificaGolpe.Vgolpe.golpes[Random.Range(0, S_verificaGolpe.Vgolpe.golpes.Count)];
    }

    private void Update()
    {
        if (dirEqui == golpe.JdirEqui) golpeP[0] = true;
        else golpeP[0] = false;

        if (pernaAberta == golpe.pernaAberta) golpeP[1] = true;
        else golpeP[1] = false;

        if (imaoDir == golpe.conectorImaoDir) golpeP[2] = true;
        else golpeP[2] = false;

        if (imaoEsq == golpe.conectorImaoEsq) golpeP[3] = true;
        else golpeP[3] = false;

        EscolheQuemMove();
    }

    void EscolheQuemMove()
    {
        int q = 0;
        foreach (bool pode in golpeP) { if (pode) q++; }
    }
}
