using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_dis_pe : S_dis_boneGrab
{
    float altura = 0.09f;
    float largura = 0.07f;
    public bool ladoEsq;
    Vector3 posInical;
    S_jogador postura;
    XRGrabInteractable grab;

    private void Start()
    {
        posInical = transform.position;
        postura = GetComponentInParent<S_jogador>();
        grab = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        // recolar o parent que o nearfar tira
        if (transform.parent == null) transform.SetParent(pai.transform);

        if (postura.pernaAberta && !grab.isSelected) transform.position = Vector3.MoveTowards(transform.position, posInical, Time.deltaTime / 400);

        if (ik != null && ik.conectado)
        {
            transform.position = ik.conectado.transform.position;

            if (Vector3.Distance(ik.conectado.transform.position, GOinicial.transform.position) >= (dis + (ik.coll.radius / 50f))) ik.Desconecta();
        }

        // MOVIMENTO COM LIMITE MÁXIMO
        Vector3 current = transform.position;

        float y = Mathf.Clamp(current.y, posInical.y, posInical.y + altura);
        float x = Mathf.Clamp(current.x, posInical.x - largura, posInical.x + largura);
        float z = Mathf.Clamp(current.z, posInical.z - dis, posInical.z + dis);

        transform.position = new Vector3(x, y, z);
    }
}
