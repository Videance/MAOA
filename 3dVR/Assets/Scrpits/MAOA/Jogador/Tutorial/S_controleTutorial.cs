using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_controleTutorial : MonoBehaviour
{
    public S_jogador jogador;
    public S_controleCena controleCena;
    public GameObject balaoFala;
    public GameObject botao;
    public TextMeshPro quadroDfala;
    public S_verificaGolpe SVgolpe;
    public static bool emTutorial = true;
    public static bool passa = false;

    // partes do tutorial
    public static bool tutorial1 = true;

    [Header("PRIMEIRA PARTE")]
    public static bool Pparte = true;
    public GameObject discoEquilibrio;
    public S_Equilibrio Sequilibrio;

    [Header("SEGUNDA PARTE")]
    public static bool Sparte = false;
    public GameObject[] RIGimao;
    public S_IK maoD;
    public S_IK maoE;
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

    private void Start()
    {
        if (S_controleCena.modo != S_controleCena.ModoJogo.Tutorial)
        {
            balaoFala.SetActive(false);
            enabled = false;
        }

        emTutorial = true;

        balaoFala.SetActive(true);
        botao.SetActive(false);


        if (tutorial1)
        {
            S_pontos.Spontos.pontos1 = 0;
            S_pontos.Spontos.pontos2 = 0;

            StartCoroutine(PrimeiraParte());
        }
        else StartCoroutine(SprimeiraParte());
    }

    public void PegarVar()
    {
        GameObject MAOA = controleCena.Jogadores;
        S_jogador[] jogadores = MAOA.GetComponentsInChildren<S_jogador>();

        Debug.Log(jogadores.Length);

        foreach (S_jogador j in jogadores)
        {
            if (j is Sbot_jogador)
            {
                bot = j.gameObject;
                adversario = bot.GetComponent<Sbot_jogador>();
                discoEquilibrioBOT = bot.GetComponentInChildren<S_off>().gameObject;
            }
            else
            {
                GameObject jg = j.gameObject;

                jogador = j;
                discoEquilibrio = jg.GetComponentInChildren<S_off>().gameObject;
                Sequilibrio = jg.GetComponent<S_Equilibrio>();

                var rigs = jg.GetComponentsInChildren<TwoBoneIKConstraint>();
                foreach (var rig in rigs)
                {
                    if (rig.name.Contains("bra dir"))
                        RIGimao[0] = rig.gameObject;
                    else if (rig.name.Contains("bra esq"))
                        RIGimao[1] = rig.gameObject;
                    else if (rig.name.Contains("per dir"))
                        RIGperna[0] = rig.gameObject;
                    else if (rig.name.Contains("per esq"))
                        RIGperna[1] = rig.gameObject;
                }

                maoD = RIGimao[0].GetComponentInChildren<S_IK>();
                maoE = RIGimao[1].GetComponentInChildren<S_IK>();
                Spostura = jg.GetComponentInChildren<S_Postura>();
                Senergia = jg.GetComponent<S_energia>();
            }
        }
    }

    IEnumerator PrimeiraParte()
    {
        Sequilibrio.enabled = false;
        discoEquilibrio.SetActive(false);

        Spostura.enabled = false;

        foreach (GameObject rig in RIGimao) rig.SetActive(false);
        foreach (GameObject rig in RIGperna) rig.SetActive(false);

        discoEquilibrioBOT.SetActive(false);
        Sbot_jogador.dificuldade = 1;
        bot.SetActive(false);

        Senergia.energia = 999999999f;

        yield return StartCoroutine(Escreve("Este é o seu MAOÁ. Estamos vendo ele através de imagens de um satélite especial equipado neste robô.", 4, true));
        yield return StartCoroutine(Escreve("Ele é composto de 3 partes principais: Cabeça, Imãos e Pés. Cada uma interligada a uma parte fundamental do judô!", 4, true));

        discoEquilibrio.SetActive(true);
        Sequilibrio.enabled = true;

        yield return StartCoroutine(Escreve("Vamos começar pela cabeça, que cuida do Equilíbrio. Em baixo do seu MAOÁ tem um disco dividido em 5 partes, cada uma simbolizando uma direção.", 4, true));
        yield return StartCoroutine(Escreve("Quando você move seu Oculos VR em alguma direção, o círculo laranja se moverá junto com ele.", 4, true));
        yield return StartCoroutine(Escreve("E quando ele estiver em cima de uma das partes, ela ficará brilhante, definindo o seu equilíbrio naquela direção", 4, true));

        yield return StartCoroutine(Escreve("Vamos aprender um pouco. Tente colocar seu equilíbrio para frente, movendo seu Oculos VR para frente.", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "f");
        yield return StartCoroutine(Escreve("Muito bem! Agora para trás!", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "t");
        yield return StartCoroutine(Escreve("Muito bem! Agora para direita!", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "d");
        yield return StartCoroutine(Escreve("Muito bem! Agora para esquerda!", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "e");
        yield return StartCoroutine(Escreve("Muito bem! Agora para o centro!", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "c");

        yield return StartCoroutine(Escreve("Isso ai!!! Você pegou o jeito. Agora vamos explorar o corpo do MAOÁ, começando pelas Imãos.", 4, true));

        Pparte = false;
        StartCoroutine(SegundaParte());
    }

    IEnumerator SegundaParte()
    {
        Sparte = true;

        yield return StartCoroutine(Escreve("Como disse antes, na sua frente você está vendo o seu MAOÁ por uma mega tela. Porém, nossa super tecnolgia permite atraversarmos ela!", 4, true));

        foreach (GameObject rig in RIGimao) rig.SetActive(true);

        yield return StartCoroutine(Escreve("coloque suas mãos para frente, através da tela, e toque com uma no Imão direito e a outra no Imão esquerdo so seu MAOÁ.", 4, true));
        yield return StartCoroutine(Escreve("As imãos do MAOÁ podem ser seguradas e movimentadas", 4, true));
        yield return StartCoroutine(Escreve("Mantenha o 'GRAB' do seu controle pressionado enquanto próximo a uma Imão para fazer ela seguir sua mão. Solte o 'GRAB' para parar.", 15, true));
        yield return StartCoroutine(Escreve("Ótimo! agora, que você ja sabe como mover as Imãos do seu MAOÁ! Agora vamos aprender pra que isso serve!", 4, true));

        bot.SetActive(true);

        yield return StartCoroutine(Escreve("As imãos cuidam da pegada do judô. O seu adversário, igual a você, possui pontos de conexão em seu corpo localizado nas juntas do MAOÁ", 4, true));
        yield return StartCoroutine(Escreve("Esses pontos permitem que você conecte as Imãos do seu MAOÁ nelas, mudando sua pegada.", 4, true));
        yield return StartCoroutine(Escreve("Com sua mão direita, leve o imão direito até o conector do ombo do adversário esquerdo, enquanto segurnando ele, pressione 'TRIGGER'", 1, false));
        
        yield return new WaitUntil(() => maoD.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe");
        
        yield return StartCoroutine(Escreve("Ótimo, agora vamos fazer o mesmo com a outra imão. Segure ela, leve até o conector do ombo do adversário direito e, enquanto segurando ela pressione 'TRIGGER'", 1, false));
        
        yield return new WaitUntil(() => maoE.conectado != null && 
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");

        yield return StartCoroutine(Escreve("Isso ai! Como voce pode ver, enquanto conectada, o imão e a sua mão do lado correspondente irão ficar brilhosas", 4, true));
        yield return StartCoroutine(Escreve("Além disso, um imão conectado não pode ser segurado, é preciso primeiro desconectar ele.", 4, true));
        yield return StartCoroutine(Escreve("Desconecte a imão direito clicando no 'TRIGGER' da mão direita", 1, false));
        
        yield return new WaitUntil(() => maoD.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Agora desconecte o imao esquerdo clicando no 'TRIGGER' da mão esquerda", 1, false));
        
        yield return new WaitUntil(() => maoE.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Perfeito! agora vamos tentar mais uma vez, mas segurando em pontos diferentes", 4, true));
        yield return StartCoroutine(Escreve("Coloque o imao esquerdo no quadril e o direito no cotovelo.", 1, false));
        
        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Q");

        yield return StartCoroutine(Escreve("Agora desconecte ambas", 1, false));
       
        yield return new WaitUntil(() => maoE.estado == S_IK.estadoMao.livre && maoD.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Isso ai! Pegou o jeito. Agora que sabemos sobre o equilíbrio e as imãos, vamos a última parte do MAOÁ, as pernas", 4, true));

        Sparte = false;
        StartCoroutine(TerceiraParte());
    }

    IEnumerator TerceiraParte()
    {
        Tparte = true;

        foreach (GameObject rig in RIGperna) rig.SetActive(true);

        yield return StartCoroutine(Escreve("Semelhante as imãos, as pernas também podem ser seguradas com o 'GRIP' e movimentadas. Tente!", 4, true));
        yield return StartCoroutine(Escreve("Elas cuidam da sua postura! Ponha as pernas na posição inicial para começarmos.", 1, false));

        Spostura.enabled = true;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < RIGperna.Length; i++) StartCoroutine(RIGperna[i].GetComponentInChildren<S_dis_pe>().Mover(false, false));
        yield return new WaitUntil(() => RIGperna[0].GetComponentInChildren<S_dis_pe>().movendo == false &&
        RIGperna[1].GetComponentInChildren<S_dis_pe>().movendo == false);
        yield return new WaitUntil(() => jogador.posPerna.Contains("F") &&
RIGperna[0].GetComponentInChildren<S_dis_pe>().segurando == false &&
RIGperna[1].GetComponentInChildren<S_dis_pe>().segurando == false);

        yield return StartCoroutine(Escreve("Existem duas posturas: Fechada, quando suas pernas estão juntas, e Aberta, quando elas estão afastadas.", 4, true));
        yield return StartCoroutine(Escreve("No momento você esta na fechada. Vamos trocar para a aberta. Segure cada perna e mova uma para frente e outra para trás.", 1, false));

        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));

        yield return StartCoroutine(Escreve("Isso! Agora sua postura é Aberta!", 4, true));
        yield return StartCoroutine(Escreve("Enquanto aberta, mover seu equilíbrio movimenta você e seu adversário pelo mapa!", 4, true));
        yield return StartCoroutine(Escreve("Equilíbrio para frente ou para trás, move para frente ou para trás. Enquanto esquerda e direita roda você para esquerda ou direita!", 4, true));
        yield return StartCoroutine(Escreve("Mas exploramos disso mais tarde. Por agora, feche sua postura e então abra novamente.", 1, false));

        yield return new WaitUntil(() => jogador.posPerna.Contains("F"));
        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));

        yield return StartCoroutine(Escreve("Perfeito!!! Você ja sabe sobre todas as bases do judo. Que tal avançarmos um pouco e botarmos em prática realizando um golpe?", 4, true));

        Tparte = false;
        StartCoroutine(QuartaParte());
    }

    IEnumerator QuartaParte() // fazendo golpes
    {
        Qparte = true;

        yield return StartCoroutine(Escreve("Para realizar um golpe, temos que juntar tudo que aprendemos até agora. Equilíbrio, pegadas de ambas as imãos e a postura!", 4, true));
        yield return StartCoroutine(Escreve("Cada um possui requisitos para ser ativado, mas não se preocupe! Quando concluídos, o golpe será usado automaticamente!", 4, true));
        yield return StartCoroutine(Escreve("vamos testar com o golpe XXXXX. Pra isso, conecte a Imão esquerda no Ombro esquerdo e a Imão direita no quadril", 1, false));

        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Q" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");
        yield return StartCoroutine(Escreve("Isso! fizemos as imãos. Agora troque sua postura para Aberta!", 1, false));
        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));
        yield return StartCoroutine(Escreve("Para finalizar, vamos ativar esse golpe colocando no equilíbrio correto! Ponha seu equilíbrio para a esquerda.", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "e");

        yield return new WaitUntil(() => jogador.dirEqui == "e" && jogador.posPerna.Contains("A") &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Q" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");

        yield return StartCoroutine(Escreve("Isso ai, você realizou um golpe!", 4, true));
        yield return StartCoroutine(Escreve("No momenti não aconteceu nada, mas é porque estavamos apenas testando.", 4, true));
        yield return StartCoroutine(Escreve("Agora que você aprendeu sobre as 3 partes essenciais do seu MAOÁ e como ativar golpes, vamos avançar para a segunda parte!", 4, true));

        Qparte = false;
        StartCoroutine(QuintaParte());
    }

    IEnumerator QuintaParte()
    {
        QIparte = true;

        yield return StartCoroutine(Escreve("Você deve ter percebido que ao realizar um golpe apenas um pequeno efeito aconteceu, mas isso é porque estavamos apenas testando.", 4, true));
        yield return StartCoroutine(Escreve("Durante uma verdadeira luta de judô você deve acertar a posição do corpo e depois realizar uma projeção!", 4, true));
        yield return StartCoroutine(Escreve("E agora que você ja sabe da posição, vamos aprender a projeção e como se defender de uma", 4, true));
        yield return StartCoroutine(Escreve("Realize mais um golpe, o XXXXX, para ativarmos uma projeção", 4, true));

        yield return StartCoroutine(Escreve("Conecte a Imão esquerda no Ombro esquerdo e a Imão direita do Ombro direito", 1, false));
        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");
        yield return StartCoroutine(Escreve("Troque sua postura para Aberta!", 1, false));
        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));
        yield return StartCoroutine(Escreve("Ponha seu equilíbrio no centro.", 1, false));
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

        yield return StartCoroutine(Escreve("Buuuuuummm~ Legal né? Entramos dentro da zona de projeção!", 4, true));
        yield return StartCoroutine(Escreve("Aqui dentro tudo fica super lento e duas coisas importântes acontecem: Um jogador tenta realizar uma projeção e o outro fugir dela", 4, true));
        yield return StartCoroutine(Escreve("Vamos primeiro falar do jogador realizando a projeção, que foi o que conseguiu realizar o golpe. No caso, você.", 4, true));
        yield return StartCoroutine(Escreve("Entre as suas imãos se criou um orbe e uma grande seta brilhante. Seu objetivo como atacante é levar o orbe até o fim da seta.", 4, true));
        yield return StartCoroutine(Escreve("Para fazer isso, aproxime uma de suas mãos dele e segure seu 'GRAB', igual você fez com as partes do seu MAOÁ.", 4, true));
        yield return StartCoroutine(Escreve("Mas fique atento! O orbe NÃO PODE SAIR DA SETA, caso contrário ele seu MAOÁ perderá o impulso e sairá da zona de projeção.", 1, false));

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

        yield return StartCoroutine(Escreve("Vush! e lá se foi o adversário voando pelos ares!", 7, true));
        yield return StartCoroutine(Escreve("Viu? É simples! você conseguiu fazer uma projeção de sucesso! mas lembre-se que em uma situação real, errar lhe tira da zona.", 4, true));

        QIparte = false;
        SEparte = true;

        StartCoroutine(SextaParte());
    }

    IEnumerator SextaParte()
    {
        yield return null;

        Sequilibrio.enabled = false;
        foreach (GameObject rig in RIGimao) rig.SetActive(false);
        foreach (GameObject rig in RIGperna) rig.SetActive(false);
        S_verificaGolpe.esperaDerrota = false;

        yield return StartCoroutine(Escreve("Agora faremos o oposto. Você será atingido por um golpe e irá realizar uma fuga!", 2, false));

        Sbot_jogador.dificuldade = 5;
        Sbot_jogador.naoMover = false;
        adversario.golpe = S_verificaGolpe.Vgolpe.golpes[4];

        S_verificaGolpe.esperaTime = true;

        yield return new WaitUntil(() => S_verificaGolpe.timeSlow == true);

        yield return StartCoroutine(Escreve("Bom, você foi atingido por um golpe. Quando isso acontecer, seu disco de equilíbrio, aqule em baixo de você, ficará com um dos paineis brilhando.", 9, false));
        yield return StartCoroutine(Escreve("E para você fugir do golpe, você deve mover seu equilíbrio para essa direção antes que o oponente leve o orbe de projeção até o fim da seta dele.", 4, false));

        Sequilibrio.enabled = true;
        yield return new WaitUntil(() => jogador.dirEqui == adversario.golpe.IdirEqui);

        Sbot_jogador.naoMover = true;
        S_verificaGolpe.esperaTime = false;

        yield return StartCoroutine(Escreve("Isso ai! Você se defendeu do golpe trocando seu equilíbrio antes do tempo! Quando fizer isso, seu oponente ficará desestabilizado e soltará tudo.", 4, true));
        yield return StartCoroutine(Escreve("Mas lembre-se, seu oponente também pode fazer isso! Então quando você for o atacante, mova seu orbe até a ponta da seta o quanto antes.", 4, true));
        yield return StartCoroutine(Escreve("Agora você ja sabe sobre quase tudo! só falta uma coisinha: energia!", 4, true));

        SEparte = false;
        STparte = true;
        StartCoroutine(SetimaParte());
    }

    IEnumerator SetimaParte()
    {
        yield return StartCoroutine(Escreve("Igual a outras máquinas, o seu MAOÁ também precisa de energia para funcionar", 4, true));

        Senergia.energia = 100f;

        yield return StartCoroutine(Escreve("Ao redor do seus disco de equilíbrio, você possui uma barra de energia, indo de 100% até 0%", 4, true));
        yield return StartCoroutine(Escreve("Ela não pode ser recuperada por ações e desce ao longo do tempo, e quando chegar a 0 seu MAOÁ irá parar de funcionar por um tempo enquanto ela se regenera.", 4, true));

        yield return StartCoroutine(Escreve("Perder 25% de energia", 2, true));
        Senergia.energia -= 25f;
        yield return StartCoroutine(Escreve("Perder 25% de energia", 2, true));
        Senergia.energia -= 25f;
        yield return StartCoroutine(Escreve("Perder 25% de energia", 2, true));
        Senergia.energia -= 25f;
        yield return StartCoroutine(Escreve("Perder 25% de energia", 2, true));
        Senergia.energia -= 25f;

        yield return new WaitForSeconds(4f);

        yield return StartCoroutine(Escreve("Vamos realizar uma batalha teste, sabendo de tudo que você soube de até agora!", 4, true));

        STparte = false;

        Sbot_jogador.dificuldade = 1;
        FindAnyObjectByType<S_onClique>().TrocaUI(0);
        controleCena.ColocarMAOA(false);
        enabled = false;
    }

    // SEGUNDA METADE DO TUTORIAL

    IEnumerator SprimeiraParte() // ensia movimento pelo mapa
    {
        Senergia.energia = 99999999f;
        tutorial1 = false;
        foreach (GameObject rig in RIGimao) rig.SetActive(true);

        yield return StartCoroutine(Escreve("Ótimo! agora que você ja dominou o básico, vamos aprender algumas situações mais específicas. Mas não se preocupe! elas são fáceis.", 4, true));
        yield return StartCoroutine(Escreve("Você ja sabe sobre as pernas, que elas podem mudar sua postura, mas ela podem fazer mais uma coisa: Mover você e o adversário pelo mapa!", 4, true));
        yield return StartCoroutine(Escreve("Enquanto com a perna aberta, permanecer com seu equilíbrio em alguma direção movimenta você e seu adversário", 4, true));
        yield return StartCoroutine(Escreve("Colocar para os lados faz vocês girarem, e colocar para frente ou para trás empurra nas direções respectivas", 4, true));
        yield return StartCoroutine(Escreve("Tente se mover um pouco!", 4, true));

        yield return StartCoroutine(Escreve("Quando um dos combatentes toca com seu pé fora da área do tatami, a partida finaliza, dando 1 ponto para o adversário.", 4, true));
        yield return StartCoroutine(Escreve("Lembre-se: os jogadores compartilham sua força, ou seja, eles podem anular ou aumentar a força de movimento ou giro dependendo de como estão cada um.", 4, true));

        StartCoroutine(SsegundaParte());
    }

    IEnumerator SsegundaParte() // ensia gastos de energia
    {
        Senergia.energia = Senergia.energiaMax;
        yield return StartCoroutine(Escreve("Durante suas partidas, você percebeu que sua energia desce de forma bem lenta, mas é porque nós desativamos os aparatos que a utilizavam.", 4, true));
        yield return StartCoroutine(Escreve("Existem 4 formas de perder energia: \n" + "Manter sua postura abera. \n" + "manter seus braços longe do ombro. \n" + "Trocar seu equilíbrio. \n" + "Falhar em realizar um golpe.", 4, true));
        yield return StartCoroutine(Escreve("Todos esse jeitos consomem rapidamente sua energia, então tome cuidado, pois ficar sem energia é algo fatal!", 4, true));

        StartCoroutine(SterceiraParte());
    }

    IEnumerator SterceiraParte() // ensia vulnerável
    {
        yield return StartCoroutine(Escreve("Vamos aprender sobre a última coisa: Fraqueza.", 4, true));
        yield return StartCoroutine(Escreve("No judô, diferente de como treinamos com o robô agora a pouco, você não consegue utilizar golpes em qualquer momento. Você precisa achar o 'Momentum'.", 4, true));
        yield return StartCoroutine(Escreve("Mover/segurar seus braços, pernas ou enquanto trocando de equilíbrio deixa você vulnerável pela duração.", 4, true));
        yield return StartCoroutine(Escreve("Ou seja, só é possivel acertar um golpe em um oponente que esteja fazendo alguma ação de trnasição. Igual para ele que só pode te atingir da mesma forma.", 4, true));

        Destroy(S_verificaGolpe.Vgolpe.gameObject);
        S_controleCena.modo = S_controleCena.ModoJogo.PvE;
        emTutorial = false;
        FindAnyObjectByType<S_onClique>().TrocaUI(0);
        controleCena.ColocarMAOA(false);
        enabled = false;
    }

    IEnumerator Escreve(string fala, int t, bool next) //yield return StartCoroutine(Escreve("", t));
    {
        quadroDfala.text = fala;
        yield return null;
        if (t > 0) yield return new WaitForSeconds(0); //trocar pra t

        if (next)
        {
            botao.SetActive(true);
            yield return new WaitUntil(() => passa);
            passa = false;
            botao.SetActive(false);
        }
    }
}
