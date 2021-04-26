using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class IntermissionMenuControl : MonoBehaviour
    {
        public bool StageOverride;
        public Stage StartStage;
        
        public Text HeadlineLabel;
        public Text TextLabel;
        public Button OptionOneButton;
        public Button OptionTwoButton;
        public Button OptionThreeButton;

        private int _optionChosen;

        private string _result;

        private void Start()
        {
            if (!StageOverride)
            {
                StartStage = GlobalGameState.Stage;
            }
            
            UpdateToStage(StartStage);
        }

        private void UpdateToStage(Stage stage)
        {
            GlobalGameState.Stage = stage;
            switch (stage)
            {
                case Stage._1_Introduction:
                    Refresh(
                        "Introduction",
                        new[]
                        {
                            "- followed by the royal fleet",
                            "- pirated a wee bit too much",
                            "- to escape trying to get into storm, because we have the mightiest sailors",
                            "- the last loot sold for 100 gold"
                        },
                        "Continue"
                    );
                    break;
                case Stage._2_With_your_last_loot:
                    Refresh(
                        "With your last loot ",
                        new[]
                        {
                            "- before the fleet detected you, you landed in a port to"
                        },
                        "Buy a new cannon (+1 cannon / -50 gold)",
                        "Reinforce your ships hull (+10 hp / -50 gold)",
                        "Save the money for worse times"
                    );
                    break;
                case Stage._3_First_Encounter:
                    Refresh(
                        "First encounter",
                        new[]
                        {
                            "- scout ship following you",
                            "- you need to destroy it"
                        },
                        "Continue"
                    );
                    break;
                case Stage._4_Scout_Sunk:
                    Refresh(
                        "Scout sunk",
                        new[]
                        {
                            "- you sank the scout",
                            "- you don't have much time looting the wreck before going on into the storm",
                            "- so you"
                        },
                        "...focus on looting for gold.",
                        "...focus on rescuing enemy sailors.",
                        "...focus on repairing damage."
                    );
                    break;
                case Stage._END_Won:
                    Refresh(
                        "Resort",
                        new[]
                        {
                            "- you won!"
                        },
                        "Back to Main Menu"
                    );
                    break;
                case Stage._END_GameOver:
                    Refresh(
                        "Fail",
                        new[]
                        {
                            "- you lost!"
                        },
                        "Back to Main Menu and try again"
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
            }
        }

        public void Refresh(string headline, string[] text, string optionOne = null, string optionTwo = null, string optionThree = null,
            bool optionOneEnabled = true, bool optionTwoEnabled = true, bool optionThreeEnabled = true)
        {
            HeadlineLabel.text = headline;
            TextLabel.text = _result + "\n\n" + string.Join("\n\n", text);

            OptionOneButton.GetComponentInChildren<Text>().text = "1. " + optionOne;
            OptionTwoButton.GetComponentInChildren<Text>().text = "2. " + optionTwo;
            OptionThreeButton.GetComponentInChildren<Text>().text = "3. " + optionThree;

            OptionOneButton.gameObject.SetActive(optionOne != null);
            OptionTwoButton.gameObject.SetActive(optionTwo != null);
            OptionThreeButton.gameObject.SetActive(optionThree != null);

            OptionOneButton.interactable = optionOneEnabled;
            OptionTwoButton.interactable = optionTwoEnabled;
            OptionThreeButton.interactable = optionThreeEnabled;

            _result = null;
        }

        private void OptionChosen()
        {
            switch (GlobalGameState.Stage)
            {
                case Stage._1_Introduction:
                    UpdateToStage(Stage._2_With_your_last_loot);
                    break;
                case Stage._2_With_your_last_loot:
                    switch (_optionChosen)
                    {
                        case 1:
                            GlobalGameState.CannonCount += 1;
                            GlobalGameState.Gold -= 50;
                            _result = "With your new cannon aboard you steer into the storm.";
                            break;
                        case 2:
                            GlobalGameState.MaxHullHealth += 10;
                            GlobalGameState.CurrentHullHealth += 10;
                            GlobalGameState.Gold -= 50;
                            _result = "With your hardened ship you steer into the storm.";
                            break;
                        case 3:
                            _result = "With your cargo hold, bursting of treasure, you steer into the storm.";
                            break;
                    }

                    UpdateToStage(Stage._3_First_Encounter);
                    break;
                case Stage._3_First_Encounter:
                    StartEncounter(Stage._3_First_Encounter);
                    break;
                case Stage._4_Scout_Sunk:
                    switch (_optionChosen)
                    {
                        case 1:
                            var randomGoldAmount = Random.Range(10, 21);
                            var totalGoldAmount = 30 + randomGoldAmount;
                            GlobalGameState.Gold += totalGoldAmount;
                            
                            _result = $"You looted {totalGoldAmount} gold from the scout.";
                            break;
                        case 2:
                            var randomSailorAmount = Random.Range(1, 3);
                            var totalSailorAmount = 2 + randomSailorAmount;
                            GlobalGameState.ChangeCrewHealth(totalSailorAmount);
                            
                            _result = $"You rescued sailors and {totalSailorAmount} of them decided to join your crew.";
                            break;
                        case 3:
                            var totalRepairAmount = 20 + Mathf.RoundToInt(10 * ((float) GlobalGameState.CurrentCrewHealth / GlobalGameState.MaxCrewHealth));
                            GlobalGameState.ChangeHullHealth(totalRepairAmount);
                            
                            _result = $"Your crew managed to repair some damage you took in the battle (+{totalRepairAmount} hull health).";
                            break;
                    }

                    UpdateToStage(Stage._END_Won);
                    break;
                case Stage._END_Won:
                case Stage._END_GameOver:
                    SceneManager.LoadScene("MainMenu");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartEncounter(Stage stage)
        {
            GlobalGameState.Stage = stage;
            SceneManager.LoadScene("GameScene");
        }

        public void OptionOneChosen()
        {
            _optionChosen = 1;
            OptionChosen();
        }

        public void OptionTwoChosen()
        {
            _optionChosen = 2;
            OptionChosen();
        }

        public void OptionThreeChosen()
        {
            _optionChosen = 3;
            OptionChosen();
        }
    }
}
