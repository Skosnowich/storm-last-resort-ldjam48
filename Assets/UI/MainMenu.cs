using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {

        public void StartGame()
        {
            GlobalGameState.Initialize();
            SceneManager.LoadScene("InterMission");
        }
        
    }
}
