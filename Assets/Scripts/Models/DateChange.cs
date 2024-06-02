using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateChange : MonoBehaviour
{

    public TMP_Dropdown monthDropdown;
    //private List<Resolution> filteredResolutions = new List<Resolution>(); // List to keep unique resolutions

    // Start is called before the first frame update
    void Start()
    {
        PopulateResolutions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PopulateResolutions()
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
}
