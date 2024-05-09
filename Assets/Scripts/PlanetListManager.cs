using utils;
using System;
using UnityEngine;
using System.Collections.Generic;

public class PlanetListManager : MonoBehaviour
{
    private enum DataProperties
    {
        PlanetName = 0,
        PlanetMass,
        PlanetType,
        PlanetTemp,
        PlanetRadius,
        PlanetGrav,
        PlanetSpeed,
        PlanetMoonCount,
        PlanetRingPresence,
        PlanetPeriapsis,
        PlanetApoapsis,
        PlanetYear,
        PlanetDay
    }
    
    [SerializeField]
    private GameObject sun;
    
    [SerializeField]
    private Transform planetListContent;
    
    [SerializeField]
    private Transform canvas;

    [SerializeField]
    private GameObject planetObjectPrefab;
    
    [SerializeField]
    private GameObject planetInfoPrefab;

    [SerializeField]
    private List<Sprite> planetSprites;

    [SerializeField]
    private List<GameObject> planetModels;

    [SerializeField]
    private CameraControl cameraControl;

    
    private readonly Wrapper<GameObject> _activeInfoTab = new (null);

    private const string MoonDetector = "Rocky Moon";
    private const string DwarfDetector = "Dwarf Planet";
    
    private const string NullData = "null";
    private const string UnknownData = "good question!";
    
    private readonly List<string> _planetNames = new();

    
    // Start is called before the first frame update
    private void Start()
    {
        var dataFromCsv = CsvReader.ReadCsv("Assets/Data/PlanetData.csv");

        var planetProperties = LoadCsvDataIntoLocalArrays(dataFromCsv);
        
        if (planetSprites.Count != _planetNames.Count)
        {
            
        }
        else
        {
            for (var i = 0; i < _planetNames.Count; i++)
            {
                CreateNewPlanet(planetSprites[i], _planetNames[i], planetModels[i], planetProperties[i]);
            }
        }
    }

    private void CreateNewPlanet(Sprite planetSprite, string planetName, GameObject planetObject, Dictionary<string, string> staticProperties)
    {
        var planetListElement = Instantiate(planetObjectPrefab, planetListContent);
        var planetInfoTab = Instantiate(planetInfoPrefab, canvas);
        
        var planetListElementPrefabController = planetListElement.GetComponent<PlanetListElementPrefabController>();
        var planetInfoPrefabController = planetInfoTab.GetComponent<PlanetInfoPrefabController>();

        var planetInfoCloseButton = planetInfoPrefabController.CloseTabButton;

        Debug.Log(planetInfoCloseButton.name);

        var variableProperties = new Dictionary<string, Func<string>>()
        {
            {"Current Speed", () => planetObject.GetComponent<CelestialBody>().velocity.magnitude.ToString("n2")},
            {"Distance to Sun", () => (planetObject.transform.position - sun.transform.position).magnitude.ToString("n2")}
        };
        
        var planetDescription = 
            "The planet " + planetName + 
            " is a famous planet located in the Solar System Sol 12384905330, Galaxy Milky Way 49858456204852076205428, Universe 35904";
        
        Debug.Log(cameraControl);
        
        planetListElementPrefabController.SetPlanetInfo(planetSprite, planetName, planetObject, 
            cameraControl, planetInfoTab, _activeInfoTab, planetInfoCloseButton);
        
        planetInfoPrefabController.SetPlanetInfo(planetName, planetSprite,
            staticProperties, variableProperties, planetDescription);
        
        planetInfoTab.SetActive(false);
    }


    private List<Dictionary<string, string>> LoadCsvDataIntoLocalArrays(List<List<string>> data)
    {
        var result = new List<Dictionary<string, string>>();
        var labels = data[0];
        
        var rowCount = data.Count;
        
        for (var rowIndex = 1; rowIndex < rowCount; rowIndex++)
        {
            if (data[rowIndex][(int) DataProperties.PlanetType] != MoonDetector && 
                data[rowIndex][(int) DataProperties.PlanetType] != DwarfDetector)
            {
                var planetProperties = new Dictionary<string, string>();
                
                foreach (int property in Enum.GetValues(typeof(DataProperties)))
                {
                    if ((DataProperties) property == DataProperties.PlanetName)
                    {
                        _planetNames.Add(data[rowIndex][property]);
                    }
                    else if (data[rowIndex][property] == NullData) 
                    {
                        planetProperties.Add(labels[property], UnknownData);
                    }
                    else
                    {
                        planetProperties.Add(labels[property], data[rowIndex][property]);
                    }
                }
                
                result.Add(planetProperties);
            }
        }

        return result;
    }
}