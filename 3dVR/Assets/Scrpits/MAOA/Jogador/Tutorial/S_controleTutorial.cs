using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_controleTutorial : MonoBehaviour
{
    S_jogador jogador;
    public GameObject balaoFala;
    public GameObject botao;
    public TextMeshPro quadroDfala;
    public S_verificaGolpe SVgolpe;
    public static bool emTutorial = true;
    public static bool passa = false;

    // partes do tutorial
    public static bool tutorial1 = true;
    bool fazendoBatalha = false;

    [Header("PRIMEIRA PARTE")]
    public static bool Pparte = true;
    public GameObject discoEquilibrio;
    public S_Equilibrio Sequilibrio;

    [Header("SEGUNDA PARTE")]
    public static bool Sparte = false;
    public GameObject[] RIGimao;
    S_IK maoD;
    S_IK maoE;
    public GameObject bot;

    [Header("TERCEIRA PARTE")]
    public static bool Tparte = false;
    public GameObject[] RIGperna;
    public S_Postura Spostura;

    [Header("QUARTA PARTE")]
    public static bool Qparte = false;

    [Header("QUINTA PARTE")]
    public static bool QIparte = false;
    public Sbot_jogador adversario;
    public GameObject discoEquilibrioBOT;

    [Header("SEXTA PARTE")]
    public static bool SEparte = false;

    [Header("SETIMA PARTE")]
    public static bool STparte = false;
    public S_energia Senergia;

    void Awake()
    {
        if (S_controleCena.modo != S_controleCena.ModoJogo.Tutorial) enabled = false;
        jogador = GetComponent<S_jogador>();
        maoD = RIGimao[0].gameObject.GetComponent<S_IK>();
        maoE = RIGimao[1].gameObject.GetComponent<S_IK>();
        emTutorial = true;
    }

    private void Start()
    {
        SVgolpe = S_verificaGolpe.Vgolpe;
        balaoFala.SetActive(true);
        quadroDfala = balaoFala.GetComponentInChildren<TextMeshPro>();

        if (!fazendoBatalha)
        {
            S_pontos.Spontos.pontos1 = 0;
            S_pontos.Spontos.pontos2 = 0;

            if (Pparte) StartCoroutine(PrimeiraParte());
            if (Sparte) StartCoroutine(SegundaParte());
            if (Tparte) StartCoroutine(TerceiraParte());
            if (Qparte) StartCoroutine(QuartaParte());
            if (QIparte) StartCoroutine(QuintaParte());
            if (SEparte) StartCoroutine(SextaParte());
            if (STparte) StartCoroutine(SetimaParte());
        }
        else
        {
            if (this == S_pontos.Spontos.jogadores[0])
            {
                if (S_pontos.Spontos.pontos1 >= 2)
                {
                    StartCoroutine(SprimeiraParte());
                }
                else if (S_pontos.Spontos.pontos2 >= 2)
                {
                    S_pontos.Spontos.pontos1 = 0;
                    S_pontos.Spontos.pontos2 = 0;
                }
            }
            else
            {
                if (S_pontos.Spontos.pontos2 >= 2)
                {
                    StartCoroutine(SprimeiraParte());
                }
                else if (S_pontos.Spontos.pontos1 >= 2)
                {
                    S_pontos.Spontos.pontos1 = 0;
                    S_pontos.Spontos.pontos2 = 0;
                }
            }
        }
    }

    private void Update()
    {
        if (Senergia.energia <= 100 && !SEparte) Senergia.energia = 999999;
    }

    IEnumerator PrimeiraParte()
    {
        Sequilibrio.enabled = false;
        discoEquilibrio.SetActive(false);

        Spostura.enabled = false;

        foreach (GameObject rig in RIGimao) rig.SetActive(false);
        foreach (GameObject rig in RIGperna) rig.SetActive(false);

        discoEquilibrioBOT.SetActive(false);
        bot.SetActive(false);
        Sbot_jogador.dificuldade = 1;
        adversario.enabled = false;

        Senergia.energia = 999999999f;

        yield return StartCoroutine(Escreve("Este é o seu MAOÁ. Estamos vendo ele através de imagens de um satélite especial equipado neste robô.", 7));
        yield return StartCoroutine(Escreve("Ele é composto de 3 partes principais: Cabeça, Imãos e Pés. Cada uma interligada a uma parte fundamental do judô!", 7));

        discoEquilibrio.SetActive(true);
        Sequilibrio.enabled = true;

        yield return StartCoroutine(Escreve("Vamos começar pela cabeça, que cuida do Equilíbrio. Em baixo do seu MAOÁ tem um disco dividido em 5 partes, cada uma simbolizando uma direção.", 8));
        yield return StartCoroutine(Escreve("Quando você move seu Oculos VR em alguma direção, o círculo laranja se moverá junto com ele.", 7));
        yield return StartCoroutine(Escreve("E quando ele estiver em cima de uma das partes, ela ficará brilhante, definindo o seu equilíbrio naquela direção", 7));

        yield return StartCoroutine(Escreve("Vamos aprender um pouco. Tente colocar seu equilíbrio para frente, movendo seu Oculos VR para frente.", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "f");
        yield return StartCoroutine(Escreve("Muito bem! Agora para trás!", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "t");
        yield return StartCoroutine(Escreve("Muito bem! Agora para direita!", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "d");
        yield return StartCoroutine(Escreve("Muito bem! Agora para esquerda!", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "e");
        yield return StartCoroutine(Escreve("Muito bem! Agora para o centro!", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "c");

        yield return StartCoroutine(Escreve("Isso ai!!! Você pegou o jeito. Agora vamos explorar o corpo do MAOÁ, começando pelas Imãos.", 7));

        Pparte = false;
        StartCoroutine(SegundaParte());
    }

    IEnumerator SegundaParte()
    {
        Sparte = true;

        yield return StartCoroutine(Escreve("Como disse antes, na sua frente você está vendo o seu MAOÁ por uma mega tela. Porém, nossa super tecnolgia permite atraversarmos ela!", 8));

        foreach (GameObject rig in RIGimao) rig.SetActive(true);

        yield return StartCoroutine(Escreve("coloque suas mãos para frente, através da tela, e toque com uma no Imão direito e a outra no Imão esquerdo so seu MAOÁ.", 7));
        yield return StartCoroutine(Escreve("As imãos do MAOÁ podem ser seguradas e movimentadas", 5));
        yield return StartCoroutine(Escreve("Mantenha o 'GRAB' do seu controle pressionado enquanto próximo a uma Imão para fazer ela seguir sua mão. Solte o 'GRAB' para parar.", 20));
        yield return StartCoroutine(Escreve("Ótimo! agora, que você ja sabe como mover as Imãos do seu MAOÁ! Agora vamos aprender pra que isso serve!", 7));

        bot.SetActive(true);
        adversario.enabled = false;

        yield return StartCoroutine(Escreve("As imãos cuidam da pegada do judô. O seu adversário, igual a você, possui pontos de conexão em seu corpo localizado nas juntas do MAOÁ", 8));
        yield return StartCoroutine(Escreve("Esses pontos permitem que você conecte as Imãos do seu MAOÁ nelas, mudando sua pegada.", 7));
        yield return StartCoroutine(Escreve("Com sua mão direita, leve o imão direito até o conector do ombo do adversário esquerdo, enquanto segurnando ele, pressione 'TRIGGER'", 1));
        
        yield return new WaitUntil(() => maoD.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe");
        
        yield return StartCoroutine(Escreve("Ótimo, agora vamos fazer o mesmo com a outra imão. Segure ela, leve até o conector do ombo do adversário direito e, enquanto segurando ela pressione 'TRIGGER'", 1));
        
        yield return new WaitUntil(() => maoE.conectado != null && 
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");

        yield return StartCoroutine(Escreve("Isso ai! Como voce pode ver, enquanto conectada, o imão e a sua mão do lado correspondente irão ficar brilhosas", 7));
        yield return StartCoroutine(Escreve("Além disso, um imão conectado não pode ser segurado, é preciso primeiro desconectar ele.", 7));
        yield return StartCoroutine(Escreve("Desconecte a imão direito clicando no 'TRIGGER' da mão direita", 5));
        
        yield return new WaitUntil(() => maoD.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Agora desconecte o imao esquerdo clicando no 'TRIGGER' da mão esquerda", 5));
        
        yield return new WaitUntil(() => maoE.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Perfeito! agora vamos tentar mais uma vez, mas segurando em pontos diferentes", 6));
        yield return StartCoroutine(Escreve("Coloque o imao esquerdo no quadril e o direito no cotovelo.", 5));
        
        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Q");

        yield return StartCoroutine(Escreve("Agora desconecte ambas", 1));
       
        yield return new WaitUntil(() => maoE.estado == S_IK.estadoMao.livre && maoD.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Isso ai! Pegou o jeito. Agora que sabemos sobre o equilíbrio e as imãos, vamos a última parte do MAOÁ, as pernas", 7));

        Sparte = false;
        StartCoroutine(TerceiraParte());
    }

    IEnumerator TerceiraParte()
    {
        Tparte = true;

        foreach (GameObject rig in RIGperna) rig.SetActive(true);

        yield return StartCoroutine(Escreve("Semelhante as imãos, as pernas também podem ser seguradas com o 'GRIP' e movimentadas. Tente!", 7));
        yield return StartCoroutine(Escreve("Elas cuidam da sua postura! Ponha as pernas na posição inicial para começarmos.", 1));

        Spostura.enabled = true;
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => jogador.posPerna.Contains("F") &&
        RIGperna[0].GetComponent<S_dis_pe>().segurando == false &&
        RIGperna[1].GetComponent<S_dis_pe>().segurando == false);
        for (int i = 0; i < RIGperna.Length; i++) StartCoroutine(RIGperna[i].GetComponent<S_dis_pe>().Mover(false, false));
        yield return new WaitUntil(() => RIGperna[0].GetComponent<S_dis_pe>().movendo == false &&
        RIGperna[1].GetComponent<S_dis_pe>().movendo == false);

        yield return StartCoroutine(Escreve("Existem duas posturas: Fechada, quando suas pernas estão juntas, e Aberta, quando elas estão afastadas.", 7));
        yield return StartCoroutine(Escreve("No momento você esta na fechada. Vamos trocar para a aberta. Segure cada perna e mova uma para frente e outra para trás.", 1));

        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));

        yield return StartCoroutine(Escreve("Isso! Agora sua postura é Aberta!", 5));
        yield return StartCoroutine(Escreve("Enquanto aberta, mover seu equilíbrio movimenta você e seu adversário pelo mapa!", 5));
        yield return StartCoroutine(Escreve("Equilíbrio para frente ou para trás, move para frente ou para trás. Enquanto esquerda e direita roda você para esquerda ou direita!", 5));
        yield return StartCoroutine(Escreve("Mas exploramos disso mais tarde. Por agora, feche sua postura e então abra novamente.", 0));

        yield return new WaitUntil(() => jogador.posPerna.Contains("F"));
        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));

        yield return StartCoroutine(Escreve("Perfeito!!! Você ja sabe sobre todas as bases do judo. Que tal avançarmos um pouco e botarmos em prática realizando um golpe?", 7));

        Tparte = false;
        StartCoroutine(QuartaParte());
    }

    IEnumerator QuartaParte() // fazendo golpes
    {
        Qparte = true;

        yield return StartCoroutine(Escreve("Para realizar um golpe, temos que juntar tudo que aprendemos até agora. Equilíbrio, pegadas de ambas as imãos e a postura!", 7));
        yield return StartCoroutine(Escreve("Cada um possui requisitos para ser ativado, mas não se preocupe! Quando concluídos, o golpe será usado automaticamente!", 7));
        yield return StartCoroutine(Escreve("vamos testar com o golpe XXXXX. Pra isso, conecte a Imão esquerda no Ombro esquerdo e a Imão direita no quadril", 1));

        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Q" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");
        yield return StartCoroutine(Escreve("Isso! fizemos as imãos. Agora troque sua postura para Aberta!", 1));
        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));
        yield return StartCoroutine(Escreve("Para finalizar, vamos ativar esse golpe colocando no equilíbrio correto! Ponha seu equilíbrio para a esquerda.", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "e");

        yield return new WaitUntil(() => jogador.dirEqui == "e" && jogador.posPerna.Contains("A") &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Q" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");

        yield return StartCoroutine(Escreve("Isso ai, você realizou um golpe!", 5));
        yield return StartCoroutine(Escreve("No momenti não aconteceu nada, mas é porque estavamos apenas testando.", 5));
        yield return StartCoroutine(Escreve("Agora que você aprendeu sobre as 3 partes essenciais do seu MAOÁ e como ativar golpes, vamos avançar para a segunda parte!", 8));

        Qparte = false;
        StartCoroutine(QuintaParte());
    }

    IEnumerator QuintaParte()
    {
        QIparte = true;

        yield return StartCoroutine(Escreve("Você deve ter percebido que ao realizar um golpe apenas um pequeno efeito aconteceu, mas isso é porque estavamos apenas testando.", 8));
        yield return StartCoroutine(Escreve("Durante uma verdadeira luta de judô você deve acertar a posição do corpo e depois realizar uma projeção!", 7));
        yield return StartCoroutine(Escreve("E agora que você ja sabe da posição, vamos aprender a projeção e como se defender de uma", 6));
        yield return StartCoroutine(Escreve("Realize mais um golpe, o XXXXX, para ativarmos uma projeção", 5));

        yield return StartCoroutine(Escreve("Conecte a Imão esquerda no Ombro esquerdo e a Imão direita do Ombro direito", 1));
        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");
        yield return StartCoroutine(Escreve("Troque sua postura para Aberta!", 1));
        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));
        yield return StartCoroutine(Escreve("Ponha seu equilíbrio no centro.", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "c");

        yield return new WaitUntil(() => jogador.dirEqui == "c" && jogador.posPerna.Contains("A") &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");

        S_verificaGolpe.esperaTime = true;
        StartCoroutine(S_verificaGolpe.Vgolpe.TimeSlow(S_verificaGolpe.Vgolpe.golpes[4], jogador, jogador.adversario));

        XRGrabInteractable pdesXR = SVgolpe.pDes.GetComponent<XRGrabInteractable>();

        pdesXR.trackPosition = false;
        pdesXR.trackRotation = false;
        pdesXR.enabled = false;

        yield return StartCoroutine(Escreve("Buuuuuummm~ Legal né? Entramos dentro da zona de projeção!", 5));
        yield return StartCoroutine(Escreve("Aqui dentro tudo fica super lento e duas coisas importântes acontecem: Um jogador tenta realizar uma projeção e o outro fugir dela", 8));
        yield return StartCoroutine(Escreve("Vamos primeiro falar do jogador realizando a projeção, que foi o que conseguiu realizar o golpe. No caso, você.", 7));
        yield return StartCoroutine(Escreve("Entre as suas imãos se criou um orbe e uma grande seta brilhante. Seu objetivo como atacante é levar o orbe até o fim da seta.", 8));
        yield return StartCoroutine(Escreve("Para fazer isso, aproxime uma de suas mãos dele e segure seu 'GRAB', igual você fez com as partes do seu MAOÁ.", 7));
        yield return StartCoroutine(Escreve("Mas fique atento! O orbe NÃO PODE SAIR DA SETA, caso contrário ele seu MAOÁ perderá o impulso e sairá da zona de projeção.", 1));

        pdesXR.trackPosition = true;
        pdesXR.trackRotation = true;
        pdesXR.enabled = true;

        while (S_verificaGolpe.timeSlow)
        {
            if (S_verificaGolpe.Spde.noCaminho == false)
            {
                Destroy(SVgolpe.pDes);
                SVgolpe.pDes = null;
                SVgolpe.CriarPonto(1, jogador, jogador.adversario);
            }

            if (S_verificaGolpe.Spde.tocouClimax == true) break;
            yield return null;
        }

        S_verificaGolpe.esperaTime = false;
        S_verificaGolpe.esperaDerrota = true;

        yield return StartCoroutine(Escreve("Vush! e lá se foi o adversário voando pelos ares!", 5));
        yield return StartCoroutine(Escreve("Viu? É simples! você conseguiu fazer uma projeção de sucesso! mas lembre-se que em uma situação real, errar lhe tira da zona.", 8));

        QIparte = false;
        SEparte = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator SextaParte()
    {
        yield return null;

        Sequilibrio.enabled = false;
        foreach (GameObject rig in RIGimao) rig.SetActive(false);
        foreach (GameObject rig in RIGperna) rig.SetActive(false);
        S_verificaGolpe.esperaDerrota = false;

        yield return StartCoroutine(Escreve("Agora faremos o oposto. Você será atingido por um golpe e irá realizar uma fuga!", 6));

        Sbot_jogador.dificuldade = 5;
        adversario.enabled = true;
        adversario.golpe = S_verificaGolpe.Vgolpe.golpes[4];

        S_verificaGolpe.esperaTime = true;

        yield return new WaitUntil(() => S_verificaGolpe.timeSlow == true);

        yield return StartCoroutine(Escreve("Bom, você foi atingido por um golpe. Quando isso acontecer, seu disco de equilíbrio, aqule em baixo de você, ficará com um dos paineis brilhando.", 8));
        yield return StartCoroutine(Escreve("E para você fugir do golpe, você deve mover seu equilíbrio para essa direção antes que o oponente leve o orbe de projeção até o fim da seta dele.", 8));

        Sequilibrio.enabled = true;
        yield return new WaitUntil(() => jogador.dirEqui == adversario.golpe.IdirEqui);

        adversario.enabled = false;
        S_verificaGolpe.esperaTime = false;

        yield return StartCoroutine(Escreve("Isso ai! Você se defendeu do golpe trocando seu equilíbrio antes do tempo! Quando fizer isso, seu oponente ficará desestabilizado e soltará tudo.", 8));
        yield return StartCoroutine(Escreve("Mas lembre-se, seu oponente também pode fazer isso! Então quando você for o atacante, mova seu orbe até a ponta da seta o quanto antes.", 8));
        yield return StartCoroutine(Escreve("Agora você ja sabe sobre quase tudo! só falta uma coisinha: energia!", 5));

        SEparte = false;
        STparte = true;
        S_controleCena.RenovaCena(SceneManager.GetActiveScene().name);
    }

    IEnumerator SetimaParte()
    {
        discoEquilibrio.SetActive(true);
        Sequilibrio.enabled = true;
        foreach (GameObject rig in RIGimao) rig.SetActive(true);
        foreach (GameObject rig in RIGperna) rig.SetActive(true);
        Spostura.enabled = true;
        discoEquilibrioBOT.SetActive(true);
        bot.SetActive(true);

        yield return StartCoroutine(Escreve("Igual a outras máquinas, o seu MAOÁ também precisa de energia para funcionar", 5));

        Senergia.energia = 100f;

        yield return StartCoroutine(Escreve("Ao redor do seus disco de equilíbrio, você possui uma barra de energia, indo de 100% até 0%", 7));
        yield return StartCoroutine(Escreve("Ela não pode ser recuperada por ações e desce ao longo do tempo, e quando chegar a 0 seu MAOÁ irá parar de funcionar por um tempo enquanto ela se regenera.", 8));
        yield return StartCoroutine(Escreve("Vamos realizar uma batalha teste, sabendo de tudo que você soube de até agora!", 6));

        STparte = false;

        Sbot_jogador.dificuldade = 1;
        fazendoBatalha = true;
        S_controleCena.RenovaCena(SceneManager.GetActiveScene().name);
    }

    // SEGUNDA METADE DO TUTORIAL

    IEnumerator SprimeiraParte() // ensia movimento pelo mapa
    {
        Senergia.energia = 99999999f;
        tutorial1 = false;
        foreach (GameObject rig in RIGimao) rig.SetActive(true);

        yield return StartCoroutine(Escreve("Ótimo! agora que você ja dominou o básico, vamos aprender algumas situações mais específicas. Mas não se preocupe! elas são fáceis.", 8));
        yield return StartCoroutine(Escreve("Você ja sabe sobre as pernas, que elas podem mudar sua postura, mas ela podem fazer mais uma coisa: Mover você e o adversário pelo mapa!", 5));
        yield return StartCoroutine(Escreve("Enquanto com a perna aberta, permanecer com seu equilíbrio em alguma direção movimenta você e seu adversário", 8));
        yield return StartCoroutine(Escreve("Colocar para os lados faz vocês girarem, e colocar para frente ou para trás empurra nas direções respectivas", 1));
        yield return StartCoroutine(Escreve("Tente se mover um pouco!", 5));

        yield return StartCoroutine(Escreve("Quando um dos combatentes toca com seu pé fora da área do tatami, a partida finaliza, dando 1 ponto para o adversário.", 5));
        yield return StartCoroutine(Escreve("Lembre-se: os jogadores compartilham sua força, ou seja, eles podem anular ou aumentar a força de movimento ou giro dependendo de como estão cada um.", 5));

        StartCoroutine(SsegundaParte());
    }

    IEnumerator SsegundaParte() // ensia gastos de energia
    {
        Senergia.energia = Senergia.energiaMax;
        yield return StartCoroutine(Escreve("Durante suas partidas, você percebeu que sua energia desce de forma bem lenta, mas é porque nós desativamos os aparatos que a utilizavam.", 1));
        yield return StartCoroutine(Escreve("Existem 4 formas de perder energia: \n" + "Manter sua postura abera. \n" + "manter seus braços longe do ombro. \n" + "Trocar seu equilíbrio. \n" + "Falhar em realizar um golpe.", 5));
        yield return StartCoroutine(Escreve("Todos esse jeitos consomem rapidamente sua energia, então tome cuidado, pois ficar sem energia é algo fatal!", 6));

        StartCoroutine(SterceiraParte());
    }

    IEnumerator SterceiraParte() // ensia vulnerável
    {
        yield return StartCoroutine(Escreve("Vamos aprender sobre a última coisa: Fraqueza.", 1));
        yield return StartCoroutine(Escreve("No judô, diferente de como treinamos com o robô agora a pouco, você não consegue utilizar golpes em qualquer momento. Você precisa achar o 'Momentum'.", 6));
        yield return StartCoroutine(Escreve("Mover/segurar seus braços, pernas ou enquanto trocando de equilíbrio deixa você vulnerável pela duração.", 6));
        yield return StartCoroutine(Escreve("Ou seja, só é possivel acertar um golpe em um oponente que esteja fazendo alguma ação de trnasição. Igual para ele que só pode te atingir da mesma forma.", 5));

        Destroy(S_verificaGolpe.Vgolpe.gameObject);
        S_controleCena.modo = S_controleCena.ModoJogo.PvE;
        S_controleCena.RenovaCena(SceneManager.GetActiveScene().name);
    }

    IEnumerator Escreve(string fala, int t) //yield return StartCoroutine(Escreve("", t));
    {
        quadroDfala.text = fala;
        yield return null;
        if (t > 0) yield return new WaitForSeconds(t);
        botao.SetActive(true);
        yield return new WaitUntil(() => passa);
        passa = false;
        botao.SetActive(false);
    }
}
