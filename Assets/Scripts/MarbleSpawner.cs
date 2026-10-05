using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MarbleSpawner : MonoBehaviour
{
    public GameObject marblePrefab;
    public Transform spawnPointMiddle;
    public Vector3 spawnPointArea = new Vector3(10, 10, 10);

    //public int minMarbles = 50;
    //public int maxMarbles = 200;

    public List<GameObject> activeMarbles = new List<GameObject>();

    [Header("colors")]
    public Material marbleMaterial;
    public Color[] marbleColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, new Color(1f, 0.5f, 0f), new Color(0.5f, 0f, 1f), Color.white, Color.black };

    [Header("sizes")]
    public float giantChance = 0.05f;
    public float tinyChance = 0.10f;

    public void SpawnMarbles(int count)
    {
        //int count = Random.Range(minMarbles, maxMarbles + 1); // (0,10) gives 0 to 9 so just adding +1
        
        for (int i = 0;  i < count; i++)
        {
            //calc random position in the area of the spawn so they act a lil mroe unpredictable
            Vector3 randomOffset = new Vector3(Random.Range(-spawnPointArea.x / 2f, spawnPointArea.x / 2f), Random.Range(-spawnPointArea.y / 2f, spawnPointArea.y / 2f), Random.Range(-spawnPointArea.z / 2f, spawnPointArea.z / 2f));

            GameObject marble = Instantiate(marblePrefab, spawnPointMiddle.position + randomOffset, Quaternion.identity);
            Renderer renderer = marble.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = marbleColors[Random.Range(0, marbleColors.Length)];
            }


            if ((i == 0))
            {
                marble.name = "PlayerMarble";
            }
            else
            {
                marble.name = $"Marble {i}";
            }

            activeMarbles.Add(marble);

            float roll = Random.value;

            if (roll < giantChance)
            {
                marble.transform.localScale *= 2f;

                Rigidbody rb = marble.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.mass *= 4f;
            }
            else if (roll < giantChance + tinyChance)
            {
                marble.transform.localScale *= 0.5f;

                Rigidbody rb = marble.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.mass *= 0.5f;
            }
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
