using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEvent : MonoBehaviour
{
    public Vector3 destination;
    public Vector3 startPosition;

    public float activateTime;
    public float travelTime;
    float timeElapsed;

    public KillTrigger hurtbox;

    bool activated = false;

    void Awake()
    {
        startPosition = transform.position;
        timeElapsed = travelTime;
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > activateTime)
        {
            Activate();
        }

        if (timeElapsed >= travelTime)
        {
            hurtbox.active = false;
            return;
        }            

        transform.position = Vector3.Lerp(startPosition, destination, timeElapsed / travelTime);
        timeElapsed += Time.deltaTime;
    }

    public void Activate()
    {
        if (activated)
            return;

        activated = true;
        hurtbox.active = true;
        timeElapsed = 0.0f;
    }
}
