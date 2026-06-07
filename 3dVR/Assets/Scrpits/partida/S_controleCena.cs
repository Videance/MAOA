using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_controleCena : MonoBehaviour
{
    public static IEnumerator RenovaCena(string nome)
    {
        yield return SceneManager.UnloadSceneAsync(nome);
        yield return SceneManager.LoadSceneAsync(nome, LoadSceneMode.Additive);
    }
}
