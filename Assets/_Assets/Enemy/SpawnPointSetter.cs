using System;
using System.Linq;
using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    EnemySpawner mSpawner;

    Transform[] mSpawnPoints;
    private void Awake()
    {
        mSpawner = GetComponent<EnemySpawner>();

        SetSpawnPoints();
        mSpawner.mSpawnPoints = mSpawnPoints;
    }

    private void SetSpawnPoints()
    {
        mSpawnPoints = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
    }
}
