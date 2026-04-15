using UnityEngine;

public class S_joint : MonoBehaviour
{
    ConfigurableJoint joint;

    public void CriarJoint(Rigidbody meuRb)
    {
        joint = gameObject.AddComponent<ConfigurableJoint>();

        joint.connectedBody = meuRb;

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        JointDrive drive = new JointDrive();
        drive.positionSpring = 2000f;
        drive.positionDamper = 150f;
        drive.maximumForce = Mathf.Infinity;

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;
    }

    public void RemoverJoint()
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }
}
