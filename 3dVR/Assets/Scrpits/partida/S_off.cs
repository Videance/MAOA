using UnityEngine;

public class S_off : MonoBehaviour
{
    private void Update()
    {
        if (S_verificaGolpe.derrotou) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
}
