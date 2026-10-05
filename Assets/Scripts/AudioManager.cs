using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("audio sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    [Header("bgm")]
    public AudioClip raceMusic;
    public AudioClip finishMusic;

    [Header("Wheel")]
    public AudioClip wheelTick;
    public AudioClip wheelFinish;

    [Header("Countdown")]
    public AudioClip countdownBeep;
    public AudioClip goSound;

    [Header("dead")]
    public AudioClip killPlaneSound;

    private void Awake()
    {
        instance = this;
    }

    public void PlayWheelTick()
    {
        uiSource.PlayOneShot(wheelTick);
    }

    public void PlayWheelFinish()
    {
        uiSource.PlayOneShot(wheelFinish);
    }

    public void PlayCountdown()
    {
        uiSource.PlayOneShot(countdownBeep);
    }

    public void PlayGo()
    {
        uiSource.PlayOneShot(goSound);
    }

    public void PlayKillPlane()
    {
        sfxSource.PlayOneShot(killPlaneSound);
    }

    public void StartRaceMusic()
    {
        musicSource.clip = raceMusic;
        musicSource.Play();
    }

    public void StartFinishMusic()
    {
        musicSource.Stop();
        musicSource.clip = finishMusic;
        musicSource.Play();
    }
}