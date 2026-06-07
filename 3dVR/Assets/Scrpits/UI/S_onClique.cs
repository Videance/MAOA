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
        SceneManager.LoadScene("MAOA vdd");
    }

    public void PlayMultiplayer()
    {
        
    }

    public void PlayHistory(int i)
    {
        
    }

    // - - - - - - - - - - J O G O - - - - - - - - - - //
    public void SairPartida()
    {
        SceneManager.LoadScene("Menu");
    }
}
