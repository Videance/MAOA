using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_controleTutorial : MonoBehaviour
{
    public S_jogador jogador;
    public S_controleCena controleCena;
    public GameObject balaoFala;
    public GameObject botao;
    public TextMeshPro quadroDfala;
    public GameObject imagem;
    public GameObject imagem2;
    public GameObject golpesImagens;
    public S_verificaGolpe SVgolpe;
    public static bool emTutorial = true; //true
    public static bool passa = false;
    public S_onClique Sclique;
    public GameObject[] Controles;

    // partes do tutorial
    public static bool tutorial1 = true; //true

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
        imagem.SetActive(false);
        Sbot_jogador.dificuldade = 1;

        S_pontos.Spontos.pontos1 = 0;
        S_pontos.Spontos.pontos2 = 0;

        StartCoroutine(PrimeiraParte());
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
        golpesImagens.SetActive(false);
        Sequilibrio.enabled = false;
        discoEquilibrio.SetActive(false);

        Spostura.enabled = false;

        foreach (GameObject rig in RIGimao) rig.SetActive(false);
        foreach (GameObject rig in RIGperna) rig.SetActive(false);

        discoEquilibrioBOT.SetActive(false);
        Sbot_jogador.dificuldade = 1;
        bot.SetActive(false);

        Senergia.energia = 999999999f;

        yield return StartCoroutine(Escreve("Olá lutador! \r\nMe chamo Tigri- *tosse* Quero dizer, Kappy. \r\nEu sou o mascote, narrador e tutor aqui da arena e vou lhe ajudar na sua jornada!", 2, true));
        yield return StartCoroutine(Escreve("Você está dentro de um MAOÁ: Máquina de Alteração de Ordem Anatômica.\r\nEle é equipado com essa mega tela Ultra HD 8K 7680 x 4320 que permite vermos ele em terceira pessoa.", 2, true));
        yield return StartCoroutine(Escreve("Ele é composto de 3 partes principais, Cada uma interligada a uma parte fundamental do judô!\r\n          - Cabeça -\r\n          - Imãos - \r\n          - Pernas -", 2, true));

        discoEquilibrio.SetActive(true);
        Sequilibrio.enabled = true;

        yield return StartCoroutine(Escreve("Vamos começar pela cabeça, que cuida do Equilíbrio.\\r\\nEm baixo do seu MAOA tem um disco dividido em 5 partes.\r\nMexa sua cabeça para mover o disco laranja e trocar seu equilóbrio!", 2, true));

        yield return StartCoroutine(Escreve("Tente colocar seu equilíbrio para frente, movendo seu Oculos VR para frente.", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "f");
        yield return StartCoroutine(Escreve("Muito bem! Agora para trás!", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "t");
        yield return StartCoroutine(Escreve("Muito bem! Agora para direita!", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "d");
        yield return StartCoroutine(Escreve("Muito bem! Agora para esquerda!", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "e");
        yield return StartCoroutine(Escreve("Muito bem! Agora para o centro!", 1, false));
        yield return new WaitUntil(() => jogador.dirEqui == "c");

        Pparte = false;
        StartCoroutine(SegundaParte());
    }

    IEnumerator SegundaParte()
    {
        Sparte = true;

        yield return StartCoroutine(Escreve("Isso ai!!! Você pegou o jeito.\r\nAgora vamos para as Imões, que cuidam da pegada.\r\nAtravesse nossa tela tecnológica com suas mãos e as aproxime das Imãos do MAOA.", 2, true));

        foreach (GameObject rig in RIGimao) rig.SetActive(true);
        Controles[0].SetActive(true);

        yield return StartCoroutine(Escreve("Quando perto o suficiente, clique e mantenha o 'GRAB' do seu controle pressionado para segurar e mover aquela Imão.\r\nSolte o 'GRAB' para parar.\r\n        [teste um pouco]", 12, true));

        bot.SetActive(true);

        yield return StartCoroutine(Escreve("O seu adversário, igual a você, possui pontos de conexão no corpo. Umas esferas de energia roxa.\r\nEsses pontos permitem que você conecte as Imãos do seu MAOA nelas.", 2, true));

        Controles[0].SetActive(false);
        Controles[1].SetActive(true);

        yield return StartCoroutine(Escreve("Com sua mão direita, leve o seu Imão do lado azul [direita] até o conector do ombro do lado branco do adversário.\r\nAinda segurnando seu Imão, pressione o 'TRIGGER' para grudar ele.", 1, false));
        
        yield return new WaitUntil(() => maoD.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe");
        
        yield return StartCoroutine(Escreve("Ótimo! Agora vamos fazer o mesmo com o outr lado.\r\nCom sua mão esquerda, leve o seu Imão do lado branco [esquerda] até o conector do ombro do lado azul do adversário e pressione 'Trigger'.", 1, false));
        
        yield return new WaitUntil(() => maoE.conectado != null && 
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");

        yield return StartCoroutine(Escreve("Agora vamos desconectar elas! \r\nPara desconectar o Imão do lado azul [direita], pressione 'TRIGGER' do controle direito.\r\nPara desconectar o Imão do lado branco [esquerda], pressione 'TRIGGER' do controle esquerdo.", 1, false));

        yield return new WaitUntil(() => maoD.estado == S_IK.estadoMao.livre && maoE.estado == S_IK.estadoMao.livre);
        Controles[0].SetActive(false);

        yield return StartCoroutine(Escreve("Isso ai! Lembre sempre:\r\n- Ambas as mãos podem mover qualquer Imão, mas cada Imão tem seu próprio 'TRIGGER'.\r\n- Enquanto conectado, um Imão não pode ser segurado. Desconecte antes.", 2, true));
        yield return StartCoroutine(Escreve("Agora que sabemos sobre o equilíbrio e os Imãos, vamos a última parte do MAOÁ:\r\nAs pernas, que controlam a postura.", 2, true));

        Sparte = false;
        StartCoroutine(TerceiraParte());
    }

    IEnumerator TerceiraParte()
    {
        Tparte = true;

        yield return StartCoroutine(Escreve("Existem duas posturas principais:\r\n- Fechada, quando suas pernas estão juntas.\r\n- Aberta, quando elas estão afastadas. [podem ser de esquerda ou direita].", 2, true));

        foreach (GameObject rig in RIGperna) rig.SetActive(true);
        Spostura.enabled = true;
        Controles[0].SetActive(true);

        yield return StartCoroutine(Escreve("Você está na fechada. Vamos trocar para a aberta de esquerda.\r\nIgual aos imãos, aproxime suas mãos, clique e mantenha o 'GRAB' do seu controle pressionado e bote a direita na frente e a esquerda a trás.", 1, false));

        yield return new WaitUntil(() => jogador.posPerna.Contains("Ae"));

        yield return StartCoroutine(Escreve("Isso! Agora sua postura é Aberta Esquerda, porque sua perna esquerda que está na frente! \r\nVamos treinar mais uma vez. Feche sua postura e então abra novamente, mas agora com a direita na frente.", 1, false));

        yield return new WaitUntil(() => jogador.posPerna.Contains("F"));
        yield return new WaitUntil(() => jogador.posPerna.Contains("Ad"));
        Controles[0].SetActive(false);

        yield return StartCoroutine(Escreve("Perfeito!!!\r\nVocê ja sabe sobre todas as bases do judô.\r\nQue tal avançarmos um pouco e botarmos em prática realizando um golpe?", 2, true));

        Tparte = false;
        StartCoroutine(QuartaParte());
    }

    IEnumerator QuartaParte() // fazendo golpes
    {
        Qparte = true;

        yield return StartCoroutine(Escreve("Para realizar um golpe é preciso completar os requisítos dele. Esses são equilíbrio, pegada e postura.\r\nQuando concluídos, o golpe será usado automaticamente!", 2, true));
        yield return StartCoroutine(Escreve("Vamos testar fazer um. \r\nPra isso, vou lhe mostrar uma imagem de um golpe e vou te ensinar a ler ela.", 2, true));

        imagem.SetActive(true);

        yield return StartCoroutine(Escreve("No lado colorido fica seu MAOA\r\n- O disco mostra em verde o equilíbrio que você deve ir.\r\n- As pernas mostram a postura que você deve ir. Caso seja aberta, a perna mais levantada é a que deve ir para frente.", 2, true));
        yield return StartCoroutine(Escreve("No lado oco fica o advserário\r\n- O disco mostra em vermelho o equilíbrio de fuga do oponente. (vamos ver em breve)\r\n- O corpo mostra em amarelo alguns conectores, nos quais suas Imãos devem conectar.", 1, false));

        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Q" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Cd" &&
        jogador.posPerna.Contains("Ad") && jogador.dirEqui == "c");

        imagem.SetActive(false);

        yield return StartCoroutine(Escreve("Isso ai! você realizou um golpe!\r\nNo momento não aconteceu nada, mas é porque estavamos apenas testando.\r\nAgora sim vamos testar um de verdade e aprender sobre projeção.", 2, true));

        Qparte = false;
        StartCoroutine(QuintaParte());
    }

    IEnumerator QuintaParte()
    {
        QIparte = true;

        yield return StartCoroutine(Escreve("Realize mais um golpe para entrar na zona de projeção e aprendermos mais sobre.", 1, false));

        imagem2.SetActive(true);

        yield return new WaitUntil(() => jogador.dirEqui == "c" && jogador.posPerna.Contains("F") &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Od" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Cd");

        S_verificaGolpe.esperaTime = true;
        StartCoroutine(S_verificaGolpe.Vgolpe.TimeSlow(S_verificaGolpe.Vgolpe.golpes[4], jogador, jogador.adversario));

        XRGrabInteractable pdesXR = SVgolpe.pDes.GetComponent<XRGrabInteractable>();

        pdesXR.trackPosition = false;
        pdesXR.trackRotation = false;
        pdesXR.enabled = false;

        imagem2.SetActive(false);

        yield return StartCoroutine(Escreve("Buuuuuummm~ Legal né? \r\nEntramos dentro da zona de projeção!\r\nAqui dentro tudo fica super lento e duas coisas importântes acontecem: Um jogador tenta realizar uma projeção e o outro fugir dela.", 2, true));
        yield return StartCoroutine(Escreve("Vamos primeiro falar do jogador atacante, que foi o que conseguiu realizar o golpe. No caso, você.\r\nEntre as suas imãos se criou um orbe e uma grande seta brilhante. Seu objetivo é levar o orbe até o fim da seta.", 2, true));

        Controles[0].SetActive(true);

        yield return StartCoroutine(Escreve("Para fazer isso, aproxime uma de suas mãos dele, clique e mantenha o 'GRAB' do controle pressionado para segurar ele.\r\nCuide! O orbe não pode sair da seta. Aqui ele apenas recomeça, mas em uma luta real ele cancela a zona de projeção.", 1, false));

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

        Controles[0].SetActive(false);
        yield return StartCoroutine(Escreve("Vush! e lá se foi o adversário voando pelos ares!", 8, true));

        yield return new WaitUntil(() => !S_verificaGolpe.derrotou);

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

        yield return StartCoroutine(Escreve("Agora faremos o oposto.\r\nVocê será atingido por um golpe e irá realizar uma fuga.", 1, false));

        Sbot_jogador.dificuldade = 5;
        Sbot_jogador.naoMover = false;

        S_verificaGolpe.esperaTime = true;

        yield return new WaitUntil(() => S_verificaGolpe.timeSlow == true);

        yield return StartCoroutine(Escreve("Você foi atingido por um golpe.\r\nQuando isso acontecer, seu disco de equilíbrio ficará com um dos paineis em veremlho. Esse é o equilíbrio de fuga. [lembra que vimos por cima quando estavamos fazendo um golpe?]", 2, true));
        yield return StartCoroutine(Escreve("Para você fugir do golpe, mova seu equilíbrio para essa direção antes que o oponente leve o orbe de projeção até o fim da seta dele.", 1, false));

        Sequilibrio.enabled = true;
        yield return new WaitUntil(() => jogador.dirEqui == adversario.golpe.IdirEqui);

        Sbot_jogador.naoMover = true;
        S_verificaGolpe.esperaTime = false;

        yield return StartCoroutine(Escreve("Isso ai! Você se defendeu com sucesso. Quando fizer isso, seu oponente ficará desestabilizado e soltará tudo.\r\nFique esperto, seu oponente também pode fazer isso com na sua vez de atacar.", 2, true));
        yield return StartCoroutine(Escreve("Agora você ja sabe sobre quase tudo! Só falta uma coisinha: energia!\r\nAo redor do seus disco de equilíbrio, você possui uma barra de energia verdinha, indo de 100% até 0%.", 2, true));

        SEparte = false;
        STparte = true;
        StartCoroutine(SetimaParte());
    }

    IEnumerator SetimaParte()
    {
        Senergia.energia = 100f;

        yield return StartCoroutine(Escreve("Ela não pode ser recuperada por ações e desce ao longo do tempo.\r\nQuando chegar a 0, o seu MAOÁ irá parar totalmente de funcionar por um tempo enquanto ela se regenera, deixando você vulnerável.", 2, true));

        yield return StartCoroutine(Escreve("Perder 25% de energia [4/4]", 2, true));
        Senergia.energia -= 25f;
        yield return StartCoroutine(Escreve("Perder 25% de energia [3/4]", 2, true));
        Senergia.energia -= 25f;
        yield return StartCoroutine(Escreve("Perder 25% de energia [2/4]", 2, true));
        Senergia.energia -= 25f;
        yield return StartCoroutine(Escreve("Perder 25% de energia [1/4]", 2, true));
        Senergia.energia -= 25f;
        yield return StartCoroutine(Escreve("Perder 25% de energia [0/4]", 1, false));

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(Escreve("Você dominou tudo! Sabia que você seria uma mega estrela!!! Agora vá fazer uma batalha teste!", 2, true));

        STparte = false;

        Sbot_jogador.dificuldade = 1;
        FindAnyObjectByType<S_onClique>().TrocaUI(0);
        controleCena.ColocarMAOA(false);
        balaoFala.SetActive(false);
        botao.SetActive(false);
        Sclique.PassarFase();
        golpesImagens.SetActive(true);
        SaveManager.Salvar();
        enabled = false;
    }

    // SEGUNDA METADE DO TUTORIAL

    public IEnumerator SprimeiraParte() // ensia movimento pelo mapa
    {
        yield return new WaitForEndOfFrame();

        if (S_controleCena.modo != S_controleCena.ModoJogo.Tutorial)
        {
            balaoFala.SetActive(false);
            enabled = false;
        }

        emTutorial = true;

        balaoFala.SetActive(true);
        botao.SetActive(false);
        imagem.SetActive(false);
        Sbot_jogador.dificuldade = 1;

        S_pontos.Spontos.pontos1 = 0;
        S_pontos.Spontos.pontos2 = 0;

        Senergia.energia = 99999999f;
        tutorial1 = false;
        foreach (GameObject rig in RIGimao) rig.SetActive(true);

        yield return StartCoroutine(Escreve("Agora que você ja dominou o básico, vamos aprender mais a fundo. Mas não se preocupe! será rápido e fácil.", 2, true));
        yield return StartCoroutine(Escreve("Enquanto estiver com as pernas abertas, manter seu equilíbrio em alguma direção fora a do centro faz você e seu adversário se moverem pelo mapa.\r\nPara frente e para trás [frente e trás] ou rotacionar [esquerda e direita].", 2, true));
        yield return StartCoroutine(Escreve("Fique atento, as forças dos jogadores funcionam juntas. É possível lutar contra ou empurrar a favor um do outro.\r\nAdemais, tocar nas bordas causa derrota na hora! cuide!\r\nTente se mover um pouco.", 10, true));

        StartCoroutine(SsegundaParte());
    }

    IEnumerator SsegundaParte() // ensia gastos de energia
    {
        Senergia.energia = Senergia.energiaMax;
        yield return StartCoroutine(Escreve("Durante suas partidas, você percebeu que sua energia desce de forma bem lenta, mas é porque nós desativamos os aparatos que a utilizavam.", 2, true));
        yield return StartCoroutine(Escreve("4 coisas reduzem a energia:\r\n- Manter sua postura abera.\r\n- Manter seus braços longe do ombro.\r\n- Trocar seu equilíbrio.\r\n- Falhar em realizar um golpe.", 2, true));
        yield return StartCoroutine(Escreve("Inclusive, as linhas de energia nos braços e pernas é para mostrar quando eles estão consumindo ou não mais energia.", 2, true));

        StartCoroutine(SterceiraParte());
    }

    IEnumerator SterceiraParte() // ensia vulnerável
    {
        yield return StartCoroutine(Escreve("Vamos aprender sobre a última coisa: Fraqueza.\r\nNo judô, diferente de como treinamos com agora a pouco, você não consegue utilizar golpes em qualquer momento. Você precisa achar o 'Momentum'.", 2, true));
        yield return StartCoroutine(Escreve("Há um escudo no seu peito. \r\nSegurar seus braços, pernas ou enquanto trocando de equilíbrio retira seu escudo por um tempo.\r\nEnquanto sem escudo você pode ser atingido por golpes. Enquanto com, você fica protegido.", 2, true));
        yield return StartCoroutine(Escreve("Utilize disso para se proteger de ataques e planejar seus movimentos.", 2, true));

        S_controleCena.modo = S_controleCena.ModoJogo.PvE;
        emTutorial = false;
        FindAnyObjectByType<S_onClique>().TrocaUI(0);
        controleCena.ColocarMAOA(false);
        botao.SetActive(false);
        balaoFala.SetActive(false);
        Sclique.PassarFase();
        SaveManager.Salvar();
        enabled = false;
    }

    IEnumerator Escreve(string fala, int t, bool next) //yield return StartCoroutine(Escreve("", t));
    {
        quadroDfala.text = fala;
        yield return null;
        if (t > 0) yield return new WaitForSecondsRealtime(t); //trocar pra t

        if (next)
        {
            botao.SetActive(true);
            yield return new WaitUntil(() => passa);
            passa = false;
            botao.SetActive(false);
        }
    }
}
