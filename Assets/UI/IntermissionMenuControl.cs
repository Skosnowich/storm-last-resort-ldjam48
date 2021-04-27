using System;
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
                        "Hope and Despair",
                        new[]
                        {
                            "You, the captain of a mighty pirate ship, lead your crew into many adventures and into great wealth. But - committing more and more acts of piracy did not remain unnoticed...",
                            "The Great Royal Fleet is now on pursuit to catch you and your crew and to bring you to your rightful sentence: Death.",
                            "Already seeing the crow's nests on the horizon, you decide to do a bold move - moving into the nearby storm in the hope that the Great Royal Fleet won't follow you there.",
                        },
                        "Continue"
                    );
                    break;
                case Stage._2_With_your_last_loot:
                    Refresh(
                        "Loot of the Past",
                        new[]
                        {
                            "As you shout the commands to your brave crew, you remember your last prize: a big royal trading ship.",
                            "Your share of the loot were 100 gold doubloons.",
                            "What did you do with that gold?",
                        },
                        "I bought a new cannon (+1 cannon / -50 gold)",
                        "I reinforced the ships hull (+10 max hull health / -50 gold)",
                        "I saved the money for worse times"
                    );
                    break;
                case Stage._3_First_Encounter:
                    Refresh(
                        "Ship ahoy!",
                        new[]
                        {
                            "\"Ship ahoy!\", the lookout screams while pointing at the rear end. You grab your spyglass and look out for the ship.",
                            "There you found it - a small vessel. \"Only a scout...\", you spit laughing to the coxswain, \"...that should not be a problem for us.\".",
                            "While turning the ship around to get the better of the enemy, your crew is cheering for the battle.",
                        },
                        "Engage the enemy"
                    );
                    break;
                case Stage._4_Scout_Sunk:
                    Refresh(
                        "They will never report again",
                        new[]
                        {
                            "As the ship slowly sinks underwater, some of your crew mates are already on their way to the ship to salvage the remains.",
                            "\"We don't have much time left, we need to go into the storm and leave the Great Royal lubbers behind us.\", you shout to them.",
                        },
                        "\"Take as much gold as you can and then return!\"",
                        "\"Pick those poor fellows up, at least those wanting to join us.\"",
                        "\"Our ship suffered damage, so bring back anything helpful and then fix the ship.\"",
                        optionTwoEnabled: GlobalGameState.CurrentCrewHealth < GlobalGameState.MaxCrewHealth,
                        optionThreeEnabled: GlobalGameState.CurrentHullHealth < GlobalGameState.MaxHullHealth
                    );
                    break;
                case Stage._END_Won:
                    Refresh(
                        "Storm - The Last Resort",
                        new[]
                        {
                            "As you finally reach the end of the storm, you once more take your spyglass and look back.",
                            "\"No one left to see.\", you think as you breath out relieved.",
                            "Happily, but also sad about all the friends you left in that storm, you gaze over the oceans horizon and then look at your brave crew - or what is left of them.",
                            "One of them steps forward: \"So, my captain... what are we going to do next?\""
                        },
                        "\"The same as always - seek out the next port, restock and search for new treasures.\"",
                        "\"What are you doing - I don't know. Me - I am going to retire on the next beautiful island we cross.\""
                    );
                    break;
                case Stage._END_Won_ResultScreen:
                    Refresh(
                        "The Future",
                        new[]
                        {
                            "Congratulations - You won the game.",
                            "Thank you very much for playing - I hope you had fun!"
                        },
                        "Back to Main Menu"
                    );
                    break;
                case Stage._END_GameOver:
                    Refresh(
                        "Deeper and Deeper...",
                        new[]
                        {
                            "...you and your crew sink.",
                            "Thinking about all the horrendous atrocities you did in your past life, you sink and sink - slowly reaching the Davy Jones's locker.",
                            "A last question crosses you mind: \"Did I live a good life or should I regret what I have done?\"",
                        },
                        "Accept all the failures you did in your life and return back to the Main Menu. (Ironman Mode)",
                        "Strike a deal with Davy Jones and retry battle. (Easy Mode)",
                        "Do some voodoo, repair your ship and retry battle. (Landlubber Mode)",
                        optionThreeEnabled: GlobalGameState.CurrentCrewHealth < GlobalGameState.MaxCrewHealth ||
                                            GlobalGameState.CurrentHullHealth < GlobalGameState.MaxHullHealth
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
            TextLabel.text = (_result != null ? _result + "\n\n" : "") + string.Join("\n\n", text);

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

                            _result = $"Your crew returns with {totalGoldAmount} gold.";
                            break;
                        case 2:
                            var randomSailorAmount = Random.Range(1, 3);
                            var totalSailorAmount = 2 + randomSailorAmount;
                            GlobalGameState.ChangeCrewHealth(totalSailorAmount);

                            _result = $"Your crew rescued {totalSailorAmount} sailors who decided to join your crew.";
                            break;
                        case 3:
                            var totalRepairAmount = 20 + Mathf.RoundToInt(10 * ((float) GlobalGameState.CurrentCrewHealth / GlobalGameState.MaxCrewHealth));
                            GlobalGameState.ChangeHullHealth(totalRepairAmount);

                            _result =
                                $"Your crew managed to salvage some materials and to repair some damage you took in the battle (+{totalRepairAmount} hull health).";
                            break;
                    }

                    UpdateToStage(Stage._END_Won);
                    break;
                case Stage._END_Won:
                    switch (_optionChosen)
                    {
                        case 1:
                            _result = "You and your crew challenge your luck and continue to pirate the seven seas. What lies in the future now is fully up to your own imagination.";
                            break;
                        case 2:
                            _result = "\"You are captain now.\", you tell the man who stepped forward a few weeks ago. You reached a island you see fit to retire. Is this really the end of your pirate life? Who knows - only you and your imagination.";
                            break;
                    }

                    UpdateToStage(Stage._END_Won_ResultScreen);
                    break;
                case Stage._END_Won_ResultScreen:
                    SceneManager.LoadScene("MainMenu");
                    break;
                case Stage._END_GameOver:
                    switch (_optionChosen)
                    {
                        case 1:
                            SceneManager.LoadScene("MainMenu");
                            break;
                        case 2:
                            GlobalGameState.Stage = GlobalGameState.LostStage;
                            SceneManager.LoadScene("GameScene");
                            break;
                        case 3:
                            GlobalGameState.CurrentCrewHealth = GlobalGameState.MaxCrewHealth;
                            GlobalGameState.CurrentHullHealth = GlobalGameState.MaxHullHealth;
                            GlobalGameState.Stage = GlobalGameState.LostStage;
                            SceneManager.LoadScene("GameScene");
                            break;
                    }

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
