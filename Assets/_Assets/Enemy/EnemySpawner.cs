using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] mEnemyPrefab;
    public float mSpawnInterval = 5.0f;
    public Transform[] mSpawnPoints;

    public int mMaxEnemies = 20;
    private readonly List<GameObject> mSpawnedEnemies = new List<GameObject>();

    public Transform mPlayer;
    public float mSpawnPriorityDistance = 15f;
    public float mWeightPower = 5f;

    void Start()
    {
        if (mPlayer == null)
        {
            mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        }

        mMaxEnemies = GameMode.Instance.enemyCount;

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    { 
        while (true)
        {
            yield return new WaitForSeconds(mSpawnInterval);
            mSpawnedEnemies.RemoveAll(enemy => enemy == null);
            if (mSpawnedEnemies.Count >= mMaxEnemies)
                continue;
            int spawnPointIndex = ChooseSpawnPointIndex();
            int enemyIndex = Random.Range(0, mEnemyPrefab.Length);
            GameObject spawned = Instantiate(mEnemyPrefab[enemyIndex], mSpawnPoints[spawnPointIndex].position, Quaternion.identity);
            mSpawnedEnemies.Add(spawned);
        }
    }

    private int ChooseSpawnPointIndex()
    {
        if (mSpawnPoints == null || mSpawnPoints.Length == 0)
            return 0;

        if (mPlayer == null || mSpawnPriorityDistance <= 0f)
            return Random.Range(0, mSpawnPoints.Length);

        float totalWeight = 0f;
        float[] weights = new float[mSpawnPoints.Length];
        const float epsilon = 0.0001f;

        for (int i = 0; i < mSpawnPoints.Length; i++)
        {
            float dist = Vector3.Distance(mPlayer.position, mSpawnPoints[i].position);
            // normalized: 1 when at player's position, 0 when at or beyond priority radius
            float normalized = 1f - Mathf.Clamp01(dist / mSpawnPriorityDistance);
            float weight = Mathf.Pow(normalized, mWeightPower);

            // Give a small non-zero weight to far spawn points so they are still possible
            if (weight <= epsilon) weight = epsilon;

            weights[i] = weight;
            totalWeight += weight;
        }

        if (totalWeight <= 0f)
            return Random.Range(0, mSpawnPoints.Length);

        float sample = Random.value * totalWeight;
        float accum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            accum += weights[i];
            if (sample <= accum)
                return i;
        }

        // Fallback (shouldn't reach here)
        return weights.Length - 1;
    }
}
