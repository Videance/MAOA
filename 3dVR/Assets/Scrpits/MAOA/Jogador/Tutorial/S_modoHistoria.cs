using System.Collections;
using UnityEngine;

public class S_modoHistoria : MonoBehaviour
{
    private void Awake()
    {
        if (S_controleCena.modo != S_controleCena.ModoJogo.Historia) enabled = false;
    }

    public IEnumerator EnsinarGolpes(C_golpes[] lista)
    {
        yield return null;
    }
}
