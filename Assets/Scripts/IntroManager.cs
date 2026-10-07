using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GameObject menuPanel;
    public Rigidbody playerIntroMarble;
    public GameObject marblePrefab;
    public Transform marbleSpawnPoint;
    public Transform marbleParent;

    public Color[] marbleColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, new Color(1f, 0.5f, 0f), new Color(0.5f, 0f, 1f), Color.white, Color.black };

    public int hordeCount = 200;

    private IEnumerator Start()
    {
        menuPanel.SetActive(false);
        //playerIntroMarble.linearVelocity = Vector3.right * 20f;

        yield return new WaitForSeconds(3f);

        SpawnHorde();

        yield return new WaitForSeconds(4f);

        menuPanel.SetActive(true);
    }

    private void SpawnHorde()
    {
        for (int i = 0; i < hordeCount; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-2f, 2f), Random.Range(0f, 2f), Random.Range(-1f, 1f));

            GameObject marble = Instantiate(marblePrefab, marbleSpawnPoint.position + offset, Quaternion.identity, marbleParent);

            Rigidbody rb = marble.GetComponent<Rigidbody>();

            if (rb != null)
            {
                //rb.linearVelocity = Vector3.right * 20f;
            }

            Renderer rend = marble.GetComponent<Renderer>();

            if (rend != null)
            {
                rend.material.color = marbleColors[Random.Range(0, marbleColors.Length)];
            }
        }
    }
}