using UnityEngine;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseMenuGroup;

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
                    Pause();
                }
                else
                {
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
    }
}
