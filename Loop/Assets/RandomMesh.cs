using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMesh : MonoBehaviour
{
    public GameObject[] meshes;

    void Start()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].SetActive(false);
        }

        SetRandomMesh();
    }

    void SetRandomMesh()
    {
        int random = Random.Range(0, meshes.Length);

        meshes[random].SetActive(true);
    }
}
