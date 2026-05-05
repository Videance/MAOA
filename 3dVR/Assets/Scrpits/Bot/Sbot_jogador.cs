using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sbot_jogador : S_jogador
{
    [Header("GOLPES")]
    public C_golpes golpe;
    public List<bool> golpeP; //0 - equi | 1 - perna | 2 - mao D | 3 - mao E

    public Sbot_IK maoD;
    public Sbot_IK maoE;

    float t;
    float tt = 0;
    float t1;
    float tt1 = 0;
    int n = 0;
    public int dificuldade = 1; // 1 a 5

    Sbot_equilibrio equilibrio;
    public List<GameObject> vectorPlacas = new List<GameObject>();

    Sbot_energia Senergia;

    private void Start()
    {
        equilibrio = GetComponent<Sbot_equilibrio>();
        foreach (Transform t in GetComponentsInChildren<Transform>()) if (t.CompareTag("e")) vectorPlacas.Add(t.gameObject);

        golpeP = new List<bool>();
        for (int i = 0; i < 4; i++) golpeP.Add(false);

        Senergia = GetComponent<Sbot_energia>();

        tt = Random.Range(3, 7) / dificuldade;
        tt1 = Random.Range(1.5f, 3f) / dificuldade;

        golpe = S_verificaGolpe.Vgolpe.golpes[Random.Range(0, S_verificaGolpe.Vgolpe.golpes.Count)];

        ProcuraGolpe(dificuldade);
    }

    private void Update()
    {
        if (!equilibrio.movendo)
        {
            t += Time.unscaledDeltaTime;

            if (Senergia.energia <= 0) return;
            if (t >= tt)
            {
                t = 0;
                tt = Random.Range(2.5f, 6.5f) / dificuldade;
                StartCoroutine(equilibrio.mover(Vector3.zero));
            }
        }

        t1 += Time.unscaledDeltaTime;
        if (Senergia.energia <= 0) return;
        if (t1 >= tt1)
        {
            if (equilibrio.movendo || Senergia.energia <= 0 || maoD.movendo || maoE.movendo || PEs[0].movendo || PEs[1].movendo) return;

            Debug.Log("moveu 1");
            tt = 0;
            tt1 = Random.Range(4f, 6.5f) / Mathf.Sqrt(dificuldade);
            VerificaVar();
        }
    }

    public void ProcuraGolpe(int lv)
    {
        int T = S_verificaGolpe.Vgolpe.golpes.Count;
        int nT = Random.Range(0, T);

        if (nT == n)
        {
            nT++;
            if (nT > T) nT = 0;
        }

        golpe = S_verificaGolpe.Vgolpe.golpes[nT];
        n = nT;
        VerificaVar();
    }

    public void VerificaVar()
    {
        golpeP[0] = (dirEqui == golpe.JdirEqui) ? true : false;
        golpeP[1] = (pernaAberta == golpe.pernaAberta) ? true : false;
        golpeP[2] = (imaoDir == golpe.conectorImaoDir) ? true : false;
        golpeP[3] = (imaoEsq == golpe.conectorImaoEsq) ? true : false;

        int q = 0;
        foreach (bool b in golpeP)
        {
            if (b) q++;
            if (q == 4)
            {
                S_verificaGolpe.Vgolpe.AcharGolpe(this, adversario);
                ProcuraGolpe(dificuldade);
                return;
            }
        }
        
        EscolheQuemMove();
    }

    void EscolheQuemMove()
    {
        int q = Random.Range(0, 4);

        switch (q)
        {
            case 0:
                if (golpeP[0]) goto case 1;
                golpeP[0] = true;
                if (golpe.JdirEqui == "c") StartCoroutine(equilibrio.mover(vectorPlacas[0].transform.position));
                if (golpe.JdirEqui == "t") StartCoroutine(equilibrio.mover(vectorPlacas[1].transform.position));
                if (golpe.JdirEqui == "d") StartCoroutine(equilibrio.mover(vectorPlacas[2].transform.position));
                if (golpe.JdirEqui == "f") StartCoroutine(equilibrio.mover(vectorPlacas[3].transform.position));
                if (golpe.JdirEqui == "e") StartCoroutine(equilibrio.mover(vectorPlacas[4].transform.position));
                return;
            case 1:
                if (golpeP[1]) goto case 2;
                golpeP[1] = true;
                foreach (S_dis_pe v in PEs) StartCoroutine(v.Mover(true));
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
