using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_onClique : MonoBehaviour
{
    public GameObject[] UIs;

    public void TrocaUI(int id) //sempre chamado quando troca a UI pra fechar todas
    {
        if (UIs.Length == 0) return;

        for (int i = 0; i < UIs.Length; i++)
        {
            if (i == id) UIs[i].SetActive(true);
            else UIs[i].SetActive(false);
        }
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
        SceneManager.LoadScene("MAOA vdd", LoadSceneMode.Additive);
        S_controleCena.modo = S_controleCena.ModoJogo.Historia;
        TrocaUI(8);
    }

    // - - - - - - - - - - J O G O - - - - - - - - - - //
    public void SairPartida()
    {
        TrocaUI(0);
        SceneManager.UnloadSceneAsync("MAOA vdd");
    }

    public void PassaDialogo() { S_controleTutorial.passa = true; }
}
