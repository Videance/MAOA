using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_controleTutorial : MonoBehaviour
{
    S_jogador jogador;
    public TextMeshPro quadroDfala;
    public S_verificaGolpe SVgolpe;

    [Header("PRIMEIRA PARTE")]
    public static bool Pparte = true;
    public GameObject[] discoEquilibrio;
    public S_Equilibrio Sequilibrio;

    [Header("SEGUNDA PARTE")]
    public static bool Sparte = false;
    public GameObject[] RIGimao;
    S_IK maoD;
    S_IK maoE;
    public GameObject bot;

    [Header("TERCEIRA PARTE")]
    public static bool Tparte = false;
    public bool[] tocou;
    public GameObject[] RIGperna;
    public GameObject pngPostura;
    public S_Postura Spostura;

    [Header("QUARTA PARTE")]
    public static bool Qparte = false;

    [Header("QUINTA PARTE")]
    public static bool QIparte = false;
    public static bool metade1 = true;
    public Sbot_jogador adversario;
    public GameObject discoEquilibrioBOT;

    [Header("SEXTA PARTE")]
    public static bool SEparte = false;
    public S_energia Senergia;
    public GameObject[] pngEnergia;

    void Awake()
    {
        jogador = GetComponent<S_jogador>();
        maoD = RIGimao[0].gameObject.GetComponent<S_IK>();
        maoE = RIGimao[1].gameObject.GetComponent<S_IK>();
    }

    private void Start()
    {
        tocou = new bool[2] { false, false };

        foreach (GameObject rig in RIGimao) rig.SetActive(false);
        foreach (GameObject rig in RIGperna) rig.SetActive(false);

        Pparte = true;

        if (Pparte) StartCoroutine(PrimeiraParte());
        if (Sparte) StartCoroutine(SegundaParte());
        if (Tparte) StartCoroutine(TerceiraParte());
        if (Qparte) StartCoroutine(QuartaParte());
        if (QIparte) StartCoroutine(QuintaParte());
        if (SEparte) StartCoroutine(SextaParte());
    }

    private void Update()
    {
        if (Senergia.energia <= 100 && !SEparte) Senergia.energia = 999999;
    }

    IEnumerator PrimeiraParte()
    {
        yield return StartCoroutine(Escreve("Este é o seu MAOÁ. Estamos vendo ele através de imagens de um satélite especial equipado neste robô.", 7));
        yield return StartCoroutine(Escreve("Ele é composto de 3 partes principais: Cabeça, Imãos e Pés. Cada uma interligada a uma parte fundamental do judô!", 7));

        foreach (GameObject disco in discoEquilibrio) disco.SetActive(true);
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
        for (int i = 0; i < RIGperna.Length; i++) StartCoroutine(RIGperna[i].GetComponent<S_dis_pe>().Mover(false));
        yield return new WaitUntil(() => RIGperna[0].GetComponent<S_dis_pe>().movendo == false &&
        RIGperna[1].GetComponent<S_dis_pe>().movendo == false);

        pngPostura.SetActive(true);

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

        yield return StartCoroutine(Escreve("Você viu esse efeito que saiu? Isso significa que você acertou um golpe! parabéns!", 6));
        yield return StartCoroutine(Escreve("Vamos tentar agora o golpe XXXXX, porém dessa vez iremos trocar a ordem do que iremos fazer", 7));
        yield return StartCoroutine(Escreve("conecte a Imão esquerda no Pescoço e a Imão direita no ombro direito", 1));

        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "P");
        yield return StartCoroutine(Escreve("Ponha e mantenha seu equilíbrio para a trás.", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "t");
        yield return StartCoroutine(Escreve("Isso! para finalizar dessa vez troque sua postura para Aberta!", 1));
        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));

        yield return new WaitUntil(() => jogador.dirEqui == "t" && jogador.posPerna.Contains("A") &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "P");

        yield return StartCoroutine(Escreve("Outro golpe concluído com sucesso! Desta vez finalizado com sua postura!", 5));
        yield return StartCoroutine(Escreve("Vamos tentar agora mais um golpe, o XXXXX, e iremos trocar a ordem mais uma vez a ordem", 7));
        yield return StartCoroutine(Escreve("Ponha e mantenha seu equilíbrio para a frente.", 1));

        yield return new WaitUntil(() => jogador.dirEqui == "f");
        yield return StartCoroutine(Escreve("troque sua postura para Aberta!", 1));
        yield return new WaitUntil(() => jogador.posPerna.Contains("A"));
        yield return StartCoroutine(Escreve("E para finalizar dessa vez, conecte a Imão esquerda no Cotovelo esquerdo e a Imão direita do Cotovelo direito", 1));
        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Cd");

        yield return new WaitUntil(() => jogador.dirEqui == "f" && jogador.posPerna.Contains("A") &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Cd");

        yield return StartCoroutine(Escreve("UHUL!!! Você concluiu 3 golpes perfeitamente!", 5));
        yield return StartCoroutine(Escreve("Como você percebeu, cada golpe é unico e existem váaarios deles que você irá aprender com o tempo!", 7));
        yield return StartCoroutine(Escreve("Agora que você aprendeu sobre as 3 partes essenciais do seu MAOÁ e como ativar golpes, vamos avançar para a segunda parte!", 8));

        Qparte = false;
        StartCoroutine(QuintaParte());
    }

    IEnumerator QuintaParte()
    {
        QIparte = true;

        if (metade1)
        {
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

            S_verificaGolpe.emTutorial = true;
            StartCoroutine(S_verificaGolpe.Vgolpe.TimeSlow(S_verificaGolpe.Vgolpe.golpes[4], jogador, jogador.adversario));
            yield return StartCoroutine(Escreve("Buuuuuummm~ Legal né? Entramos dentro da zona de projeção!", 5));
            yield return StartCoroutine(Escreve("Aqui dentro tudo fica super lento e duas coisas importântes acontecem: Um jogador tenta realizar uma projeção e o outro fugir dela", 8));
            yield return StartCoroutine(Escreve("Vamos primeiro falar do jogador realizando a projeção, que foi o que conseguiu realizar o golpe. No caso, você.", 7));
            yield return StartCoroutine(Escreve("Entre as suas imãos se criou um orbe e uma grande seta brilhante. Seu objetivo como atacante é levar o orbe até o fim da seta.", 8));
            yield return StartCoroutine(Escreve("Para fazer isso, aproxime uma de suas mãos dele e segure seu 'GRAB', igual você fez com as partes do seu MAOÁ.", 7));
            yield return StartCoroutine(Escreve("Mas fique atento! O orbe NÃO PODE SAIR DA SETA, caso contrário ele seu MAOÁ perderá o impulso e sairá da zona de projeção.", 8));

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

            S_verificaGolpe.emTutorial = false;

            yield return null;
            yield return new WaitForEndOfFrame();

            S_verificaGolpe.emTutorial = true;

            yield return StartCoroutine(Escreve("Vush! e lá se foi o adversário voando pelos ares!", 5));
            yield return StartCoroutine(Escreve("Viu? É simples! você conseguiu fazer uma projeção de sucesso! mas lembre-se que em uma situação real, errar lhe tira da zona.", 8));

            metade1 = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield break;
        }
        else if (!metade1)
        {
            yield return null;
            yield return new WaitForEndOfFrame();

            Spostura.enabled = true;
            pngPostura.SetActive(true);
            discoEquilibrioBOT.SetActive(true);
            bot.SetActive(true);
            adversario.enabled = false;

            yield return StartCoroutine(Escreve("Agora faremos o oposto. Você será atingido por um golpe e irá realizar uma fuga!", 6));

            adversario.enabled = true;
            adversario.golpe = S_verificaGolpe.Vgolpe.golpes[4];
            Sbot_jogador.dificuldade = 4;

            yield return new WaitUntil(() => S_verificaGolpe.timeSlow == true);

            yield return StartCoroutine(Escreve("Bom, você foi atingido por um golpe. Quando isso acontecer, seu disco de equilíbrio, aqule em baixo de você, ficará com um dos paineis brilhando.", 8));
            yield return StartCoroutine(Escreve("E para você fugir do golpe, você deve mover seu equilíbrio para essa direção antes que o oponente leve o orbe de projeção até o fim da seta dele.", 8));

            foreach (GameObject disco in discoEquilibrio) disco.SetActive(true);
            Sequilibrio.enabled = true;
            yield return new WaitUntil(() => jogador.dirEqui == adversario.golpe.IdirEqui);

            adversario.enabled = false;

            yield return StartCoroutine(Escreve("Isso ai! Você se defendeu do golpe trocando seu equilíbrio antes do tempo! Quando fizer isso, seu oponente ficará desestabilizado e soltará tudo.", 8));
            yield return StartCoroutine(Escreve("Mas lembre-se, seu oponente também pode fazer isso! Então quando você for o atacante, mova seu orbe até a ponta da seta o quanto antes.", 8));
            yield return StartCoroutine(Escreve("Agora você ja sabe sobre quase tudo! só falta uma coisinha: energia!", 5));

            QIparte = false;
            SEparte = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            StartCoroutine(SextaParte());
        }
    }

    IEnumerator SextaParte()
    {
        foreach (GameObject disco in discoEquilibrio) disco.SetActive(true);
        Sequilibrio.enabled = true;
        foreach (GameObject rig in RIGimao) rig.SetActive(true);
        foreach (GameObject rig in RIGperna) rig.SetActive(true);
        Spostura.enabled = true;
        pngPostura.SetActive(true);
        discoEquilibrioBOT.SetActive(true);
        bot.SetActive(true);

        yield return StartCoroutine(Escreve("Igual a outras máquinas, o seu MAOÁ também precisa de energia para funcionar", 5));

        Senergia.energia = 100f;
        foreach (GameObject imagem in pngEnergia) imagem.SetActive(true);

        yield return StartCoroutine(Escreve("Em cima da suas mãos há um medidor que diz o quanto de energia você possui, indo de 100% até 0%", 6));
        yield return StartCoroutine(Escreve("A energia reduz de algumas formas: Mantendo a postura aberta, mantendo suas imãos conectadas, trocando de equilíbrio e falhando em projetar um golpe no adversário.", 9));
        yield return StartCoroutine(Escreve("Ela não pode ser recuperada por ações, e quando chegar a 0 seu MAOÁ irá parar de funcionar por um tempo enquanto ela se regenera.", 8));
        yield return StartCoroutine(Escreve("Agora, vamos realizar seu último teste, uma batalha final de verdade (agora com energia)!", 6));

        SEparte = false;
        S_verificaGolpe.emTutorial = false;

        Sbot_jogador.dificuldade = 1;
        SceneManager.LoadScene("MAOA vdd");
    }

    IEnumerator Escreve(string fala, int t) //yield return StartCoroutine(Escreve("", 5));
    {
        quadroDfala.text = fala;
        if (t > 0) yield return new WaitForSecondsRealtime(t);
    }
}
