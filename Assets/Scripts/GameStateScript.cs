using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateScript : MonoBehaviour
{
    private bool isPaused = false;
    private float lastSpeed = 1f;

    public void playPause()
    {
        if (Time.timeScale != 0)
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
        if (Time.timeScale != 0) Time.timeScale = 1;        
    }

    public void increaseSpeedTen()
    {
        if (Time.timeScale != 0) Time.timeScale = 10;
    }

    public void simSpeedMaximum() 
    {
       if(Time.timeScale != 0) Time.timeScale = 100;
    }

}
