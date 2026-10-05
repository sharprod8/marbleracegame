using System.Collections;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public MarbleSpawner marbleSpawner;
    public MarbleProgress playerProgress;
    public CameraShake cameraShake;
    public Rigidbody playerRb;
    public Transform player;

    [Header("start stuff")]
    public GameObject startGate;
    public FanScript[] fans;
    public float modChance = 1f;

    [Header("race")]
    public int marbleCount = 100;

    [Header("UI")]
    public TMP_Text countdownText;
    public TMP_Text timerText;
    public TMP_Text speedText;
    public TMP_Text marbleCountText;
    public TMP_Text positionText;
    public TMP_Text jackpotText;
    private Vector2 wheelDefaultPos;
    private Vector2 modifierDefaultPos;

    private float raceTimer;
    private float positionUpdateTimer;
    private int wheelResult;

    [Header("wheel")]
    public TMP_Text wheelResultText;
    public int[] wheelValues = { 100, 200, 300, 500, 750, 1000 }; //6 slces
    public float wheelSpinTime = 3f;
    public float wheelUpdateRate = 0.1f;

    [Header("Results")]
    public GameObject resultsPanel;
    public TMP_Text finalTimeText;
    public TMP_Text finalPositionText;
    public TMP_Text finalMarbleCountText; [Header("Modifiers")]

    public TMP_Text modifierText;
    public RaceModifier currentModifier;

    public enum RaceState
    {
        Waiting,
        Countdown,
        Racing,
        Finished
    }

    public enum RaceModifier
    {
        None,
        LowGravity,
        BigMarbles,
        TinyMarbles,
        NoSpeedCap,
        OneLife
    }

    public RaceState currentState;

    private void Start()
    {
        countdownText.gameObject.SetActive(false);
        jackpotText.gameObject.SetActive(false);
        modifierText.gameObject.SetActive(false);

        wheelDefaultPos = wheelResultText.rectTransform.anchoredPosition;

        modifierDefaultPos = modifierText.rectTransform.anchoredPosition;

        StartCoroutine(SpinWheel());
    }

    IEnumerator SpinWheel()
    {
        float delay = 0.05f;
        int spins = Random.Range(20, 35);

        for (int i = 0; i < spins; i++)
        {
            int displayValue =wheelValues[Random.Range(0, wheelValues.Length)];
            wheelResultText.text = displayValue.ToString();
            AudioManager.instance.PlayWheelTick();

            LeanTween.cancel(wheelResultText.gameObject);
            wheelResultText.transform.localScale = Vector3.one;

            LeanTween.scale(wheelResultText.gameObject, Vector3.one * 1.2f, 0.1f).setEaseOutBack();

            yield return new WaitForSeconds(delay);

            delay *= 1.08f;
        }

        wheelResult = wheelValues[Random.Range(0, wheelValues.Length)];
        AudioManager.instance.PlayWheelFinish();

        wheelResultText.text = wheelResult.ToString() + "\nMARBLES!";
        if (wheelResult == 1000)
        {
            wheelResultText.color = Color.red;
        }
        else
        {
            wheelResultText.color = Color.white;
        }

        

        LeanTween.scale(wheelResultText.gameObject, Vector3.one * 1.6f, 0.3f).setEaseOutBack();
        LeanTween.scale(wheelResultText.gameObject, Vector3.one, 0.2f).setDelay(0.3f);
        
        if (wheelResult == 1000)
        {
            jackpotText.gameObject.SetActive(true);
            LeanTween.scale(jackpotText.gameObject, Vector3.one * 1.6f, 0.3f).setEaseOutBack();
            LeanTween.scale(jackpotText.gameObject, Vector3.one, 0.2f).setDelay(0.3f);

            yield return new WaitForSeconds(1f);
            jackpotText.gameObject.SetActive(false);
        }
        marbleCount = wheelResult;

        if (Random.value < modChance)
        {
            yield return new WaitForSeconds(1f);

            LeanTween.scale(wheelResultText.gameObject, Vector3.zero, 0.3f).setEaseInBack();

            yield return new WaitForSeconds(0.3f);

            wheelResultText.gameObject.SetActive(false);

            yield return StartCoroutine(SpinModifier());

            wheelResultText.gameObject.SetActive(true);
            wheelResultText.transform.localScale = Vector3.one;
        }

        marbleSpawner.SpawnMarbles(marbleCount);

        yield return new WaitForSeconds(1f);

        StartCoroutine(StartCountdown());
    }

    private IEnumerator SpinModifier()
    {
        modifierText.gameObject.SetActive(true);

        float delay = 0.05f;
        int spins = Random.Range(15, 25);

        RaceModifier[] possibleModifiers =
        {
            RaceModifier.LowGravity,
            RaceModifier.BigMarbles,
            RaceModifier.TinyMarbles,
            RaceModifier.NoSpeedCap,
            RaceModifier.OneLife
        };

        for (int i = 0; i < spins; i++)
        {
            RaceModifier displayed = possibleModifiers[Random.Range(0, possibleModifiers.Length)];

            modifierText.text = displayed.ToString();
            LeanTween.cancel(modifierText.gameObject);
            modifierText.transform.localScale = Vector3.one;
            LeanTween.scale(modifierText.gameObject, Vector3.one * 1.2f, 0.1f).setEaseOutBack();

            yield return new WaitForSeconds(delay);

            delay *= 1.08f;
        }

        RollModifier();
        modifierText.text = currentModifier.ToString();
        LeanTween.scale(modifierText.gameObject, Vector3.one * 1.5f, 0.3f).setEaseOutBack();

        ApplyModifier();

        yield return new WaitForSeconds(2f);

        LeanTween.scale(modifierText.gameObject, Vector3.zero, 0.3f).setEaseInBack();

        yield return new WaitForSeconds(0.3f);

        modifierText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (currentState == RaceState.Racing)
        {
            raceTimer += Time.deltaTime;

            timerText.text = raceTimer.ToString("F2"); //two d.p
        }

        float speed = playerRb.linearVelocity.magnitude;
        speedText.text = Mathf.RoundToInt(speed).ToString();

        if (currentState == RaceState.Racing)
        {
            positionUpdateTimer += Time.deltaTime;

            if (positionUpdateTimer >= 0.1f)
            {
                positionUpdateTimer = 0f;

                positionText.text = CalculatePosition().ToString();
            }
        }
    }

    IEnumerator StartCountdown()
    {
        currentState = RaceState.Countdown;
        wheelResultText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);

        AudioManager.instance.StartRaceMusic();

        countdownText.text = "3";
        AudioManager.instance.PlayCountdown();
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        AudioManager.instance.PlayCountdown();
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        AudioManager.instance.PlayCountdown();
        yield return new WaitForSeconds(1);

        countdownText.text = "GO!";
        AudioManager.instance.PlayGo();
        currentState = RaceState.Racing;

        Destroy(startGate);

        foreach (FanScript fan in fans)
        {
            fan.isActive = true;
        }

        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
    }

    public void FinishRace()
    {
        currentState = RaceState.Finished;
        resultsPanel.transform.localScale = Vector3.zero;
        resultsPanel.SetActive(true);
        AudioManager.instance.StartFinishMusic();

        LeanTween.scale(resultsPanel, Vector3.one, 0.4f).setEaseOutBack();

        finalTimeText.text ="Time: " + raceTimer.ToString("F2"); //still two d.p

        finalPositionText.text ="Position: " + positionText.text;

        finalMarbleCountText.text ="Marbles: " + marbleCount;
    }

    private int CalculatePosition()
    {
        int position = 1;

        float playerScore = GetProgressScore(playerProgress);

        foreach (GameObject marble in marbleSpawner.activeMarbles)
        {
            if (marble == null)
                continue;

            MarbleProgress progress = marble.GetComponent<MarbleProgress>();

            if (progress == null)
                continue;

            float marbleScore = GetProgressScore(progress);

            if (marbleScore > playerScore)
            {
                position++;
            }
        }

        return position;
    }

    private float GetProgressScore(MarbleProgress progress)
    {
        int cp = progress.currentCheckpoint;

        if (cp >= CheckpointManager.Instance.checkpoints.Length - 1)
        {
            return cp * 100000f;
        }

        Transform currentCheckpoint = CheckpointManager.Instance.checkpoints[cp];

        Transform nextCheckpoint = CheckpointManager.Instance.checkpoints[cp + 1];

        float segmentLength = Vector3.Distance(currentCheckpoint.position, nextCheckpoint.position);

        float distanceToNext = Vector3.Distance(progress.transform.position, nextCheckpoint.position);

        float segmentProgress = Mathf.Clamp01(1f - (distanceToNext / segmentLength));

        return cp * 100000f + segmentProgress; //carter here

        /*speed why u tryna not laugh bru*/
    }

    private void RollModifier()
    {
        RaceModifier[] possibleModifiers =
        {
            RaceModifier.LowGravity,
            RaceModifier.BigMarbles,
            RaceModifier.TinyMarbles,
            RaceModifier.NoSpeedCap,
            RaceModifier.OneLife
        };

        currentModifier = possibleModifiers[Random.Range(0, possibleModifiers.Length)];
    }

    private void ApplyModifier()
    {
        switch (currentModifier)
        {
            case RaceModifier.LowGravity:
                Physics.gravity = new Vector3(0f, -4.9f, 0f);
                break;

            case RaceModifier.NoSpeedCap:
                playerRb.GetComponent<NewBallController>().maxSpeed = 9999f;
                break;
        }
    }
}