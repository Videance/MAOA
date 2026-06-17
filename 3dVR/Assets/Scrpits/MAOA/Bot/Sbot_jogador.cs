using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sbot_jogador : S_jogador
{
    [Header("GOLPES")]
    public C_golpes golpe;
    public List<bool> golpeP; //0 - equi | 1 - perna | 2 - mao D | 3 - mao E
    S_Conector conMD;
    S_Conector conME;
    bool fazendoGolpe = false;
    int q = 0;

    public Sbot_IK maoD;
    public Sbot_IK maoE;

    float t;
    float tt = 0;
    float t1;
    float tt1 = 0;
    int n = 0;
    public static int dificuldade = 1; // 1 a 99

    Sbot_equilibrio equilibrio;
    public List<GameObject> vectorPlacas = new List<GameObject>();
    public GameObject pDesequilibrio;
    public Collider[] colliderSeta;

    protected override void Awake()
    {
        base.Awake();

        equilibrio = GetComponent<Sbot_equilibrio>();
        foreach (Transform t in GetComponentsInChildren<Transform>()) if (t.CompareTag("e")) vectorPlacas.Add(t.gameObject);

        golpeP = new List<bool>();
        for (int i = 0; i < 4; i++) golpeP.Add(false);
    }

    private void Start()
    {
        tt = Random.Range(3, 7) / dificuldade;
        tt1 = Random.Range(1.5f, 3f) / dificuldade;

        golpe = S_verificaGolpe.Vgolpe.golpes[Random.Range(0, S_verificaGolpe.Vgolpe.golpes.Count)];

        ProcuraGolpe(dificuldade);
    }

    private void Update()
    {
        if (!equilibrio.movendo && golpeP[0] == false) t += Time.unscaledDeltaTime;
        if (!fazendoGolpe) t1 += Time.unscaledDeltaTime;

        if (!S_controleTutorial.emTutorial)
        {
            if (seMovendo && escudo.IsAlive())
            {
                escudo.Stop();
                escudo.Clear();
            }
            else
            {
                escudo.Play();
            }
        }

        if (Senergia.rodandoSS) return;

        if (t >= tt)
        {
            t = 0;
            tt = Random.Range(2f, 7f) / (dificuldade * 1.35f);
            int lado = Random.Range(0, 5);
            StartCoroutine(equilibrio.mover(vectorPlacas[lado].transform.position));
        }
        else if (equilibrio.fugaQualPlaca != null)
        {
            if (equilibrio.fugaQualPlaca == "c") StartCoroutine(equilibrio.mover(vectorPlacas[0].transform.position));
            if (equilibrio.fugaQualPlaca == "t") StartCoroutine(equilibrio.mover(vectorPlacas[0].transform.position));
            if (equilibrio.fugaQualPlaca == "d") StartCoroutine(equilibrio.mover(vectorPlacas[0].transform.position));
            if (equilibrio.fugaQualPlaca == "f") StartCoroutine(equilibrio.mover(vectorPlacas[0].transform.position));
            if (equilibrio.fugaQualPlaca == "e") StartCoroutine(equilibrio.mover(vectorPlacas[0].transform.position));

            equilibrio.fugaQualPlaca = null;
        }

        if (t1 >= tt1)
        {
            t1 = 0;
            tt1 = Random.Range(4f, 8.5f) / Mathf.Sqrt(dificuldade);
            int vel = Random.Range(2, 7);
            if (vel <= dificuldade) StartCoroutine(FazendoGolpe(true));
            else StartCoroutine(FazendoGolpe(false));
        }

        seMovendo = (maoD.movendo || maoE.movendo ||
            PEs[0].movendo || PEs[1].movendo || Sequilibrio.equilibrioCandidato != null || vulneravel)
            ? true : false;

        if (dirEqui == "c" || posPerna.Contains("F") || Senergia.rodandoSS)
        {
            S_moveTudo.J2dirX = 0f;
            S_moveTudo.J2dirY = 0f;
        }
        else if (posPerna.Contains("A"))
        {
            if (dirEqui == "f") S_moveTudo.J2dirX = 0.8f;
            if (dirEqui == "t") S_moveTudo.J2dirX = -0.8f;
            if (dirEqui == "d") S_moveTudo.J2dirY = 2f;
            if (dirEqui == "e") S_moveTudo.J2dirY = -2f;

            if (dirEqui == "e" || dirEqui == "d") S_moveTudo.J2dirX = 0;
            if (dirEqui == "f" || dirEqui == "t") S_moveTudo.J2dirY = 0;
        }
    }

    IEnumerator FazendoGolpe(bool rapidinho)
    {
        fazendoGolpe = true;
        VerificaVar(5);
        float espera = 0;
        int troca = 0;

        while (q < 4)
        {
            EscolheQuemMove();

            espera = rapidinho ? Random.Range(1.2f, 2f) / Mathf.Sqrt(dificuldade) : Random.Range(4f, 6.5f) / Mathf.Sqrt(dificuldade);
            yield return new WaitForSeconds(espera);

            troca = Random.Range(0, 10);

            if (troca == 0 && dificuldade != 1) rapidinho = !rapidinho;
            if (dificuldade >= 3 && !rapidinho && troca <= dificuldade && troca != 0) rapidinho = true;

            VerificaVar(6);
        }

        fazendoGolpe = false;
    }

    public void ProcuraGolpe(int lv)
    {
        if (maoD.conectado != null) maoD.Desconecta();
        if (maoE.conectado != null) maoE.Desconecta();

        int T = S_verificaGolpe.Vgolpe.golpes.Count;
        int nT = Random.Range(0, T);

        int tentativas = 0;
        while (tentativas < T)
        {
            C_golpes golpinho = S_verificaGolpe.Vgolpe.golpes[nT];

            bool invalido = false;

            // evita repetir golpe anterior
            if (nT == n) invalido = true;
            else if (dificuldade >= 3 && golpinho.IdirEqui == adversario.dirEqui) invalido = true;
            else
            {
                foreach (S_Conector v in adversario.conectores)
                {
                    // MÃO ESQUERDA
                    if (v.localDoCorpo == golpinho.conectorImaoEsq)
                    {
                        if (Vector3.Distance(v.transform.position, maoE.DisGrab.GOinicial.transform.position) > maoE.DisGrab.dis)
                        {
                            invalido = true;
                            break;
                        }
                        else conME = v;
                    }

                    // MÃO DIREITA
                    if (v.localDoCorpo == golpinho.conectorImaoDir)
                    {
                        if (Vector3.Distance(v.transform.position, maoD.DisGrab.GOinicial.transform.position) > maoD.DisGrab.dis)
                        {
                            invalido = true;
                            break;
                        }
                        else conMD = v;
                    }
                }
            }

            // se for válido, usa ele
            if (!invalido)
            {
                golpe = golpinho;
                n = nT;
                VerificaVar(5);
                return;
            }

            // senão tenta próximo
            nT++;
            if (nT >= T) nT = 0;

            tentativas++;
        }

        // fallback (se nenhum válido)
        golpe = S_verificaGolpe.Vgolpe.golpes[nT];
        n = nT;
    }

    public void VerificaVar(int qual)
    {
        if (S_verificaGolpe.timeSlow) return;

        if (qual == 0) golpeP[0] = (dirEqui == golpe.JdirEqui) ? true : false;
        if (qual == 1) golpeP[1] = (posPerna == golpe.pernaAberta) ? true : false;
        if (qual == 2) golpeP[2] = (imaoDir == golpe.conectorImaoDir) ? true : false;
        if (qual == 3) golpeP[3] = (imaoEsq == golpe.conectorImaoEsq) ? true : false;
        if (qual == 5)
        {
            golpeP[0] = (dirEqui == golpe.JdirEqui) ? true : false;
            golpeP[1] = (posPerna == golpe.pernaAberta) ? true : false;
            golpeP[2] = (imaoDir == golpe.conectorImaoDir) ? true : false;
            golpeP[3] = (imaoEsq == golpe.conectorImaoEsq) ? true : false;
        }

        q = 0;
        foreach (bool b in golpeP) if (b) q++;
        if (q == 4 && qual <= 5)
        {
            StartCoroutine(S_verificaGolpe.Vgolpe.TimeSlow(golpe, this, adversario));
            if (S_verificaGolpe.timeSlow) return;

            ProcuraGolpe(dificuldade);
        }
    }

    void EscolheQuemMove()
    {
        if (Senergia.rodandoSS || (golpeP[0] || equilibrio.movendo) &&
            (golpeP[1] || PEs[0].movendo || PEs[1].movendo) &&
            (golpeP[2] || maoD.movendo) && (golpeP[3] || maoE.movendo))
            return;

        int q = Random.Range(0, 4);

        switch (q)
        {
            case 0:
                if (golpeP[0] || equilibrio.movendo || equilibrio.equilibrioCandidato != null) goto case 1;
                if (golpe.JdirEqui == "c") StartCoroutine(equilibrio.mover(vectorPlacas[0].transform.position));
                if (golpe.JdirEqui == "t") StartCoroutine(equilibrio.mover(vectorPlacas[1].transform.position));
                if (golpe.JdirEqui == "d") StartCoroutine(equilibrio.mover(vectorPlacas[2].transform.position));
                if (golpe.JdirEqui == "f") StartCoroutine(equilibrio.mover(vectorPlacas[3].transform.position));
                if (golpe.JdirEqui == "e") StartCoroutine(equilibrio.mover(vectorPlacas[4].transform.position));
                return;
            case 1:
                if (golpeP[1] || PEs[0].movendo || PEs[1].movendo) goto case 2;
                foreach (S_dis_pe v in PEs)
                    if (golpe.pernaAberta == "Ae") StartCoroutine(v.Mover(true, true));
                    else if (golpe.pernaAberta == "Ad") StartCoroutine(v.Mover(true, false));
                    else StartCoroutine(v.Mover(false, false));
                return;
            case 2:
                if (golpeP[2] || maoD.movendo) goto case 3;
                maoD.alvoC = golpe.conectorImaoDir;
                StartCoroutine(maoD.Move(conMD.GetComponent<Transform>()));
                return;
            case 3:
                if (golpeP[3] || maoE.movendo) goto case 0;
                maoE.alvoC = golpe.conectorImaoEsq;
                StartCoroutine(maoE.Move(conME.GetComponent<Transform>()));
                return;
        }
    }

    public IEnumerator MoverPdesequilibrio(GameObject pDese, GameObject seta)
    {
        colliderSeta = seta.GetComponentsInChildren<Collider>();

        for (int i = 0; i < colliderSeta.Length; i++)
        {
            Collider box = colliderSeta[i];
            if (box == null) break;
            Vector3 centroMundo = box.bounds.center;

            float vel = equilibrio.speedMax * Random.Range(0.33f, 0.5f);
            if (S_controleTutorial.emTutorial) vel = vel / 40;

            while (pDese != null && Vector3.Distance(pDese.transform.position, centroMundo) > 0.0001f)
            {
                pDese.transform.position = Vector3.MoveTowards(pDese.transform.position, centroMundo, vel * Time.unscaledDeltaTime);
                yield return null;
            }
        }
    }
}
