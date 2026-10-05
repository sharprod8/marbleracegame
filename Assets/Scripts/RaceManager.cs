using System.Collections;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public MarbleSpawner marbleSpawner;
    public Rigidbody playerRb;
    public Transform player;

    [Header("start stuff")]
    public GameObject startGate;
    public FanScript[] fans;

    [Header("race")]
    public int marbleCount = 100;

    [Header("UI")]
    public TMP_Text countdownText;
    public TMP_Text timerText;
    public TMP_Text speedText;
    public TMP_Text marbleCountText;
    public TMP_Text positionText;

    private float raceTimer;
    private int wheelResult;

    [Header("wheel")]
    public TMP_Text wheelResultText;
    public int[] wheelValues = { 100, 250, 500, 750, 1000 };
    public float wheelSpinTime = 3f;
    public float wheelUpdateRate = 0.1f;

    [Header("Results")]
    public GameObject resultsPanel;
    public TMP_Text finalTimeText;
    public TMP_Text finalPositionText;
    public TMP_Text finalMarbleCountText;

    public enum RaceState
    {
        Waiting,
        Countdown,
        Racing,
        Finished
    }

    public RaceState currentState;

    private void Start()
    {
        countdownText.gameObject.SetActive(false);
        StartCoroutine(SpinWheel());
    }

    IEnumerator SpinWheel()
    {
        float elapsed = 0f;

        while (elapsed < wheelSpinTime)
        {
            int displayValue = wheelValues[Random.Range(0, wheelValues.Length)];
            wheelResultText.text = displayValue.ToString();
            wheelResultText.transform.localScale = Vector3.one;
            LeanTween.scale(wheelResultText.gameObject, Vector3.one * 1.2f,0.08f).setEaseOutBack();
            elapsed += wheelUpdateRate;

            yield return new WaitForSeconds(wheelUpdateRate);
        }

        wheelResult = wheelValues[Random.Range(0, wheelValues.Length)];
        marbleCount = wheelResult;

        wheelResultText.text = wheelResult.ToString();
        marbleSpawner.SpawnMarbles(marbleCount);

        yield return new WaitForSeconds(1f);

        StartCoroutine(StartCountdown());
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
            int position = 1;

            foreach (GameObject marble in marbleSpawner.activeMarbles)
            {
                if (marble == null)
                    continue;

                if (marble.transform.position.z > player.position.z)
                {
                    position++;
                }
            }

            positionText.text = position.ToString();
        }
    }

    IEnumerator StartCountdown()
    {
        currentState = RaceState.Countdown;
        wheelResultText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.text = "GO!";
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

        LeanTween.scale(resultsPanel, Vector3.one, 0.4f).setEaseOutBack();

        finalTimeText.text ="Time: " + raceTimer.ToString("F2"); //still two d.p

        finalPositionText.text ="Position: " + positionText.text;

        finalMarbleCountText.text ="Marbles: " + marbleCount;
    }
}