using System;
using System.Linq;
using Audio;
using Ship;
using UI;
using UnityEngine;
using UnityEngine.Audio;

public class StageLoader : MonoBehaviour
{
    public GameOverWindow GameOverWindow;
    public Stage Stage;
    private ShipControl _playerShip;

    public AudioMixerGroup AmbienteAudioMixerGroup;
    public AudioClip CalmAmbienteAudio;

    public AudioMixerGroup RainAudioMixerGroup;
    public AudioClip HeavyRainAudio;
    public AudioClip NotSoHeavyRainAudio;

    private SoundManager _soundManager;
    private bool _initializedStage;

    private float _missionTime;

    private void Start()
    {
        _playerShip = GameObject.FindWithTag("PlayerShip").GetComponent<ShipControl>();
    }

    private void Update()
    {
//        if (Input.GetKeyDown(KeyCode.H))
//        {
//            var enemyShips = GameObject.FindGameObjectsWithTag("EnemyShip");
//            foreach (var enemyShip in enemyShips)
//            {
//                Destroy(enemyShip);
//            }
//        }

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

        if (!GameOverWindow.gameObject.activeSelf)
        {
            switch (Stage)
            {
                case Stage._3_First_Encounter:
                    if (!_initializedStage)
                    {
                        GetSoundManager().PlaySound(CalmAmbienteAudio, AmbienteAudioMixerGroup, loopingIdentifier: "ambienteAudio");
                        _initializedStage = true;
                    }
                    else
                    {
                        WinWhenAllEnemiesAreDead(Stage._4_Scout_Sunk);
                    }

                    break;
                case Stage._8_Begin_o_storm:
                    if (!_initializedStage)
                    {
                        GetSoundManager().PlaySound(CalmAmbienteAudio, AmbienteAudioMixerGroup, loopingIdentifier: "ambienteAudio");
                        GetSoundManager().PlaySound(NotSoHeavyRainAudio, RainAudioMixerGroup, loopingIdentifier: "rainAudio");
                        _initializedStage = true;
                    }

                    else
                    {
                        WinWhenAllEnemiesAreDead(Stage._9_After_Begin_o_storm);
                    }

                    break;
                case Stage._12_Fight_in_the_storm:
                    if (!_initializedStage)
                    {
                        GetSoundManager().PlaySound(CalmAmbienteAudio, AmbienteAudioMixerGroup, loopingIdentifier: "ambienteAudio");
                        GetSoundManager().PlaySound(HeavyRainAudio, RainAudioMixerGroup, loopingIdentifier: "rainAudio");
                        _initializedStage = true;
                    }

                    else
                    {
                        WinWhenAllEnemiesAreDead(Stage._13_After_Fight_in_the_storm);
                    }

                    break;
                case Stage._16_Flee_in_the_storm:
                    if (!_initializedStage)
                    {
                        GetSoundManager().PlaySound(CalmAmbienteAudio, AmbienteAudioMixerGroup, loopingIdentifier: "ambienteAudio");
                        GetSoundManager().PlaySound(HeavyRainAudio, RainAudioMixerGroup, loopingIdentifier: "rainAudio");
                        _initializedStage = true;
                        _missionTime = 0;
                    }
                    else
                    {
                        _missionTime += Time.deltaTime;

                        if (_missionTime > 60)
                        {
                            var enemyShips = GameObject.FindGameObjectsWithTag("EnemyShip");
                            foreach (var enemyShip in enemyShips)
                            {
                                Destroy(enemyShip);
                            }
                        }

                        WinWhenAllEnemiesAreDead(Stage._END_Won);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void WinWhenAllEnemiesAreDead(Stage nextStage)
    {
        var enemyShips = GameObject.FindGameObjectsWithTag("EnemyShip");
        if (enemyShips.Length <= 0 || enemyShips.All(enemyShip => enemyShip.GetComponent<ShipControl>().CurrentCrewHealth() < 0.01F))
        {
            Win(nextStage);
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
        _playerShip.DamageOverTime = 0;
        _playerShip.Invincible = true;
        GameOverWindow.GameOverText = "No more enemies in sight!";
        GameOverWindow.gameObject.SetActive(true);
        _playerShip.GetComponent<ShipControl>().UpdateToGlobalGameState();
        GlobalGameState.Stage = nextStage;
    }
}
