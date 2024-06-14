using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Models
{

    /// <summary>
    /// Handling date changes and updates the gamestate accordingly.
    /// </summary>
    public class DateChange : MonoBehaviour
    {
        // year 0 to 9999
        [SerializeField] private TMP_Dropdown dayDropdown;
        [SerializeField] private TMP_Dropdown monthDropdown;
        [SerializeField] private InputField yearInputField;

        [SerializeField] private TMP_Dropdown hourDropdown;
        [SerializeField] private TMP_Dropdown minuteDropdown;
        [SerializeField] private TMP_Dropdown secondDropdown;
        //private List<Resolution> filteredResolutions = new List<Resolution>(); // List to keep unique resolutions

        [SerializeField] private GameStateController gameStateController;

        [SerializeField] private GameObject datePanel;

        // Start is called before the first frame update
        private readonly List<string> options = new List<string>();        

        private int previousYear = -1;
        private int previousMonth = -1;

        private readonly int actualValue = 1;

        private readonly int actualMonth = 1;

        private void Start()
        {
            PopulateMonth();
            fillHour();
            fillMinuteAndSecond();
        }

        // Update is called once per frame
        private void Update()
        {
            int year = PopulateYearRes();
            int month = monthDropdown.value + actualMonth;

            if (year != previousYear || month != previousMonth)
            {
                PopulateDayRes(year, month);
                previousYear = year;
                previousMonth = month;
            }
        }
        private void PopulateMonth()
        {
            monthDropdown.ClearOptions();
            options.Clear();

            options.Add("January");
            options.Add("February");
            options.Add("March");
            options.Add("April");
            options.Add("May");
            options.Add("June");
            options.Add("July");
            options.Add("August");
            options.Add("September");
            options.Add("October");
            options.Add("November");
            options.Add("December");


            monthDropdown.AddOptions(options);
        }

        private void fillHour()
        {
            hourDropdown.ClearOptions();
            options.Clear();

            for (int i = 0; i < 24; i++)
            {
                options.Add(i.ToString());
            }
            hourDropdown.AddOptions(options);
        }

        private void fillMinuteAndSecond()
        {
            minuteDropdown.ClearOptions();
            secondDropdown.ClearOptions();

            options.Clear();

            for (int i = 0; i < 60; i++)
            {
                options.Add(i.ToString());
            }

            minuteDropdown.AddOptions(options);
            secondDropdown.AddOptions(options);
        }

        private void PopulateDayRes(int year, int month)
        {
            dayDropdown.ClearOptions();
            options.Clear();

            int days = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= days; i++)
            {
                options.Add(i.ToString());
            }

            dayDropdown.AddOptions(options);
        }

        private int PopulateYearRes()
        {
            String value = yearInputField.GetComponent<InputField>().text;

            int year = checkYear(value);
            return year;
        }

        private int checkYear(String year)
        {
            int val;
            try
            {
                val = int.Parse(year);
            }
            catch
            {
                val = DateTime.UtcNow.Year;
                setYearTextField(DateTime.UtcNow.Year.ToString());
            }
            return checkLimitYear(val);
        }

        private int checkLimitYear(int year)
        {
            if (year <= 1)
            {
                return 1;
            }
            else if (year >= 9999)
            {
                return 9999;
            }
            return year;
        }

        private void setYearTextField(String year)
        {
            yearInputField.GetComponent<InputField>().text = year;
        }

        /// <summary>
        /// Closes the date panel.
        /// </summary>
        public void closePanel()
        {
            datePanel.SetActive(false);
        }

        /// <summary>
        /// Submits the selected date and updates the game state.
        /// </summary>
        public void submit()
        {
            DateTime time = new DateTime(
                checkYear(yearInputField.text),
                monthDropdown.value + actualValue,
                dayDropdown.value + actualValue,
                hourDropdown.value,
                minuteDropdown.value,
                secondDropdown.value,
                DateTimeKind.Local);

            time = TimeZoneInfo.ConvertTimeToUtc(time);

            gameStateController.updateDate(time);
        }
    }
}
