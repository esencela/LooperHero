using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    public float pushForce;

    bool pushing = false;

    List<Collider> colliders = new List<Collider>();

    void Update()
    {
        pushing = Input.GetButton("Fire1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pushing)
            return;

        if (other.transform.root == transform.root)
            return;

        if (colliders.Contains(other))
            return;

        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 direction = other.transform.position - transform.position;

            rb.AddForce(direction.normalized * pushForce, ForceMode.VelocityChange);            
        }

        if (other.transform.root.TryGetComponent<IRagdoll>(out IRagdoll ragdoll))
        {
            ragdoll.Ragdoll(false);
        }

        colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (colliders.Contains(other))
        {
            colliders.Remove(other);
        }
    }
}
