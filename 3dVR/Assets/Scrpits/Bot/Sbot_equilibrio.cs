using System.Collections;
using UnityEngine;

public class Sbot_equilibrio : S_Equilibrio
{
    public bool movendo = false;
    float speed = 1f;

    private void Start()
    {
        jogador = GetComponentInParent<S_jogador>();
        energia = GetComponentInParent<S_energia>();
        inicialPos = pCentral.transform.position;
        TrocaEquilibrio("c", 0);
        if (((Sbot_jogador)jogador).dificuldade != 1) speed = speed + Mathf.Sqrt(((Sbot_jogador)jogador).dificuldade);
    }

    // Update is called once per frame
    void Update()
    {
        if (pCentral == null || energia.rodandoSS) return;

        if (Vector3.Distance(pCentral.transform.position, inicialPos) <= (dist * 0.52f) && direcaoEquilibrio != "c") TrocaEquilibrio("c", 0);
        else if (Vector3.Distance(pCentral.transform.position, inicialPos) >= (dist * 0.6f))
        {
            Vector3 dir = (pCentral.transform.position - inicialPos).normalized;
            if (dir.x > XYdir && direcaoEquilibrio != "d") TrocaEquilibrio("e", 4);
            else if (dir.x < -XYdir && direcaoEquilibrio != "e") TrocaEquilibrio("d", 2);
            else if (dir.z > XYdir && direcaoEquilibrio != "f") TrocaEquilibrio("t", 1);
            else if (dir.z < -XYdir && direcaoEquilibrio != "t") TrocaEquilibrio("f", 3);
        }
    }

    void TrocaEquilibrio(string letra, int index)
    {
        if (jogador.dirEqui == letra) return;
        direcaoEquilibrio = letra;
        jogador.dirEqui = letra;
        if (primeira) primeira = false;
        else energia.energia -= 5;
        energia.energia = Mathf.Clamp(energia.energia, 0, energia.energiaMax);
        for (int i = 0; i < blocos.Count; i++)
        {
            if (i != index) blocos[i].material.color = corNormal;
            else blocos[i].material.color = corAtiva;
        }
        ((Sbot_jogador)jogador).VerificaVar(0);
    }

    public IEnumerator mover(Vector3 final)
    {
        movendo = true;

        // POSSIVELMENTE SERA RETIRADO
        if (final == Vector3.zero)
        {
            float pcX = inicialPos.x;
            float pcZ = inicialPos.z;

            float distancia = dist;

            Vector2 rand = Random.insideUnitCircle * distancia;

            float x = pcX + rand.x;
            float z = pcZ + rand.y;

            final = new Vector3(x, 0, z);
        }
        //

        final.y = pCentral.transform.position.y;

        while (Vector3.Distance(pCentral.transform.position, final) > 0.0005f)
        {
            if (energia.energia <= 0) yield break;
            pCentral.transform.position = Vector3.Lerp(pCentral.transform.position, final, Time.deltaTime * speed);
            yield return null;
        }
        pCentral.transform.position = final;

        movendo = false;
    }
}
