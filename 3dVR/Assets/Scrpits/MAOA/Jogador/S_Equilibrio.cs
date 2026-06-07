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
    public string dirFulga = null;
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

    private int blocoAtual = -1;
    private int faixaEnergia = -1;

    bool noEnergy = false;

    [Header("Cores C")]
    protected Color Cbase;
    protected Color Cazul;
    protected Color Cvermelho;
    protected Color Cpreto;

    [Header("Cores B")]
    protected Color Bverde;
    protected Color Bamarelo;
    protected Color Blaranja;
    protected Color Bvermelho;
    protected Color Bpreto;
    protected Color Bbverde;
    protected Color Bbamarelo;
    protected Color Bblaranja;
    protected Color Bbvermelho;

    [Header("Cores L")]
    public Material[] materials; // 0 = amarelinho bonitinho | 1 = brilho
    protected Color Lazul;
    protected Color Lvermelho;
    protected Color Lpreto;

    protected void Awake()
    {
        ColorUtility.TryParseHtmlString("#E2BFA1", out Cbase);
        ColorUtility.TryParseHtmlString("#1B426C", out Cazul);
        ColorUtility.TryParseHtmlString("#6C1B1B", out Cvermelho);
        ColorUtility.TryParseHtmlString("#3D3D3D", out Cpreto);

        ColorUtility.TryParseHtmlString("#00C521", out Bbverde);
        ColorUtility.TryParseHtmlString("#8DC500", out Bbamarelo);
        ColorUtility.TryParseHtmlString("#C56300", out Bblaranja);
        ColorUtility.TryParseHtmlString("#C61600", out Bbvermelho);
        ColorUtility.TryParseHtmlString("#000000", out Bpreto);
        ColorUtility.TryParseHtmlString("#00890B", out Bverde);
        ColorUtility.TryParseHtmlString("#5E8900", out Bamarelo);
        ColorUtility.TryParseHtmlString("#895000", out Blaranja);
        ColorUtility.TryParseHtmlString("#740300", out Bvermelho);

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
        AtualizarEnergiaVisual();

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

        if (dirFulga != null && letra == dirFulga) dirFulga = null;

        tempoMesmoEquilibrio = 0f;

        dirBalanco = Random.insideUnitSphere;
        dirBalanco.y = 0;
        dirBalanco.Normalize();

        S_verificaGolpe.Vgolpe.AcharGolpe(jogador, jogador.adversario);

        if (primeira) primeira = false;
        else if (!S_verificaGolpe.timeSlow && !S_controleTutorial.tutorial1) energia.energia -= 5;
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

            if (blocoAtual == i)
            {
                p[0].material = materials[1];
                p[0].material.SetColor("_Cor", Lpreto);
                p[1].material.color = Cpreto;
            }
        }
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
            TrocarCor(direcaoEquilibrio, dirFulga != null);
        }
    }

    protected void AtualizarCorEnergia()
    {
        float energia01 = energia.energia / energia.energiaMax;

        int novaFaixa;

        if (energia01 <= 0f) novaFaixa = 0;
        else if (energia01 <= 0.25f) novaFaixa = 1;
        else if (energia01 <= 0.50f) novaFaixa = 2;
        else if (energia01 <= 0.75f) novaFaixa = 3;
        else novaFaixa = 4;

        if (novaFaixa == faixaEnergia) return;

        faixaEnergia = novaFaixa;

        Color cor1 =
            novaFaixa == 0 ? Bpreto :
            novaFaixa == 1 ? Bvermelho :
            novaFaixa == 2 ? Blaranja :
            novaFaixa == 3 ? Bamarelo :
            Bverde;

        Color cor2 =
                novaFaixa == 0 ? Bpreto :
                novaFaixa == 1 ? Bbvermelho :
                novaFaixa == 2 ? Bblaranja :
                novaFaixa == 3 ? Bbamarelo :
                Bbverde;

        for (int i = 0; i < renderersBlocos.Length; i++)
        {
            renderersBlocos[i][2].material.SetColor("_CorCheia", cor1);
            renderersBlocos[i][3].material.SetColor("_CorCheia", cor2);
        }
    }

    protected void AtualizarEnergiaVisual()
    {
        float energia01 = energia.energia / energia.energiaMax;

        AtualizarCorEnergia();

        // Centro
        renderersBlocos[0][2].material.SetFloat("_Fill", energia01);
        renderersBlocos[0][3].material.SetFloat("_Fill", energia01);

        int[] ordemEnergia = { 4, 3, 2, 1 }; // F D T E

        float energiaExterior = energia01 * 4f;

        for (int i = 0; i < ordemEnergia.Length; i++)
        {
            float fill = Mathf.Clamp01(energiaExterior - i);

            int bloco = ordemEnergia[i];

            renderersBlocos[bloco][2].material.SetFloat("_Fill", fill);
            renderersBlocos[bloco][3].material.SetFloat("_Fill", fill);
        }
    }
}
