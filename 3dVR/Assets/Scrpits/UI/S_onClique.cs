using System.Collections.Generic;
using UnityEngine;

public class S_onClique : MonoBehaviour
{
    public GameObject[] UIs;

    private void Awake()
    {
        UIs = GetComponentsInChildren<GameObject>(gameObject.CompareTag("ui"));
    }

    public void TrocaUI(int id) //semore chamado quando troca a UI pra fechar todas
    {
        if (UIs.Length == 0) return;

        for (int i = 0; i < UIs.Length; i++)
        {
            if (i == id) UIs[i].SetActive(true);
            else UIs[i].SetActive(false);
        }
    }

    // MENU
    public void Arena()
    {

    }

    // ARENA


    // PAUSE


    // LEADBOARD


    // SETTIGNS


    // QUIT
}
