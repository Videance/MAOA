using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_onClique : MonoBehaviour
{
    public GameObject[] UIs;
    S_verificaGolpe Svg;

    private void Awake()
    {
        Svg = S_verificaGolpe.Vgolpe;
    }

    public void TrocaUI(int id) //sempre chamado quando troca a UI pra fechar todas
    {
        if (UIs.Length == 0) return;

        for (int i = 0; i < UIs.Length; i++)
        {
            if (i == id) UIs[i].SetActive(true);
            else UIs[i].SetActive(false);
        }
    }

    public void EnsinarGolpes(int i)
    {

    }

    public void PlayBot()
    {
        SceneManager.LoadScene("MAOA vdd", LoadSceneMode.Additive);
        S_controleCena.modo = S_controleCena.ModoJogo.PvE;
        TrocaUI(8);
    }

    public void PlayMultiplayer()
    {
        
    }

    public void PlayHistory(int i)
    {
        if (i == 0) S_controleCena.modo = S_controleCena.ModoJogo.Tutorial;
        else if (i != 0)
        {
            S_controleCena.modo = S_controleCena.ModoJogo.Historia;

            int inicio = (i - 1) * 5;

            for (int j = 0; j < 5; j++)
                S_modoHistoria.listaGolpes[j] = Svg.golpes[inicio + j];
        }
        SceneManager.LoadScene("MAOA vdd", LoadSceneMode.Additive);
        TrocaUI(8);
    }


    IEnumerator ensinaGolpes()
    {
        //yield return new WaitUntil(() => Cena "MAOA vdd" reze);
        //espera o evengo

        yield return null;
    }
    // - - - - - - - - - - J O G O - - - - - - - - - - //
    public void SairPartida()
    {
        TrocaUI(0);
        SceneManager.UnloadSceneAsync("MAOA vdd");
    }

    public void PassaDialogo() { S_controleTutorial.passa = true; }
}
