using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEvent : MonoBehaviour
{
    public GameObject triggerEvent;
    bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (!other.transform.root.GetComponent<CitizenController>())
            return;

        if (triggerEvent.TryGetComponent<IActivateEvent>(out IActivateEvent e))
        {
            activated = true;
            e.TargetInPlace();
        }
    }
}
