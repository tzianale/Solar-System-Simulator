using Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuScript : MonoBehaviour
    {

        private GameObject panel;

        // getter and setter for Panel
        public GameObject Panel
        {
            get { return panel; }
            set { panel = value; }
        }
  
        private void callSettings()
        {
            SceneManager.LoadSceneAsync(1);
        }

        public void openPanel()
        {
            if (Panel != null)
            {
                Panel.SetActive(true);
            }
        }

        public void closePanel()
        {
            if(Panel != null)
            {
                Panel.SetActive(false);
            }
        }

        private void loadExplorer()
        {
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Explorer;
            SceneManager.LoadSceneAsync(2);
        }

        private void loadSandbox()
        {
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Sandbox;
            SceneManager.LoadSceneAsync(1);
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}
