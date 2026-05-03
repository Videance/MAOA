using Meta.XR.Simulator.Editor;
using UnityEngine;

public class S_dis_pe : S_dis_boneGrab
{
    float altura = 0.009f;
    float largura = 0.007f;
    Vector3 posInical;
    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
        posInical = transform.position;
    }

    private void Update()
    {
        // recolar o parent que o nearfar tira
        if (transform.parent == null) transform.SetParent(pai.transform);

        if (ik != null && ik.conectado)
        {
            transform.position = ik.conectado.transform.position;

            if (Vector3.Distance(ik.conectado.transform.position, GOinicial.transform.position) >= (dis + (ik.coll.radius / 50f))) ik.Desconecta();
        }

        // MOVIMENTO COM LIMITE MÁXIMO
        float y = Mathf.Clamp(pos.y, posInical.y, posInical.y + altura);
        float x = Mathf.Clamp(pos.x, posInical.x - largura, posInical.x + largura);
        float z = Mathf.Clamp(pos.z, posInical.z - dis, posInical.z + dis);

        transform.position = new Vector3(x, y, z);
    }
}
