using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public RaceManager raceManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        raceManager.FinishRace();
    }
}