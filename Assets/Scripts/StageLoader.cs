using System;
using Audio;
using Ship;
using UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    public GameOverWindow GameOverWindow;
    public Stage Stage;
    private ShipControl _playerShip;

    public AudioMixerGroup AmbienteAudioMixerGroup;
    public AudioClip CalmAmbienteAudio;
    
    private SoundManager _soundManager;
    private bool _initializedStage;

    private void Start()
    {
        _playerShip = GameObject.FindWithTag("PlayerShip").GetComponent<ShipControl>();
    }

    private void Update()
    {
        if (_playerShip != null && !_playerShip.Invincible)
        {
            if (_playerShip.CurrentCrewHealth() < 0.01F)
            {
                GameOver("Your whole crew, including you, is dead...");
            }

            if (_playerShip.CurrentHullHealth() < 0.01F)
            {
                GameOver("Your ship was destroyed...");
                Destroy(_playerShip.gameObject);
            }
        }
        
        switch (Stage)
        {
            case Stage._3_First_Encounter:
                if (!_initializedStage)
                {
                    GetSoundManager().PlaySound(CalmAmbienteAudio, AmbienteAudioMixerGroup, loopingIdentifier: "ambienteAudio");
                    _initializedStage = true;
                }
                
                var enemyShips = GameObject.FindGameObjectsWithTag("EnemyShip");
                if (enemyShips.Length <= 0)
                {
                    Win(Stage._4_Scout_Sunk);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private SoundManager GetSoundManager()
    {
        if (_soundManager == null)
        {
            _soundManager = SoundManager.FindByTag();
        }

        return _soundManager;
    }

    private void GameOver(string losingText)
    {
        GameOverWindow.GameOverText = losingText;
        GameOverWindow.gameObject.SetActive(true);
        GlobalGameState.LostStage = GlobalGameState.Stage;
        GlobalGameState.Stage = Stage._END_GameOver;
    }

    private void Win(Stage nextStage)
    {
        GameOverWindow.GameOverText = "No more enemies in sight!";
        GameOverWindow.gameObject.SetActive(true);
        _playerShip.GetComponent<ShipControl>().UpdateToGlobalGameState();
        GlobalGameState.Stage = nextStage;
    }
}
