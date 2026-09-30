using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MarbleSpawner : MonoBehaviour
{
    public GameObject marblePrefab;
    public Transform spawnPointMiddle;
    public Vector3 spawnPointArea = new Vector3(10, 10, 10);

    public int minMarbles = 50;
    public int maxMarbles = 200;

    public List<GameObject> activeMarbles = new List<GameObject>();

    private void Start()
    {
        SpawnMarbles();
    }

    private void Update()
    {
        
    }

    void SpawnMarbles()
    {
        int count = Random.Range(minMarbles, maxMarbles + 1); // (0,10) gives 0 to 9 so just adding +1
        
        for (int i = 0;  i < count; i++)
        {
            //calc random position in the area of the spawn so they act a lil mroe unpredictable
            Vector3 randomOffset = new Vector3(Random.Range(-spawnPointArea.x / 2f, spawnPointArea.x / 2f), Random.Range(-spawnPointArea.y / 2f, spawnPointArea.y / 2f), Random.Range(-spawnPointArea.z / 2f, spawnPointArea.z / 2f));

            GameObject marble = Instantiate(marblePrefab, spawnPointMiddle.position + randomOffset, Quaternion.identity);

            marble.name = (i == 0) ? "PlayerMarble" : $"Marble {i}";

            activeMarbles.Add(marble);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPointMiddle != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(spawnPointMiddle.position, spawnPointArea);
        }
    }
}
