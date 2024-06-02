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
        public Toggle realTimeToggle;
        public static float currentExplorerTimeStep;

        private int simulationDirection = 1;

        private float scale = 1f;

        // DEBUGGING
        public bool toggleJ2000Time = false;


        /// <summary>
        /// Get acutal Data and color all the buttons and the speedtext correctly.
        /// </summary>
        private void Start()
        {
            double T = CalculateJulianCenturies(DateTime.UtcNow);
            explorerModeDay = toggleJ2000Time ? 0 : (T * 36525);
            colorSpeedText();
            switchDirectionToForward();
        }

        /// <summary>
        /// Check which date mode was choosen. By Real Time the simulation will be simulated like real time.
        /// Else the speed which the user have choosen will be used!
        /// </summary>
        private void FixedUpdate()
        {
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer && !isPaused)
            {
                if (realTimeToggle && realTimeToggle.isOn)
                {
                    currentExplorerTimeStep = Time.deltaTime / (24 * 3600) * simulationDirection;
                }
                else
                {
                    currentExplorerTimeStep = Time.deltaTime * scale * simulationDirection;
                }

                DisplayDate(ComputeDateByCurrentDate(explorerModeDay));
                explorerModeDay += currentExplorerTimeStep;
            }
        }

        /// <summary>
        /// Pause or Resume the simulation. Color the "Play / Pause" Button red, if the game is paused!
        /// </summary>
        public void playPause()
        {
            isPaused = !isPaused;
            executeColoring("Play / Pause", 255, isPaused ? 0 : 255, isPaused ? 0 : 255);            
        }

        
        public void setTimeScale(float timeScale)
        {
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Sandbox && Time.timeScale != 0)
            {
                Time.timeScale = timeScale;
            } else if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer)
            {
                scale = timeScale;
                colorSpeedText();
            }
        }

        private DateTime ComputeDateByCurrentDate(double daysPassed)
        {
            DateTime currentDate = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            currentDate = currentDate.AddDays(daysPassed);
            return currentDate;
        }

        private void DisplayDate(DateTime date)
        {
            TimeZoneInfo switzerlandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(date, switzerlandTimeZone);
            dayText.text = localDate.ToString("d MMM yyyy HH:mm:ss");
        }

        private void colorSpeedText()
        {
            simulationSpeedText.text = simulationSpeedSlider.value.ToString("0") + " days per second";

            switch (simulationSpeedSlider.value)
            {
                case float value when value == simulationSpeedSlider.maxValue:
                    colorText(simulationSpeedText, 255, 0, 0); // Red
                    break;
                case float value when value == simulationSpeedSlider.minValue:
                    colorText(simulationSpeedText, 0, 255, 0); // Green
                    break;
                default:
                    colorText(simulationSpeedText, 125, 125, 0); // Green
                    break;
            }
        }

        private void colorText(TextMeshProUGUI text, int r, int g, int b)
        {
            text.color = new Color(r, g, b);
        }

        private double CalculateJulianCenturies(DateTime date)
        {
            double JD = CalculateJulianDate(date);
            return (JD - 2451545.0) / 36525.0;
        }

        private double CalculateJulianDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }

            int A = year / 100;
            int B = 2 - A + (A / 4);
            double JD = Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + date.Day + B - 1524.5;

            return JD + (date.Hour + date.Minute / 60.0 + date.Second / 3600.0) / 24.0;
        }

        /// <summary>
        /// Set the simulationDirection to 1. The simulationspeed is positive! 
        /// Color the "ForwardButton" Green and the "BackwardButton" have no color!
        /// </summary>
        public void switchDirectionToForward()
        {
            simulationDirection = 1;
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer) colorButton();
        }

        /// <summary>
        /// Set the simulationDirection to -1. The simulationspeed is negative and the simulation moves backwards! 
        /// Color the "BackwardButton" Green and the "ForwardButton" have no color!
        /// </summary>
        public void switchDirectionToReverse()
        {
            simulationDirection = -1;
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer) colorButton();
        }

        private void colorButton()
        {
            executeColoring("ForwardButton", simulationDirection > 0 ? 0 : 255, 255, simulationDirection > 0 ? 0 : 255);
            executeColoring("BackwardButton", simulationDirection < 0 ? 0 : 255, 255, simulationDirection < 0 ? 0 : 255);
        }

        private void executeColoring(String buttonName, int r, int g, int b)
        {
            GameObject.Find(buttonName).GetComponent<Image>().color = new Color(r, g, b);
        }
    }
}
