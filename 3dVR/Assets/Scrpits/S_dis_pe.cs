using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class S_dis_pe : S_dis_boneGrab
{
    float altura = 0.09f;
    float largura = 0.07f;
    public bool ladoEsq;
    public bool movendo;
    Vector3 posInical;
    S_jogador postura;
    public XRGrabInteractable grab;
    Rigidbody rb;

    private void Awake()
    {
        ik = GetComponent<S_IK>();
        dis = Vector3.Distance(GOponta.transform.position, GOinicial.transform.position);
        posInical = transform.position;
        postura = GetComponentInParent<S_jogador>();
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // recolar o parent que o nearfar tira
        if (transform.parent == null) transform.SetParent(pai.transform);

        if (postura.pernaAberta && !grab.isSelected && !movendo) transform.position = Vector3.MoveTowards(transform.position, posInical, Time.deltaTime / 330);

        if (ik != null && ik.conectado)
        {
            transform.position = ik.conectado.transform.position;

            if (Vector3.Distance(ik.conectado.transform.position, GOinicial.transform.position) >= (dis + (ik.coll.radius / 50f))) ik.Desconecta();
        }

        // MOVIMENTO COM LIMITE MÁXIMO
        Vector3 pos = transform.position;
        Vector3 original = pos;

        // Y
        if (pos.y > posInical.y + altura)
            pos.y = posInical.y + altura;
        else if (pos.y < posInical.y)
            pos.y = posInical.y;

        // X
        if (pos.x > posInical.x + largura)
            pos.x = posInical.x + largura;
        else if (pos.x < posInical.x - largura)
            pos.x = posInical.x - largura;

        // Z
        if (pos.z > posInical.z + dis)
            pos.z = posInical.z + dis;
        else if (pos.z < posInical.z - dis)
            pos.z = posInical.z - dis;

        // Só aplica se realmente mudou
        if (pos != original)
        {
            transform.position = pos;
        }
    }

    public IEnumerator Mover(bool praFrente)
    {
        movendo = true;
        Debug.Log("foi");

        if (!praFrente)
        {
            while (Vector3.Distance(transform.position, posInical) > 0.0005f)
            {
                transform.position = Vector3.Lerp(transform.position, posInical, Time.deltaTime / 10);
                yield return null;
            }
            transform.position = posInical;
        }
        else
        {
            float targetZ;

            if (ladoEsq) targetZ = posInical.z - (dis * 0.95f);
            else targetZ = posInical.z + (dis * 0.95f);

            while (Mathf.Abs(transform.position.z - targetZ) > 0.001f)
            {
                Vector3 pos = transform.position;

                pos.z = Mathf.MoveTowards(pos.z, targetZ, 0.2f * Time.deltaTime);

                transform.position = pos;

                yield return null;
            }
        }

        movendo = false;
    }
}
