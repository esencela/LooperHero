using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEvent : MonoBehaviour
{
    public float activateTime;

    public GameObject meteorPrefab;
    public Vector3 meteorSpawnPoint;

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

        Meteor meteor = Instantiate(meteorPrefab).GetComponent<Meteor>();
        meteor.transform.position = meteorSpawnPoint;
        meteor.startDestination = meteorSpawnPoint;
        meteor.destination = transform.position;
        meteor.StartFall();

        StartCoroutine(SpawnAnimations(meteor.fallTime));

        activated = true;
    }

    IEnumerator SpawnAnimations(float time)
    {
        yield return new WaitForSeconds(time);

        Instantiate(explosionPrefab, transform);
        Instantiate(firePrefab, transform);
    }
}
