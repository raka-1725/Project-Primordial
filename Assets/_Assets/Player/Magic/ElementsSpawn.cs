using System.Collections.Generic;
using UnityEngine;

public class ElementsSpawn : MonoBehaviour
{
    public GameObject[] mElementsPrefab;
    public Transform[] mSpawnPoints;

    public int mMaxElements = 3;


    void Start()
    {
        for (int i = 0; i < mElementsPrefab.Length; i++) 
        {
            Instantiate(mElementsPrefab[i], mSpawnPoints[Random.Range(0,mSpawnPoints.Length)].position, Quaternion.Euler(0,0,90));
        }
    }
}
