using UnityEngine;

public class MarbleAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] impactSounds;

    public float minimumImpactForce = 4f;

    private void OnCollisionEnter(Collision collision)
    {
        float force = collision.relativeVelocity.magnitude;

        if (force < minimumImpactForce)
            return;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(impactSounds[Random.Range(0, impactSounds.Length)]);
    }
}