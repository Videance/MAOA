using UnityEngine;

public class S_segueIKjoint : MonoBehaviour
{
    public GameObject ik;

    void Update()
    {
        transform.position = ik.transform.position;
        transform.rotation = ik.transform.rotation;
    }
}
