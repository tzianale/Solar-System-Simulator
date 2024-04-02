using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateScript : MonoBehaviour
{
    private bool isPaused = false;

    public void pauseResumeGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        } else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void increaseSimulationSpeed()
    {
        Time.timeScale = 1000;
    }

    public void setNormalSpeed()
    {
        Time.timeScale = 1;
    }
   
}
