using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeScript : MonoBehaviour
{

    public GameObject panel;

    public GameObject Panel
    {
        get { return panel; }
        set { panel = value; }
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
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
    }

    private void escapeGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
