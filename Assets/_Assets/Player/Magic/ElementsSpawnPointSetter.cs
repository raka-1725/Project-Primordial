using System.Linq;
using UnityEngine;

public class ElementsSpawnPointSetter : MonoBehaviour
{
    ElementsSpawn mSpawner;

    Transform[] mSpawnPoints;
    private void Awake()
    {
        mSpawner = GetComponent<ElementsSpawn>();

        SetSpawnPoints();
        mSpawner.mSpawnPoints = mSpawnPoints;
    }

    private void SetSpawnPoints()
    {
        mSpawnPoints = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
    }
}
