using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public GameObject Panel;
    public void callSettings()
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

    public void loadExplorer()
    {
        SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Explorer;
        SceneManager.LoadSceneAsync(2);
    }

    public void loadSandbox()
    {
        SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Sandbox;
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
