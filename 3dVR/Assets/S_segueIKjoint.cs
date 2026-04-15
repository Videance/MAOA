using UnityEngine;

public class S_segueIKjoint : MonoBehaviour
{
    public GameObject ik;

    // Update is called once per frame
    void Update()
    {
        transform.position = ik.transform.position;
        transform.rotation = ik.transform.rotation;
    }
}
