using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadSceneAsync(2);
    }

    private void loadSandbox()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
