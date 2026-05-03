using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sbot_jogador : S_jogador
{
    public C_golpes golpe;
    public List<bool> golpeP; //0 - equi | 1 - perna | 2 - mao D | 3 - mao E

    public Sbot_IK maoD;
    public Sbot_IK maoE;

    float t;
    int n = 0;
    public int dificuldade = 1; // 1 a 5

    private void Start()
    {
        golpeP = new List<bool>();
        for (int i = 0; i < 4; i++) golpeP.Add(false);

        ProcuraGolpe(dificuldade, out golpe);

        VerificaVar();
    }

    private void Update()
    {
        return;

        t += Time.unscaledDeltaTime;
        if (t >= (5 / dificuldade))
        {
            t = 0;
            VerificaVar();          
        }
    }

    public void ProcuraGolpe(int lv, out C_golpes golpe)
    {
        int T = S_verificaGolpe.Vgolpe.golpes.Count;
        int nT = Random.Range(0, T);
        if (nT == n) nT++;
        if (nT > T) nT = 0;

        C_golpes golpeT = S_verificaGolpe.Vgolpe.golpes[nT];
        
        if (dificuldade >= 3)
        {
            //ve a distancia dos golpes do braços, e dependendo se não de pra alcancar, rola;
        }

        n = nT;
        golpe = golpeT;
    }

    public void VerificaVar()
    {
        for (int i = 0; i < golpeP.Count; i++) { golpeP[i] = false; }

        if (dirEqui == golpe.JdirEqui) golpeP[0] = true;
        if (pernaAberta == golpe.pernaAberta) golpeP[1] = true;
        if (imaoDir == golpe.conectorImaoDir) golpeP[2] = true;
        if (imaoEsq == golpe.conectorImaoEsq) golpeP[3] = true;

        int q = 0;
        foreach (bool b in golpeP)
        {
            if (b) q++;
            if (q == 4)
            {
                S_verificaGolpe.Vgolpe.AcharGolpe(this, adversario);
                ProcuraGolpe(dificuldade, out golpe);
                VerificaVar();
                return;
            }
        }
        
        EscolheQuemMove();
    }

    void EscolheQuemMove()
    {
        int q = Random.Range(0, 4);

        q = 2; // momentaneo

        switch (q)
        {
            case 0:
                if (golpeP[0]) goto case 1;
                golpeP[0] = true;
                return;
            case 1:
                if (golpeP[1]) goto case 2;
                golpeP[1] = true;
                return;
            case 2:
                if (golpeP[2]) goto case 3;
                golpeP[2] = true;
                maoD.alvoC = golpe.conectorImaoDir;
                foreach (S_Conector v in adversario.conectores)
                    if (v.localDoCorpo == golpe.conectorImaoDir)
                    {
                        StartCoroutine(maoD.Move(v.GetComponent<Transform>()));
                        break;
                    }
                return;
            case 3:
                if (golpeP[3]) goto case 0;
                golpeP[3] = true;
                maoE.alvoC = golpe.conectorImaoDir;
                foreach (S_Conector v in adversario.conectores)
                    if (v.localDoCorpo == golpe.conectorImaoEsq)
                    {
                        StartCoroutine(maoE.Move(v.GetComponent<Transform>()));
                        break;
                    }
                return;
        }
    }
}
