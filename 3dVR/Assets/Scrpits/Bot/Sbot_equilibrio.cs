using System.Collections;
using UnityEngine;

public class Sbot_equilibrio : S_Equilibrio
{
    float t = 0f;
    bool movendo = false;

    private void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
        energia = GetComponentInParent<S_energia>();
        inicialPos = pCentral.transform.position;
        TrocaEquilibrio("c", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (pCentral == null || energia.rodandoSS) return;

        if (!movendo) t += Time.deltaTime;
        if (t >= 5f)
        {
            t = 0f;

            int objeto = Random.Range(0, blocos.Count);

            Vector3 alvo = blocos[objeto].transform.position;
            
            StartCoroutine(mover(alvo));
        }

        if (Vector3.Distance(pCentral.transform.position, inicialPos) <= (dist * 0.52f) && direcaoEquilibrio != "c") TrocaEquilibrio("c", 0);
        else if (Vector3.Distance(pCentral.transform.position, inicialPos) >= (dist * 0.6f))
        {
            Vector3 dir = (pCentral.transform.position - inicialPos).normalized;
            if (dir.x > XYdir && direcaoEquilibrio != "d") TrocaEquilibrio("d", 2);
            else if (dir.x < -XYdir && direcaoEquilibrio != "e") TrocaEquilibrio("e", 4);
            else if (dir.z > XYdir && direcaoEquilibrio != "f") TrocaEquilibrio("f", 3);
            else if (dir.z < -XYdir && direcaoEquilibrio != "t") TrocaEquilibrio("t", 1);
        }
    }

    IEnumerator mover(Vector3 final)
    {
        movendo = true;
        while (Vector3.Distance(pCentral.transform.position, final) > 0.01f)
        {
            pCentral.transform.position = Vector3.Lerp
             (pCentral.transform.position, new Vector3(final.x, pCentral.transform.position.y, final.z), Time.deltaTime * 10f);
            yield return null;
        }
        pCentral.transform.position = final;
        movendo = false;
    }
}
