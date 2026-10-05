using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public void StartNormal()
    {
        StartCoroutine(LoadRace(GameMode.Normal));
    }

    public void StartSpeedrun()
    {
        StartCoroutine(LoadRace(GameMode.Speedrun));
    }

    public void StartModMayhem()
    {
        StartCoroutine(LoadRace(GameMode.ModMayhem));
    }

    private IEnumerator LoadRace(GameMode gameMode)
    {
        GameManager.instance.selectedMode = gameMode;
        LeanTween.alphaCanvas(canvasGroup, 0f, 0.3f);

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene("Level 1");
    }
}