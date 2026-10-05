using UnityEngine;

public class MarbleAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] impactSounds;

    public float minimumImpactForce = 4f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("NPC Marble"))
            return;

        float force = collision.relativeVelocity.magnitude;

        if (force < minimumImpactForce)
            return;

        audioSource.pitch = Random.Range(0.6f, 1.4f);

        if (impactSounds.Length > 0)
        {
            audioSource.PlayOneShot(impactSounds[Random.Range(0, impactSounds.Length)]);
        }
    }
}