using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Models
{
    public class GameStateController : MonoBehaviour
    {
        public static bool isPaused = false;
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
            simulationSpeedText.text = simulationSpeedSlider.value.ToString("0") + " days per second";
            colorSpeedText();
        }

        private void FixedUpdate()
        {
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer && !isPaused)
            {
                DisplayDate(ComputeDateByCurrentDate(explorerModeDay));
                explorerModeDay += CelestialBody.isRewinding ? -Time.deltaTime : Time.deltaTime;
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

        private void colorSpeedText()
        {
            if (simulationSpeedSlider.value == simulationSpeedSlider.maxValue)
            {
                simulationSpeedText.color = new Color(255, 0, 0);
            }
            else if (simulationSpeedSlider.value == simulationSpeedSlider.minValue)
            {
                simulationSpeedText.color = new Color(0, 255, 0);
            }
            else
            {
                simulationSpeedText.color = new Color(125, 125, 0);
            }
        }
    }
}
