using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public Rigidbody rb;
    public float breakForce;

    bool grabbing = false;

    List<Collider> colliders = new List<Collider>();
    List<FixedJoint> connectedJoints = new List<FixedJoint>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        grabbing = Input.GetButton("Fire2");

        if (Input.GetButtonUp("Fire2"))
        {
            foreach (FixedJoint joint in connectedJoints)
            {
                Destroy(joint);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!grabbing)
            return;

        if (other.transform.root == transform.root)
            return;

        if (colliders.Contains(other))
            return;

        FixedJoint connectJoint = other.gameObject.AddComponent<FixedJoint>();
        connectJoint.connectedBody = rb;
        connectJoint.breakForce = breakForce;
        connectedJoints.Add(connectJoint);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!colliders.Contains(other))
            return;

        if (other.gameObject.TryGetComponent<FixedJoint>(out FixedJoint joint))
            Destroy(joint);

        colliders.Remove(other);
    }
}
