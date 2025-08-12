using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    Rigidbody rb;
    ConfigurableJoint joint;

    float startXPositionSpring = 0.0f;
    float startYZPositionSpring = 0.0f;

    public Rigidbody targetRigidbody;

    public bool syncAnimation = false;

    Quaternion initialRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();

        initialRotation = transform.localRotation;

        startXPositionSpring = joint.angularXDrive.positionSpring;
        startYZPositionSpring = joint.angularYZDrive.positionSpring;
    }

    public void SyncJointAnimation()
    {
        if (!syncAnimation)
            return;

        ConfigurableJointExtensions.SetTargetRotationLocal(joint, targetRigidbody.transform.localRotation, initialRotation);
    }

    public void Ragdoll()
    {
        JointDrive xDrive = joint.angularXDrive;
        JointDrive yzDrive = joint.angularYZDrive;
        xDrive.positionSpring = 5.0f;
        yzDrive.positionSpring = 5.0f;
        joint.angularXDrive = xDrive;
        joint.angularYZDrive = yzDrive;
    }

    public void ActiveRagdoll()
    {
        JointDrive xDrive = joint.angularXDrive;
        JointDrive yzDrive = joint.angularYZDrive;
        xDrive.positionSpring = startXPositionSpring;
        yzDrive.positionSpring = startYZPositionSpring;
        joint.angularXDrive = xDrive;
        joint.angularYZDrive = yzDrive;
    }
}
