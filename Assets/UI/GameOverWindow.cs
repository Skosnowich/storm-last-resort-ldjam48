using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverWindow : MonoBehaviour
    {
        public Text GameOverLabel;
        public string GameOverText;

        private void Update()
        {
            GameOverLabel.text = GameOverText;
        }

        public void PressedContinue()
        {
            GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>().enabled = false;
            SceneManager.LoadScene("InterMission");
        }
    }
}
