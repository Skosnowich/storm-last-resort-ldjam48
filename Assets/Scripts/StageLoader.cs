using System;
using Ship;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    public Stage Stage;
    private GameObject _playerShip;

    private void Start()
    {
        _playerShip = GameObject.FindWithTag("PlayerShip");
    }

    private void Update()
    {
        if (_playerShip == null)
        {
            GameOver();
        }
        
        switch (Stage)
        {
            case Stage._1_Introduction:
                break;
            case Stage._2_With_your_last_loot:
                break;
            case Stage._3_First_Encounter:
                var enemyShips = GameObject.FindGameObjectsWithTag("EnemyShip");
                if (enemyShips.Length <= 0)
                {
                    Win(Stage._4_Scout_Sunk);
                }
                break;
            case Stage._4_Scout_Sunk:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GameOver()
    {
        GlobalGameState.Stage = Stage._END_GameOver;
        SceneManager.LoadScene("InterMission");
    }

    private void Win(Stage nextStage)
    {
        _playerShip.GetComponent<ShipControl>().UpdateToGlobalGameState();
        GlobalGameState.Stage = nextStage;
        SceneManager.LoadScene("InterMission");
    }
}
