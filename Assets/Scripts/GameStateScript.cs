using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateScript : MonoBehaviour
{
    private bool isPaused = false;
    public float lastSpeed = 1f;

    public void pauseResumeGame()
    {
        if (!isPaused)
        {
            lastSpeed = Time.timeScale;
            Time.timeScale = 0;
            isPaused = true;
        } else
        {
            Time.timeScale = lastSpeed;
            isPaused = false;
        }
    }


    public void setNormalSpeed()
    {
        Time.timeScale = 1;
    }

    public void increaseSpeedTen()
    {
        Time.timeScale = 10;
    }

    public void simSpeedMaximum()
    {
        Time.timeScale = 100;
    }

}
