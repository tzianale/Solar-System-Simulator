using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> filteredResolutions = new List<Resolution>(); // List to keep unique resolutions

    void Start()
    {
        PopulateResolutions();
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
    }

    void PopulateResolutions()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        Resolution currentResolution = Screen.currentResolution;
        filteredResolutions.Clear();

        foreach (Resolution res in Screen.resolutions)
        {
            string option = res.width + "x" + res.height;
            if (!options.Contains(option))
            {
                options.Add(option);
                filteredResolutions.Add(res);
            }
        }

        resolutionDropdown.AddOptions(options);
    }

    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
