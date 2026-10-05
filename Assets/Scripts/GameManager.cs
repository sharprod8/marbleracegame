using UnityEngine;

public enum GameMode
{
    Normal,
    Speedrun,
    ModMayhem
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameMode selectedMode;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}