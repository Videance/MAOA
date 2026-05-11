using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class S_controleTutorial : MonoBehaviour
{
    S_jogador jogador;
    public TextMesh quadroDfala;
    public S_verificaGolpe SVgolpe;

    [Header("PRIMEIRA PARTE")]
    public bool Pparte = false;
    public GameObject[] discoEquilibrio;
    public S_Equilibrio Sequilibrio;

    [Header("SEGUNDA PARTE")]
    public bool Sparte = false;
    public GameObject[] RIGimao;
    S_IK maoD;
    S_IK maoE;
    public GameObject bot;

    [Header("TERCEIRA PARTE")]
    public bool Tparte = false;
    public bool[] tocou = new bool[2];
    public GameObject[] RIGperna;
    public GameObject pngPostura;
    public S_Postura Spostura;

    [Header("QUARTA PARTE")]
    public bool Qparte = false;
    Sbot_jogador adversario;

    [Header("QUINTA PARTE")]
    public bool QIparte = false;

    [Header("SEXTA PARTE")]
    public bool SEparte = false;
    public S_energia Senergia;
    public GameObject[] pngEnergia;

    void Awake()
    {
        jogador = GetComponent<S_jogador>();
        adversario = FindAnyObjectByType<Sbot_jogador>();
        maoD = RIGimao[0].GetComponent<S_IK>();
        maoE = RIGimao[1].GetComponent<S_IK>();
    }

    private void Start()
    {
        StartCoroutine(PrimeiraParte());
    }

    private void Update()
    {
        if (Senergia.energia <= 100 && !SEparte) Senergia.energia = 999999;
    }

    IEnumerator PrimeiraParte()
    {
        Pparte = true;

        yield return StartCoroutine(Escreve("Este é o seu MAOÁ. Estamos vendo ele através de imagens de um satélite especial equipado neste robô.", 5));
        yield return StartCoroutine(Escreve("Ele é composto de 3 partes principais: Cabeça, Imãos e Pés. Cada uma interligada a uma parte fundamental do judô!", 5));

        foreach (GameObject disco in discoEquilibrio) disco.SetActive(true);
        Sequilibrio.enabled = true;

        yield return StartCoroutine(Escreve("Vamos começar pela cabeça, que cuida do Equilíbrio. Em baixo do seu MAOÁ tem um disco dividido em 5 partes, cada uma simbolizando uma direção.", 5));
        yield return StartCoroutine(Escreve("Quando você move seu Oculos VR em alguma direção, o círculo laranja se moverá junto com ele.", 5));
        yield return StartCoroutine(Escreve("E quando ele estiver em cima de uma das partes, ela ficará brilhante, definindo o seu equilíbrio naquela direção", 5));

        yield return StartCoroutine(Escreve("Vamos aprender um pouco. Tente colocar seu equilíbrio para frente, movendo seu Oculos VR para frente.", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "f");
        yield return StartCoroutine(Escreve("Muito bem! Agora para trás!", 0));
        yield return new WaitUntil(() => jogador.dirEqui == "t");
        yield return StartCoroutine(Escreve("Muito bem! Agora para direita!", 0));
        yield return new WaitUntil(() => jogador.dirEqui == "d");
        yield return StartCoroutine(Escreve("Muito bem! Agora para esquerda!", 0));
        yield return new WaitUntil(() => jogador.dirEqui == "e");
        yield return StartCoroutine(Escreve("Muito bem! Agora para o centro!", 0));
        yield return new WaitUntil(() => jogador.dirEqui == "c");

        yield return StartCoroutine(Escreve("Isso ai!!! Você pegou o jeito. Agora vamos explorar o corpo do MAOÁ, começando pelas Imãos.", 5));

        Pparte = true;
        StartCoroutine(SegundaParte());
    }

    IEnumerator SegundaParte()
    {
        Sparte = true;

        yield return StartCoroutine(Escreve("Como disse antes, na sua frente você está vendo o seu MAOÁ por uma mega tela. Porém, nossa super tecnolgia permite atraversarmos ela!", 5));
        yield return StartCoroutine(Escreve("coloque suas mãos para frente, através da tela, e toque com uma no Imão direito e a outra no Imão esquerdo so seu MAOÁ.", 0));
        foreach (GameObject rig in RIGimao) rig.SetActive(true);

        tocou[0] = false;
        tocou[1] = false;
        yield return new WaitUntil(() => tocou[1] == true && tocou[0] == true);

        yield return StartCoroutine(Escreve("As imãos do MAOÁ podem ser seguradas e movimentadas", 5));
        yield return StartCoroutine(Escreve("Mantenha o 'GRAB' do seu controle pressionado enquanto próximo a uma Imão para fazer ela seguir sua mão. Solte o 'GRAB' para parar.", 10));
        yield return StartCoroutine(Escreve("Ótimo! agora, que você ja sabe como mover as Imãos do seu MAOÁ! Agora vamos aprender pra que isso serve!", 5));

        GameObject boti = Instantiate(bot);

        yield return StartCoroutine(Escreve("As imãos cuidam da pegada do judô. O seu adversário, igual a você, possui pontos de conexão em seu corpo localizado nas juntas do MAOÁ", 5));
        yield return StartCoroutine(Escreve("Esses pontos permitem que você conecte as Imãos do seu MAOÁ nelas, mudando sua pegada.", 5));
        yield return StartCoroutine(Escreve("Com sua mão direita, leve o imão direito até o conector do ombo do adversário e, enquanto segurnando ele, pressione 'TRIGGER'", 0));
        
        yield return new WaitUntil(() => maoD.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe");
        
        yield return StartCoroutine(Escreve("Ótimo, agora vamos fazer o mesmo com a outra imão. Segure ela, leve até o ponto, e enquanto segurando ela pressione 'TRIGGER'", 0));
        
        yield return new WaitUntil(() => maoE.conectado != null && 
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");

        yield return StartCoroutine(Escreve("Isso ai! Como voce pode ver, enquanto conectada, o imão e a sua mão do lado correspondente irão ficar brilhosas", 5));
        yield return StartCoroutine(Escreve("Além disso, um imão conectado não pode ser segurado, é preciso primeiro desconectar ele.", 5));
        yield return StartCoroutine(Escreve("Desconecte a imão direito clicando no 'TRIGGER' da mão direita", 5));
        
        yield return new WaitUntil(() => maoD.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Agora desconecte o imao esquerdo clicando no 'TRIGGER' da mão esquerda", 5));
        
        yield return new WaitUntil(() => maoE.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Perfeito! agora vamos tentar mais uma vez, mas segurando em pontos diferentes", 5));
        yield return StartCoroutine(Escreve("Coloque o imao esquerdo no quadril e o direito no cotovelo.", 5));
        
        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Q");

        yield return StartCoroutine(Escreve("Agora desconecte ambas", 5));
       
        yield return new WaitUntil(() => maoE.estado == S_IK.estadoMao.livre && maoD.estado == S_IK.estadoMao.livre);

        yield return StartCoroutine(Escreve("Isso ai! Pegou o jeito. Agora que sabemos sobre o equilíbrio e as imãos, vamos a última parte do MAOÁ, as pernas", 5));

        Sparte = false;
        StartCoroutine(TerceiraParte());
    }

    IEnumerator TerceiraParte()
    {
        Tparte = true;

        foreach (GameObject rig in RIGperna) rig.SetActive(true);

        yield return StartCoroutine(Escreve("Semelhante as imãos, as pernas também podem ser seguradas com o 'GRIP' e movimentadas. Tente!", 5));
        yield return StartCoroutine(Escreve("Elas cuidam da sua postura! Ponha as pernas na posição inicial para começarmos.", 2));

        Spostura.enabled = true;
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => jogador.pernaAberta == false &&
        RIGperna[0].GetComponent<S_dis_pe>().segurando == false &&
        RIGperna[1].GetComponent<S_dis_pe>().segurando == false);
        for (int i = 0; i < RIGperna.Length; i++) StartCoroutine(RIGperna[i].GetComponent<S_dis_pe>().Mover(false));
        yield return new WaitUntil(() => RIGperna[0].GetComponent<S_dis_pe>().movendo == false &&
        RIGperna[1].GetComponent<S_dis_pe>().movendo == false);

        pngPostura.SetActive(true);

        yield return StartCoroutine(Escreve("Existem duas posturas: Fechada, quando suas pernas estão juntas, e Aberta, quando elas estão afastadas.", 5));
        yield return StartCoroutine(Escreve("No momento você esta na fechada. Vamos trocar para a aberta. Segure cada perna e mova uma para frente e outra para trás.", 0));

        yield return new WaitUntil(() => jogador.pernaAberta == true);

        yield return StartCoroutine(Escreve("Isso! Agora sua postura é Aberta!", 5));
        yield return StartCoroutine(Escreve("Porém fique atento, manter a postura aberta exige muito do seu MAOÁ, então ele tentará fechar ela constântemente.", 5));
        yield return StartCoroutine(Escreve("Espere ela fechar ou traga elas de volta e então abra novamente.", 0));

        yield return new WaitUntil(() => jogador.pernaAberta == false);
        yield return new WaitUntil(() => jogador.pernaAberta == true);

        yield return StartCoroutine(Escreve("Perfeito!!! Você ja sabe sobre todas as bases do judo. Que tal avançarmos um pouco e botarmos em prática realizando um golpe?", 5));

        Tparte = false;
        StartCoroutine(QuartaParte());
    }

    IEnumerator QuartaParte()
    {
        Qparte = true;

        yield return StartCoroutine(Escreve("Para realizar um golpe, temos que juntar tudo que aprendemos até agora. Equilíbrio, pegadas de ambas as imãos e a postura!", 5));
        yield return StartCoroutine(Escreve("Cada um possui requisitos para ser ativado, mas não se preocupe! Quando concluídos, o golpe será usado automaticamente!", 5));
        yield return StartCoroutine(Escreve("vamos testar com o golpe XXXXX. Pra isso, conecte a Imão esquerda no Ombro esquerdo e a Imão direita do Cotovelo esquerdo", 1));

        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe");
        yield return StartCoroutine(Escreve("Isso! fizemos as imãos. Agora troque sua postura para Aberta!", 1));
        yield return new WaitUntil(() => jogador.pernaAberta == true);
        yield return StartCoroutine(Escreve("Para finalizar, vamos ativar esse golpe colocando no equilíbrio correto! Ponha seu equilíbrio para a esquerda.", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "d");

        yield return new WaitUntil(() => jogador.dirEqui == "d" && jogador.pernaAberta == true &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe");

        yield return StartCoroutine(Escreve("Você viu esse efeito que saiu? Isso significa que você acertou um golpe! parabéns!", 5));
        yield return StartCoroutine(Escreve("Vamos tentar agora o golpe XXXXX, porém dessa vez iremos trocar a ordem do que iremos fazer", 5));
        yield return StartCoroutine(Escreve("conecte a Imão esquerda no Ombro direito e a Imão direita do Pescoço", 0));

        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "P" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");
        yield return StartCoroutine(Escreve("Ponha e mantenha seu equilíbrio para a trás.", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "t");
        yield return StartCoroutine(Escreve("Isso! para finalizar dessa vez troque sua postura para Aberta!", 1));
        yield return new WaitUntil(() => jogador.pernaAberta == true);

        yield return new WaitUntil(() => jogador.dirEqui == "t" && jogador.pernaAberta == true &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "P" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Od");

        yield return StartCoroutine(Escreve("Outro golpe concluído com sucesso! Desta vez finalizado com sua postura!", 5));
        yield return StartCoroutine(Escreve("Vamos tentar agora mais um golpe, o XXXXX, e iremos trocar a ordem mais uma vez a ordem", 5));
        yield return StartCoroutine(Escreve("Ponha e mantenha seu equilíbrio para a frente.", 1));

        yield return new WaitUntil(() => jogador.dirEqui == "f");
        yield return StartCoroutine(Escreve("troque sua postura para Aberta!", 1));
        yield return new WaitUntil(() => jogador.pernaAberta == true);
        yield return StartCoroutine(Escreve("E para finalizar dessa vez, conecte a Imão esquerda no Cotovelo direito e a Imão direita do Cotovelo esquerdo", 0));
        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Cd" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce");

        yield return new WaitUntil(() => jogador.dirEqui == "f" && jogador.pernaAberta == true &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Cd" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Ce");

        yield return StartCoroutine(Escreve("UHUL!!! Você concluiu 3 golpes perfeitamente!", 5));
        yield return StartCoroutine(Escreve("Como você percebeu, cada golpe é unico e existem váaarios deles que você irá aprender com o tempo!", 5));
        yield return StartCoroutine(Escreve("Agora que você aprendeu sobre as 3 partes essenciais do seu MAOÁ e como ativar golpes, vamos avançar para a segunda parte!", 5));

        Qparte = false;
        StartCoroutine(QuintaParte());
    }

    IEnumerator QuintaParte()
    {
        QIparte = true;

        yield return StartCoroutine(Escreve("Você deve ter percebido que ao realizar um golpe apenas um pequeno efeito aconteceu, mas isso é porque estavamos apenas testando.", 5));
        yield return StartCoroutine(Escreve("Durante uma verdadeira luta de judô você deve acertar a posição do corpo e depois realizar uma projeção!", 5));
        yield return StartCoroutine(Escreve("E agora que você ja sabe da posição, vamos aprender a projeção e como se defender de uma", 5));
        yield return StartCoroutine(Escreve("Realize mais um golpe, o XXXXX, para ativarmos uma projeção", 5));
       
        yield return StartCoroutine(Escreve("Conecte a Imão esquerda no Ombro esquerdo e a Imão direita do Ombro esquerdo", 1));
        yield return new WaitUntil(() => maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe");
        yield return StartCoroutine(Escreve("Troque sua postura para Aberta!", 1));
        yield return new WaitUntil(() => jogador.pernaAberta == true);
        yield return StartCoroutine(Escreve("Ponha seu equilíbrio no centro.", 1));
        yield return new WaitUntil(() => jogador.dirEqui == "c");

        yield return new WaitUntil(() => jogador.dirEqui == "c" && jogador.pernaAberta == true &&
        maoD.conectado != null && maoE.conectado != null &&
        maoD.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe" &&
        maoE.conectado.GetComponent<S_Conector>().localDoCorpo == "Oe");

        S_verificaGolpe.emTutorial = true;
        StartCoroutine(S_verificaGolpe.Vgolpe.TimeSlow(null, jogador, jogador.adversario));
        yield return StartCoroutine(Escreve("Buuuuuummm~ Legal né? Entramos dentro da zona de projeção!", 5));
        yield return StartCoroutine(Escreve("Aqui dentro tudo fica super lento e duas coisas importântes acontecem: Um jogador tenta realizar uma projeção e o outro fugir dela", 5));
        yield return StartCoroutine(Escreve("Vamos primeiro falar do jogador realizando a projeção, que foi o que conseguiu realizar o golpe. No caso, você.", 5));
        yield return StartCoroutine(Escreve("Entre as suas imãos se criou um orbe e uma grande seta brilhante. Seu objetivo como atacante é levar o orbe até o fim da seta.", 5));
        yield return StartCoroutine(Escreve("Para fazer isso, aproxime uma de suas mãos dele e segure seu 'GRAB', igual você fez com as partes do seu MAOÁ.", 5));
        yield return StartCoroutine(Escreve("Mas fique atento! O orbe NÃO PODE SAIR DA SETA, caso contrário ele seu MAOÁ perderá o impulso e sairá da zona de projeção.", 5));

        while (S_verificaGolpe.timeSlow)
        {
            if (S_verificaGolpe.Spde.noCaminho == false)
            {
                Destroy(SVgolpe.pDes);
                SVgolpe.pDes = null;
                SVgolpe.CriarPonto(1, jogador, jogador.adversario);
            }

            if (S_verificaGolpe.Spde.tocouClimax == true) yield break;
            yield return null;
        }

        S_verificaGolpe.emTutorial = false;

        yield return StartCoroutine(Escreve("Vush! e lá se foi o adversário voando pelos ares!", 5));
        yield return StartCoroutine(Escreve("Viu? É simples! você conseguiu fazer uma projeção de sucesso! mas lembre-se que em uma situação real, errar lhe tira da zona.", 5));
        yield return StartCoroutine(Escreve("Agora faremos o oposto. Você será atingido por um golpe e irá realizar uma fuga! Coloque seu equilíbrio no centro para começar", 5));

        yield return new WaitUntil(() => jogador.dirEqui == "c");

        adversario.enabled = true;
        adversario.golpe = S_verificaGolpe.Vgolpe.golpes[4];
        adversario.dificuldade = 4;

        S_verificaGolpe.emTutorial = true;
        yield return new WaitUntil(() => S_verificaGolpe.timeSlow == true);

        yield return StartCoroutine(Escreve("Bom, você foi atingido por um golpe. Quando isso acontecer, seu disco de equilíbrio, aqule em baixo de você, ficará com um dos paineis brilhando.", 5));
        yield return StartCoroutine(Escreve("E para você fugir do golpe, você deve mover seu equilíbrio para essa direção antes que o oponente leve o orbe de projeção até o fim da seta dele.", 5));

        yield return new WaitUntil(() => jogador.dirEqui == adversario.golpe.IdirEqui);

        yield return StartCoroutine(Escreve("Isso ai! Você se defendeu do golpe trocando seu equilíbrio antes do tempo! Quando fizer isso, seu oponente ficará desestabilizado e soltará tudo.", 5));
        yield return StartCoroutine(Escreve("Mas lembre-se, seu oponente também pode fazer isso! Então quando você for o atacante, mova seu orbe até a ponta da seta o quanto antes.", 5));
        yield return StartCoroutine(Escreve("Agora você ja sabe sobre quase tudo! só falta uma coisinha: energia!", 5));

        QIparte = false;
        StartCoroutine(SextaParte());
    }

    IEnumerator SextaParte()
    {
        SEparte = true;

        yield return StartCoroutine(Escreve("Igual a outras máquinas, o seu MAOÁ também precisa de energia para funcionar", 5));

        Senergia.energia = 100f;
        foreach (GameObject imagem in pngEnergia) imagem.SetActive(true);

        yield return StartCoroutine(Escreve("Em cima da suas mãos há um medidor que diz o quanto de energia você possui, indo de 100% até 0%", 5));
        yield return StartCoroutine(Escreve("A energia reduz de algumas formas: Mantendo a postura aberta, mantendo suas imãos conectadas, trocando de equilíbrio e falhando em projetar um golpe no adversário.", 5));
        yield return StartCoroutine(Escreve("Ela não pode ser recuperada por ações, e quando chegar a 0 seu MAOÁ irá parar de funcionar por um tempo enquanto ela se regenera.", 5));
        yield return StartCoroutine(Escreve("Agora, vamos realizar seu último teste, uma batalha final de verdade (agora com energia)!", 5));

        Senergia.energia = 100f;

        SEparte = false;
    }

    IEnumerator Escreve(string fala, int t) //yield return StartCoroutine(Escreve("", 5));
    {
        quadroDfala.text = fala;
        if (t > 0) yield return new WaitForSeconds(t);
    }
}
