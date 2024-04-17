using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    private bool isPaused = false;
    private float lastSpeed = 1f;
    public static double explorerModeDay;
    public TextMeshProUGUI dayText;

    private void Start()
    {
        explorerModeDay = 0;
    }

    private void FixedUpdate()
    {
        if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer && !isPaused)
        {
            explorerModeDay += Time.deltaTime;
            dayText.text = "Day: " + explorerModeDay.ToString();
        }
    }

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

    public void setTimeScale(float timeScale)
    {
        if (Time.timeScale != 0) Time.timeScale = timeScale;
    }

}
