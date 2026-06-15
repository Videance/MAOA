using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_controleCena : MonoBehaviour
{
    //cria um evento cm isso

    public static ModoJogo modo = ModoJogo.Tutorial;

    public enum ModoJogo
    {
        Historia,
        PvE,
        Tutorial
    }

    public static IEnumerator RenovaCena(string nome)
    {
        yield return SceneManager.UnloadSceneAsync(nome);
        yield return SceneManager.LoadSceneAsync(nome, LoadSceneMode.Additive);
    }
}
