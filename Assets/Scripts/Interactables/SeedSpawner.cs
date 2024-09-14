using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeedSpawner : MonoBehaviour
{
    public GameObject seedPrefab;
    public List<Transform> spawnPoints;
    public int seedsToSpawn = 9;
    void Start()
    {
        Spawn();
    }

    void Update()
    {
        if(spawnPoints.Count < seedsToSpawn)
        {
            Debug.LogError("Not enough spawn points defined");
            return;
        }

        /*List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);
        ShuffleList(availableSpawnPoints);*/

        
    }

    private void Spawn()
    {
        for (int i = 0; i < seedsToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            Instantiate(seedPrefab, spawnPoint.position, Quaternion.identity);

            spawnPoints.Remove(spawnPoint);
        }
    }
}
