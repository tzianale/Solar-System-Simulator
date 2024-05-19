using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Utils;

namespace Models
{
    /// <summary>
    /// Handles the Planet List by creating new Planet List Elements as well as Planet Info Tabs
    /// </summary>
    public class PlanetListManager : MonoBehaviour
    {
        /// <summary>
        /// Stores the relevant indexes of the Planet Properties that are saved in the Planet Properties csv file
        /// </summary>
        private enum DataPropertyIndexes
        {
            PlanetName = 0,
            PlanetType = 2
        }
    
        /// <summary>
        /// Stores the relevant indexes of the Planet Descriptions that are saved in the Planet Descriptions csv file
        /// </summary>
        private enum DataDescriptionIndexes
        {
            PlanetType = 1,
            PlanetDescription
        }

        private const string PropertiesPath = "Assets/Data/PlanetProperties.csv";
        private const string DescriptionsPath = "Assets/Data/PlanetDescriptions.csv";
    
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
            var propertiesData = CsvReader.ReadCsv(PropertiesPath);
            var descriptionsData = CsvReader.ReadCsv(DescriptionsPath);

            var planetProperties = LoadCsvPropertiesIntoLocalDictionaries(propertiesData);
            var planetDescriptions = UnpackPlanetDescriptionsFromCsv(descriptionsData);
        
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
        
                    CreateNewPlanet(planetSprites[i], _planetNames[i], currentPlanetModel, planetProperties[i], 
                        variableProperties, planetDescriptions[i]);
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
        /// Takes the properties data that has been loaded from the memory and transforms it in a easily readable, planet separated
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
        private List<Dictionary<string, string>> LoadCsvPropertiesIntoLocalDictionaries(List<List<string>> data)
        {
            var result = new List<Dictionary<string, string>>();
            var labels = data[0];
        
            var rowCount = data.Count;
        
            for (var rowIndex = 1; rowIndex < rowCount; rowIndex++)
            {
                if (data[rowIndex][(int) DataPropertyIndexes.PlanetType] != MoonDetector && 
                    data[rowIndex][(int) DataPropertyIndexes.PlanetType] != DwarfDetector)
                {
                    var planetProperties = new Dictionary<string, string>();
                
                    for(var property = 0; property < data[rowIndex].Count; property++)
                    {
                        if ((DataPropertyIndexes) property == DataPropertyIndexes.PlanetName)
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


        private List<string> UnpackPlanetDescriptionsFromCsv(List<List<string>> data)
        {
            var planetDescriptions = new List<string>();
        
            for (int planetIndex = 1; planetIndex < data.Count; planetIndex++)
            {
                var planet = data[planetIndex];
            
                if (planet[(int)DataDescriptionIndexes.PlanetType] == MoonDetector ||
                    planet[(int)DataDescriptionIndexes.PlanetType] == DwarfDetector)
                {
                    continue;
                }
            
                planetDescriptions.Add(planet[(int) DataDescriptionIndexes.PlanetDescription]);
            }

            return planetDescriptions;
        }
    }
}