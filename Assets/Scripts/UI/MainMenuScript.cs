using Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainMenuPanel;

        [SerializeField]
        private GameObject settingsPanel;

        public void OnSettingsButton()
        {
            OpenSettingsPanel();
        }

        public void OnQuitButton()
        {
            Application.Quit();
        }

        public void OnBackButton()
        {
            CloseSettingsPanel();
        }

        public void OnExplorerButton()
        {
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Explorer;
            SceneManager.LoadSceneAsync(2);
        }

        public void OnSandboxButton()
        {
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Sandbox;
            SceneManager.LoadSceneAsync(1);
        }
        public void OpenSettingsPanel()
        {
            settingsPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
        }

        public void CloseSettingsPanel()
        {
            mainMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }
    }
}
