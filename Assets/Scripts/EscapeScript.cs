using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeScript : MonoBehaviour
{

    public GameObject Panel;
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

    public void escapeGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
