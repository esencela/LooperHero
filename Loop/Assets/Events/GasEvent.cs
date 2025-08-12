using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasEvent : MonoBehaviour
{
    public float activateTime;
    public float radius = 4.0f;
    public float force = 400f;

    public GameObject explosionPrefab;
    public GameObject firePrefab;

    bool activated = false;

    void Update()
    {
        if (Time.timeSinceLevelLoad > activateTime)
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (activated)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        List<IRagdoll> ragdolls = new List<IRagdoll>();

        Instantiate(explosionPrefab, transform);
        Instantiate(firePrefab, transform);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.root.TryGetComponent<IRagdoll>(out IRagdoll ragdoll))
            {
                if (ragdolls.Contains(ragdoll))
                    continue;
                
                ragdoll.Ragdoll(true);
                ragdolls.Add(ragdoll);
            }

            if (hits[i].transform.root.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Vector3 direction = hits[i].transform.position - transform.position;
                rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            }
        }

        activated = true;
    }
}
