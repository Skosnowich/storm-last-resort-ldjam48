using UnityEngine;

public class GlobalGameState : MonoBehaviour
{
    public static int MaxHullHealth;
    public static int MaxCrewHealth;

    public static int CurrentHullHealth;
    public static int CurrentCrewHealth;
    public static int CannonCount;
    public static int ReadyUpTime;

    private static bool _initialized;
    private static bool _paused;

    private void Start()
    {
        _paused = false;
        if (!_initialized)
        {
            Debug.LogWarning("Initialize GlobalGameState somewhere!");
            Initialize();
        }
    }

    public static void Initialize()
    {
        _paused = false;

        MaxHullHealth = 100;
        MaxCrewHealth = 50;
        CannonCount = 5;
        ReadyUpTime = 10;

        CurrentHullHealth = MaxHullHealth;
        CurrentCrewHealth = MaxCrewHealth;
        
        _initialized = true;
    }

    public static void Pause()
    {
        Time.timeScale = 0f;
        _paused = true;
        AudioListener.pause = true;
    }

    public static void Unpause()
    {
        Time.timeScale = 1f;
        _paused = false;
        AudioListener.pause = false;
    }

    public static bool IsUnpaused()
    {
        return !_paused;
    }
}
