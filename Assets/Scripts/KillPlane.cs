using System.Collections;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    public OrbitCamera orbitCamera;
    public RaceManager raceManager;

    private void OnTriggerEnter(Collider other)
    {
        MarbleProgress progress = other.GetComponent<MarbleProgress>();

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (progress == null || rb == null)
            return;

        StartCoroutine(RespawnMarble(progress, rb));
    }

    private IEnumerator RespawnMarble(MarbleProgress progress, Rigidbody rb)
    {
        bool isPlayer = progress.CompareTag("Player");

        if (isPlayer)
        {
            orbitCamera.Freeze();
        }

        if (progress.CompareTag("Player") && raceManager.currentModifier == RaceManager.RaceModifier.OneLife)
        {
            raceManager.FinishRace();
            yield break;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (isPlayer)
        {
            AudioManager.instance.PlayKillPlane();
        }

        yield return new WaitForSeconds(0.5f);

        Transform checkpoint = CheckpointManager.Instance.checkpoints[progress.currentCheckpoint];

        rb.position = checkpoint.position;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (isPlayer)
        {
            Quaternion rotation = Quaternion.Euler(0f, orbitCamera.transform.eulerAngles.y, 0f);

            Vector3 cameraTargetPos = rb.position + rotation * orbitCamera.offset;

            LeanTween.move(orbitCamera.gameObject, cameraTargetPos, 0.75f).setEaseOutCubic();

            yield return new WaitForSeconds(0.75f);

            orbitCamera.Unfreeze();
        }
    }
}