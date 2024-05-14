using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    public static bool isPaused = false;
    private float lastSpeed = 1f;
    public static double explorerModeDay;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI simulationSpeedText;
    public Slider simulationSpeedSlider;

    private void Start()
    {
        explorerModeDay = 0;
    }

    private void Update()
    {
        simulationSpeedText.text = simulationSpeedSlider.value.ToString("0") + "x";
    }

    private void FixedUpdate()
    {
        if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer && !isPaused)
        {
            DisplayDate(ComputeDateByCurrentDate(explorerModeDay));
            explorerModeDay += Time.deltaTime;
        }
    }

    public void playPause()
    {
        isPaused = !isPaused;
    }

    public void setTimeScale(float timeScale)
    {
        if (Time.timeScale != 0) Time.timeScale = timeScale;
    }

    private DateTime ComputeDateByCurrentDate(double daysPassed)
    {
        return DateTime.Now.AddDays(daysPassed);
    }

    private void DisplayDate(DateTime date)
    {
        dayText.text = date.ToString("d");
    }
    public void AddNewBody()
    {
        CelestialBody[] bodies =  FindObjectsOfType<CelestialBody>();
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.name = "new Body";
        CelestialBody newBody = gameObject.AddComponent<CelestialBody>();

        foreach (var item in bodies)
        {
            item.AppendCelestialBody(newBody);
        }
    }
}
