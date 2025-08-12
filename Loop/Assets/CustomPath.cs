using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPath : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    public bool loop = false;

    int currentPoint = 0;
    bool stopped = false;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            points.Add(child);
        }
    }

    public Transform GetCurrentPoint()
    {
        return points[currentPoint];
    }

    public void NextPoint()
    {
        if (stopped)
            return;

        currentPoint++;

        if (currentPoint >= points.Count)
        {
            if (loop)
            {
                currentPoint = 0;
            }
            else
            {
                currentPoint = points.Count - 1;
                stopped = true;
            }
        }
    }

    public bool IsLastPoint()
    {
        return stopped;
    }
}
