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
        

        private void Start()
        {
            double T = CalculateJulianCenturies(DateTime.UtcNow);
            explorerModeDay = toggleJ2000Time ? 1 : (T * 36525);
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

        public void playPause()
        {
            isPaused = !isPaused;
        }

        public void setTimeScale(float timeScale)
        {
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Sandbox && Time.timeScale != 0)
            {
                Time.timeScale = timeScale;
            } else if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer)
            {
                scale = timeScale;
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

        public void switchDirectionToForward()
        {
            simulationDirection = 1;
        }

        public void switchDirectionToReverse()
        {
            simulationDirection = -1;
        }
    }
}
