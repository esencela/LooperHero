using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    public bool active = false;

    List<GameObject> hits = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
            return;

        if (hits.Contains(other.transform.root.gameObject))
            return;

        if (other.transform.root.TryGetComponent<IRagdoll>(out IRagdoll ragdoll))
        {
            ragdoll.Ragdoll(true);
            hits.Add(other.transform.root.gameObject);
        }
    }
}
