using UnityEngine;

public class GlobalGameState : MonoBehaviour
{
    private static bool _paused;

    private void Start()
    {
        _paused = false;
    }

    public  static void Pause()
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
