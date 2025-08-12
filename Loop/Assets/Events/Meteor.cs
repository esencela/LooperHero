using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float fallTime;
    float timeElapsed;

    public Vector3 destination;
    public Vector3 startDestination;

    public KillTrigger hurtbox;

    void Awake()
    {
        timeElapsed = fallTime;
    }

    void Update()
    {
        if (timeElapsed >= fallTime)
        {
            hurtbox.active = false;
            return;
        }

        transform.position = Vector3.Lerp(startDestination, destination, timeElapsed / fallTime);
        timeElapsed += Time.deltaTime;
    }

    public void StartFall()
    {
        timeElapsed = 0.0f;
        hurtbox.active = true;
    }
}
