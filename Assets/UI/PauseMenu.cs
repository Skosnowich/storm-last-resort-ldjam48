using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseMenuGroup;
        public Text ReallyWantToQuitMessage;

        private void Awake()
        {
            PauseMenuGroup.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GlobalGameState.IsUnpaused())
                {
                    ReallyWantToQuitMessage.text = "Quit your adventure";
                    Pause();
                }
                else
                {
                    ReallyWantToQuitMessage.text = "Quit your adventure";
                    Unpause();
                }
            }
        }

        private void Pause()
        {
            GlobalGameState.Pause();
            PauseMenuGroup.SetActive(true);
        }

        private void Unpause()
        {
            PauseMenuGroup.SetActive(false);
            GlobalGameState.Unpause();
        }

        public void ContinueButtonClicked()
        {
            Unpause();
        }

        public void BackToMainMenu()
        {
            const string reallyMessage = "If you are sure, click again...";
            if (ReallyWantToQuitMessage != null && ReallyWantToQuitMessage.text == reallyMessage)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else if (ReallyWantToQuitMessage != null)
            {
                ReallyWantToQuitMessage.text = reallyMessage;
            }
        }
    }
}
