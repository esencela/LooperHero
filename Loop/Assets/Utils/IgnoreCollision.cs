using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    public Collider col;
    public Collider[] collidersToIgnore;

    void Start()
    {
        foreach (Collider otherCollider in collidersToIgnore)
        {
            Physics.IgnoreCollision(col, otherCollider, true);
        }
    }
}
