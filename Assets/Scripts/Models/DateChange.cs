using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateChange : MonoBehaviour
{
    // year 0 to 9999
    public TMP_Dropdown dayDropdown;
    public TMP_Dropdown monthDropdown;
    public InputField yearInputField;
    //private List<Resolution> filteredResolutions = new List<Resolution>(); // List to keep unique resolutions

    // Start is called before the first frame update

    public GameObject datePanel;
       
    void Start()
    {
        PopulateMonth();
        //CameraControlV2.
    }

    // Update is called once per frame
    void Update()
    {
        int year = PopulateYearRes();
        int month = monthDropdown.GetComponent<TMP_Dropdown>().value + 1;

        PopulateDayRes(year, month);
    }
    void PopulateMonth()
    {
        monthDropdown.ClearOptions();
        List<string> options = new List<string>();
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

    void PopulateDayRes(int year, int month)
    {
        dayDropdown.ClearOptions();

        List<String> options = new List<String>();
        options.Clear();

        int days = DateTime.DaysInMonth(year, month);

        for (int i = 1; i <= days; i++)
        {
            options.Add(i.ToString());
        }

        dayDropdown.AddOptions(options);
    }

    int PopulateYearRes()
    {
        String value = yearInputField.GetComponent<InputField>().text;

        int year;
        if (int.TryParse(value, out year))
        {
            if (year <= 0)
            {
                year = 0;
                return 0;
            }
            else if (year >= 9999)
            {
                year = 9999;
                return 9999;
            } else if(year > 0 || year < 9999)
            {
                return year;
            }
        }
        else
        {
            Debug.Log("Bitte geben Sie eine gültige Zahl ein.");
        }

        return 2024;
    }

    public void closePanel()
    {
        datePanel.SetActive(false);
    }
}
