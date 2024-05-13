using utils;
using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles the Planet List by creating new Planet List Elements as well as Planet Info Tabs
/// </summary>
public class PlanetListManager : MonoBehaviour
{
    /// <summary>
    /// Stores the indexes of the Planet Properties that are saved in the Planet Data csv file
    /// </summary>
    private enum DataProperties
    {
        PlanetName = 0,
        PlanetType = 2
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

    
    /// <summary>
    /// Called on Script initialization, reads the data stored in the csv file and initialises List Elements and
    /// Info Tabs accordingly
    /// </summary>
    private void Start()
    {
        var dataFromCsv = CsvReader.ReadCsv("Assets/Data/PlanetData.csv");

        var planetProperties = LoadCsvDataIntoLocalDictionaries(dataFromCsv);
        
        if (planetSprites.Count != _planetNames.Count)
        {
            
        }
        else
        {
            for (var i = 0; i < _planetNames.Count; i++)
            {
                var currentPlanetModel = planetModels[i];
                
                var variableProperties = new Dictionary<string, Func<string>>()
                {
                    {"Current Speed", () => currentPlanetModel.GetComponent<CelestialBody>().velocity.magnitude.ToString("n2")},
                    {"Distance to Sun", () => (currentPlanetModel.transform.position - sun.transform.position).magnitude.ToString("n2")}
                };
        
                var planetDescription = 
                    "The planet " + _planetNames[i] + 
                    " is a famous planet located in the Solar System Sol 1234, Galaxy Milky Way 498, Universe 35, main branch";
                
                CreateNewPlanet(planetSprites[i], _planetNames[i], planetModels[i], planetProperties[i], 
                    variableProperties, planetDescription);
            }
        }
    }

    /// <summary>
    /// Adds a new Planet to the Planet List, as well as creating the corresponding info tab
    /// </summary>
    /// 
    /// <param name="planetSprite">
    /// The Image that will be associated to this planet
    /// </param>
    /// 
    /// <param name="planetName">
    /// The name of this planet
    /// </param>
    /// 
    /// <param name="planetObject">
    /// The GameObject that represents the planet in the simulation
    /// </param>
    /// 
    /// <param name="staticProperties">
    /// The "fixed" properties of a planet, aka the ones that won't have to be changed dynamically.
    /// Example: Planet Mass
    /// </param>
    /// 
    /// <param name="variableProperties">
    /// The "variable" properties of a planet, aka the ones that will have to be changed dynamically.
    /// Example: Planet Speed
    /// </param>
    /// 
    /// <param name="planetDescription">
    /// A description about this planet
    /// </param>
    private void CreateNewPlanet(Sprite planetSprite, string planetName, GameObject planetObject, 
        Dictionary<string, string> staticProperties, Dictionary<string, Func<string>> variableProperties,
        string planetDescription)
    {
        var planetListElement = Instantiate(planetObjectPrefab, planetListContent);
        var planetInfoTab = Instantiate(planetInfoPrefab, canvas);
        
        var planetListElementPrefabController = planetListElement.GetComponent<PlanetListElementPrefabController>();
        var planetInfoPrefabController = planetInfoTab.GetComponent<PlanetInfoPrefabController>();

        var planetInfoCloseButton = planetInfoPrefabController.CloseTabButton;

        Debug.Log(planetInfoCloseButton.name);
        
        Debug.Log(cameraControl);
        
        planetListElementPrefabController.SetPlanetInfo(planetSprite, planetName, planetObject, 
            cameraControl, planetInfoTab, _activeInfoTab, planetInfoCloseButton);
        
        planetInfoPrefabController.SetPlanetInfo(planetName, planetSprite,
            staticProperties, variableProperties, planetDescription);
        
        planetInfoTab.SetActive(false);
    }

    /// <summary>
    /// Takes the data that has been loaded from the memory and transforms it in a easily readable, planet separated
    /// list of Dictionaries
    /// </summary>
    /// 
    /// <param name="data">
    /// The "raw data" that has been loaded from the memory
    /// </param>
    /// 
    /// <returns>
    /// The refined list of Dictionaries - One dictionary for each body
    /// </returns>
    private List<Dictionary<string, string>> LoadCsvDataIntoLocalDictionaries(List<List<string>> data)
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
                
                for(var property = 0; property < data[rowIndex].Count; property++)
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