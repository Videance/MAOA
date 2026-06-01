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

    [Header("Instabilidade")]
    public float forcaBalanco = 0.05f;
    public float crescimentoBalanco = 0.03f;
    public float velocidadeBalanco = 1.5f;

    private Vector3 dirBalanco;
    private float tempoMesmoEquilibrio = 0f;

    [Header("Cores")]
    public List<GameObject> blocos = new List<GameObject>();
    private Renderer[][] renderersBlocos;

    private bool ultimoEstadoFuga = false;
    private int blocoAtual = -1;

    bool noEnergy = false;

    [Header("Cores C")]
    protected Color Cbase;
    protected Color Cazul;
    protected Color Cvermelho;
    protected Color Cpreto;

    [Header("Cores B")]
    protected Color Bazul;
    protected Color BBazul;
    protected Color Bvermelho;
    protected Color BBvermelho;
    protected Color Bpreto;
    protected Color BBpreto;

    [Header("Cores L")]
    public Material[] materials; // 0 = amarelinho bonitinho | 1 = brilho
    protected Color Lazul;
    protected Color Lvermelho;
    protected Color Lpreto;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#CDCDCD", out Cbase);
        ColorUtility.TryParseHtmlString("#1B426C", out Cazul);
        ColorUtility.TryParseHtmlString("#6C1B1B", out Cvermelho);
        ColorUtility.TryParseHtmlString("#3D3D3D", out Cpreto);

        ColorUtility.TryParseHtmlString("#28938E", out Bazul);
        ColorUtility.TryParseHtmlString("#217B77", out BBazul);
        ColorUtility.TryParseHtmlString("#932C28", out Bvermelho);
        ColorUtility.TryParseHtmlString("#7B2321", out BBvermelho);
        ColorUtility.TryParseHtmlString("#353535", out Bpreto);
        ColorUtility.TryParseHtmlString("#2E2E2E", out BBpreto);

        ColorUtility.TryParseHtmlString("#008BFF", out Lazul);
        ColorUtility.TryParseHtmlString("#FF0007", out Lvermelho);
        ColorUtility.TryParseHtmlString("#000000", out Lpreto);
    }

    protected virtual void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
        energia = GetComponentInParent<S_energia>();
        inicialPos = pCentral.transform.position;
        JinicialPos = transform.position;

        dirBalanco = UnityEngine.Random.insideUnitSphere;
        dirBalanco.y = 0;
        dirBalanco.Normalize();

        renderersBlocos = new Renderer[blocos.Count][];

        for (int i = 0; i < blocos.Count; i++)
        {
            renderersBlocos[i] = blocos[i].GetComponentsInChildren<Renderer>();
        }

        TrocaEquilibrio("c", 0);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        AtualizaEstadoEnergia();

        if (pCentral == null || energia.rodandoSS) return;

        tempoMesmoEquilibrio += Time.deltaTime;

        Vector3 offset = transform.position - JinicialPos;
        offset.y = 0;

        float mag = offset.magnitude;
        if (mag < 0.0001f) offset = Vector3.zero;

        float distancia = Mathf.Min(mag * multiplicador, dist);
        float intensidade = forcaBalanco + (tempoMesmoEquilibrio * crescimentoBalanco);

        // movimento oscilando
        float noiseX = Mathf.PerlinNoise(Time.time * velocidadeBalanco, 0f) - 0.5f;
        float noiseZ = Mathf.PerlinNoise(0f, Time.time * velocidadeBalanco) - 0.5f;

        Vector3 balanco = new Vector3(noiseX, 0, noiseZ) * intensidade;
        if (S_controleTutorial.emTutorial) balanco = Vector3.zero;

        Vector3 alvo = inicialPos + offset.normalized * distancia + balanco;

        pCentral.transform.position = Vector3.Lerp(pCentral.transform.position, alvo, Time.unscaledDeltaTime * speedi);

        // --------------------------------------------------------------
        float porcentagemEnergia = energia.energia / energia.energiaMax;
        if (contadorTroca == 0) tempoTroca = Mathf.Lerp(1f, 0.25f, porcentagemEnergia);

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
            contadorTroca -= Time.unscaledDeltaTime;

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

        tempoMesmoEquilibrio = 0f;

        dirBalanco = Random.insideUnitSphere;
        dirBalanco.y = 0;
        dirBalanco.Normalize();

        S_verificaGolpe.Vgolpe.AcharGolpe(jogador, jogador.adversario);

        if (primeira) primeira = false;
        else if (!S_verificaGolpe.timeSlow) energia.energia -= 5;
        energia.energia = Mathf.Clamp(energia.energia, 0, energia.energiaMax);

        TrocarCor(letra, false);
    }

    public virtual void TrocarCor(string letra, bool emFuga)
    {
        if (emFuga)
        {
            tempoMesmoEquilibrio = 0f;

            dirBalanco = Random.insideUnitSphere;
            dirBalanco.y = 0;
            dirBalanco.Normalize();

            dirFulga = letra;
        }

        if (noEnergy)
        {
            SemCor();
            return;
        }

        int index = 0;
        if (letra == "c") index = 0;
        if (letra == "t") index = 3;
        if (letra == "d") index = 2;
        if (letra == "f") index = 1;
        if (letra == "e") index = 4;

        // Atualiza B e BB apenas se o estado mudou
        if (ultimoEstadoFuga != emFuga) AtualizarBB(emFuga);

        // Desliga o bloco anterior
        if (!emFuga && blocoAtual >= 0 && blocoAtual != index)
        {
            Renderer[] pAntigo = renderersBlocos[blocoAtual];

            pAntigo[0].material = materials[0];
            pAntigo[1].material.color = Cbase;
        }

        // Liga o bloco novo
        Renderer[] pNovo = renderersBlocos[index];
        pNovo[0].material = materials[1];

        if (emFuga)
        {
            pNovo[0].material.SetColor("_Cor", Lvermelho);
            pNovo[1].material.color = Cvermelho;
        }
        else
        {
            pNovo[0].material.SetColor("_Cor", Lazul);
            pNovo[1].material.color = Cazul;
        }

        blocoAtual = index;
    }

    public virtual void SemCor()
    {
        for (int i = 0; i < renderersBlocos.Length; i++)
        {
            Renderer[] p = renderersBlocos[i];

            p[2].material.color = BBpreto;
            p[3].material.color = Bpreto;

            if (blocoAtual == i)
            {
                p[0].material = materials[1];
                p[0].material.SetColor("_Cor", Lpreto);
                p[1].material.color = Cpreto;
            }
        }
    }

    protected void AtualizarBB(bool emFuga)
    {
        for (int i = 0; i < renderersBlocos.Length; i++)
        {
            Renderer[] p = renderersBlocos[i];

            if (emFuga)
            {
                p[2].material.color = BBvermelho;
                p[3].material.color = Bvermelho;
            }
            else
            {
                p[2].material.color = BBazul;
                p[3].material.color = Bazul;
            }
        }

        ultimoEstadoFuga = emFuga;
    }

    protected void AtualizaEstadoEnergia()
    {
        if (energia.rodandoSS && !noEnergy)
        {
            noEnergy = true;
            SemCor();
        }

        if (!energia.rodandoSS && noEnergy)
        {
            noEnergy = false;

            AtualizarBB(dirFulga != null);
            TrocarCor(direcaoEquilibrio, dirFulga != null);
        }
    }
}
