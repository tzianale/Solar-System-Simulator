using UI;
using Utils;
using QuickOutline.Scripts;
using Models.PlanetListUtils;

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


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
            PlanetName,
            PlanetType,
            PlanetDescription
        }

        private const string PropertiesPath = "PlanetProperties.csv";
        private const string DescriptionsPath = "PlanetDescriptions.csv";

        private const string MoonDetector = "Rocky Moon";
        private const string DwarfDetector = "Dwarf Planet";

        private const string EarthsMoonName = "Earths Moon";
        
        private const string NullData = "null";
        private const string UnknownData = "good question!";

        [SerializeField]
        private float highlightWidth = 5f;

        [SerializeField]
        private Color highlightColor = Color.green;
        
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
        private CameraControlV2 cameraControl;

        [SerializeField]
        private bool allowPropertyEditing;

        [SerializeField]
        private Color highlightingColor;

        
        private readonly Wrapper<GameObject> _activeInfoTab = new (null);
        private readonly Wrapper<GameObject> _highlightedPlanet = new (null);
        
        private readonly List<string> _planetNames = new();

        private bool _highlightedPlanetOriginalEmissionEnabled;
        private Color _highlightedPlanetOriginalEmissionValue;
        
        /// <summary>
        /// Called on Script initialization, reads the data stored in the csv file and initialises List Elements and
        /// Info Tabs accordingly
        /// </summary>
        private void Start()
        {
            _highlightedPlanet.AddOnSetValueAction(HandlePlanetHighlighting);
            
            var propertiesData = CsvReader.ReadCsv(PropertiesPath);
            var descriptionsData = CsvReader.ReadCsv(DescriptionsPath);

            var planetProperties = UnpackPlanetDataFromCsv(propertiesData);
            var planetDescriptions = UnpackPlanetDescriptionsFromCsv(descriptionsData);
            
            if (planetSprites.Count == _planetNames.Count)
            {
                for (var i = 0; i < _planetNames.Count; i++)
                {
                    var currentPlanetModel = planetModels[i];

                    Dictionary<string, TwoObjectContainer<Func<string>, UnityAction<string>>> variableProperties = new();

                    if (allowPropertyEditing)
                    {
                        variableProperties = PlanetListDictionaries.GetVariablePropertiesDictionary(currentPlanetModel);
                    }

                    var liveStats = PlanetListDictionaries.GetLiveStatsDictionary(currentPlanetModel, sun);
                    
                    CreateNewPlanet(planetSprites[i], _planetNames[i], currentPlanetModel,
                        variableProperties,
                        planetProperties[i], 
                        liveStats, 
                        planetDescriptions[i]);
                }
            }
        }

        /// <summary>
        /// Turns off the highlighting effect on a deselected planet (oldPlanet) while turning it on for the newly
        /// selected one (newPlanet). Both values can be null without exceptions being thrown
        /// </summary>
        /// 
        /// <param name="oldPlanet">The planet that was highlighted previously (can be null)</param>
        /// <param name="newPlanet">The planet to highlight (can be null)</param>
        private void HandlePlanetHighlighting(GameObject oldPlanet, GameObject newPlanet)
        {
            if (oldPlanet)
            {
                var oldOutline = oldPlanet.GetComponent<Outline>();

                if (oldOutline)
                {
                    oldOutline.enabled = false;
                }
            }

            if (newPlanet)
            {
                var newRenderer = newPlanet.GetComponent<Outline>();
                    
                if (!newRenderer)
                {
                    newRenderer = newPlanet.AddComponent<Outline>();
                }
                
                newRenderer.OutlineColor = highlightColor;
                newRenderer.OutlineWidth = highlightWidth;
                newRenderer.enabled = true;
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
        /// <param name="variableProperties">
        /// The properties of a planet which will impact the simulation when (and if) changed
        /// </param>
        /// 
        /// <param name="staticProperties">
        /// The "fixed" properties of a planet, aka the ones that won't have to be changed dynamically.
        /// Example: Planet Surface Temperature
        /// </param>
        /// 
        /// <param name="liveStats">
        /// The "live" properties of a planet, aka the ones that will have to be changed dynamically.
        /// Example: Current Planet Speed
        /// </param>
        /// 
        /// <param name="planetDescription">
        /// A description about this planet
        /// </param>
        private void CreateNewPlanet(Sprite planetSprite, string planetName, GameObject planetObject, 
            Dictionary<string, TwoObjectContainer<Func<string>, UnityAction<string>>> variableProperties,
            Dictionary<string, string> staticProperties, 
            Dictionary<string, Func<string>> liveStats,
            string planetDescription)
        {
            var planetListElement = Instantiate(planetObjectPrefab, planetListContent);
            var planetInfoTab = Instantiate(planetInfoPrefab, canvas);
            
            var planetListElementPrefabController = planetListElement.GetComponent<PlanetListElementPrefabController>();
            var planetInfoPrefabController = planetInfoTab.GetComponent<PlanetInfoPrefabController>();

            var planetInfoCloseButton = planetInfoPrefabController.CloseTabButton;

            var onClickActions = new List<Action<int>>
            {
                clickCount => planetListElementPrefabController.HandleClickEvent(clickCount)
            };
            
            SetGameObjectOnClickBehaviour(planetObject, onClickActions);
            
            planetListElementPrefabController.SetPlanetInfo(
                planetSprite, planetName, planetObject, 
                cameraControl, 
                planetInfoTab, 
                _activeInfoTab,
                _highlightedPlanet,
                planetInfoCloseButton);
            
            planetInfoPrefabController.SetPlanetInfo(planetName, planetSprite, planetObject, cameraControl,
                variableProperties,
                staticProperties, 
                liveStats, 
                planetDescription);
            
            planetInfoTab.SetActive(false);
        }
        
        /// <summary>
        /// Sets up click behavior for a given GameObject and its children,
        /// by assigning a list of actions to be executed on click
        /// </summary>
        /// 
        /// <param name="planetObject">
        /// The GameObject to which the click behavior will be assigned
        /// </param>
        /// 
        /// <param name="actions">
        /// A list of actions to be executed when the GameObject or its children are clicked.
        /// Each action takes an integer parameter representing the click count
        /// </param>
        private static void SetGameObjectOnClickBehaviour(GameObject planetObject, List<Action<int>> actions)
        {
            var onClick = planetObject.AddComponent<OnGameObjectClick>();
            
            onClick.SetActions(actions);

            for (var childIndex = 0; childIndex < planetObject.transform.childCount; childIndex++)
            {
                var child = planetObject.transform.GetChild(childIndex).gameObject;
                
                onClick = child.AddComponent<OnGameObjectClick>();
            
                onClick.SetActions(actions);
            }
        }

        /// <summary>
        /// Takes the properties data that has been loaded from the memory and transforms it in an easily readable,
        /// planet separated list of Dictionaries
        /// </summary>
        /// 
        /// <param name="data">
        /// The "raw data" that has been loaded from the memory
        /// </param>
        /// 
        /// <returns>
        /// The refined list of Dictionaries - One dictionary for each body
        /// </returns>
        private List<Dictionary<string, string>> UnpackPlanetDataFromCsv(List<List<string>> data)
        {
            var result = new List<Dictionary<string, string>>();
            var labels = data[0];
            
            var rowCount = data.Count;
            
            for (var rowIndex = 1; rowIndex < rowCount; rowIndex++)
            {
                var row = data[rowIndex];
                
                if (IncludePlanetInfoInList(
                        row[(int) DataPropertyIndexes.PlanetName], 
                        row[(int) DataPropertyIndexes.PlanetType]))
                {
                    var planetProperties = new Dictionary<string, string>();
                    
                    for(var property = 0; property < row.Count; property++)
                    {
                        if ((DataPropertyIndexes) property == DataPropertyIndexes.PlanetName)
                        {
                            _planetNames.Add(row[property]);
                        }
                        else if (row[property] == NullData) 
                        {
                            planetProperties.Add(labels[property], UnknownData);
                        }
                        else
                        {
                            planetProperties.Add(labels[property], row[property]);
                        }
                    }
                    
                    result.Add(planetProperties);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts planet descriptions from a CSV data structure, currently excluding moons and dwarf planets
        /// </summary>
        /// 
        /// <param name="data">
        /// A list of lists, where each inner list represents a row in the CSV
        /// and each element in the inner list represents a cell in that row
        /// </param>
        /// 
        /// <returns>The list of planet descriptions.</returns>
        private List<string> UnpackPlanetDescriptionsFromCsv(List<List<string>> data)
        {
            var planetDescriptions = new List<string>();
            
            for (var planetIndex = 1; planetIndex < data.Count; planetIndex++)
            {
                var planet = data[planetIndex];
                
                if (!IncludePlanetInfoInList(
                        planet[(int) DataDescriptionIndexes.PlanetName],
                        planet[(int) DataDescriptionIndexes.PlanetType]))
                {
                    continue;
                }
                
                planetDescriptions.Add(planet[(int) DataDescriptionIndexes.PlanetDescription]);
            }

            return planetDescriptions;
        }


        /// <summary>
        /// Checks, depending on the given parameters, if the item from the csv file should be included
        /// in the planet list and info tabs
        /// </summary>
        /// 
        /// <param name="planetName">
        /// The name of the planet
        /// </param>
        /// 
        /// <param name="planetType">
        /// The type of the planet
        /// </param>
        /// 
        /// <returns>
        /// A boolean, excluding moon and dwarf planet data from the planet list.
        /// An exception is made for the earths moon when the simulation is in the sandbox mode
        /// </returns>
        private bool IncludePlanetInfoInList(string planetName, string planetType)
        {
            var isMoonOrDwarf = planetType.Equals(MoonDetector) || planetType.Equals(DwarfDetector);
            var isEarthsMoon = planetName.Equals(EarthsMoonName);
            var isSandboxMode = allowPropertyEditing;

            return !isMoonOrDwarf || (isEarthsMoon && isSandboxMode);
        }
        

        /// <summary>
        /// Adds a new item to the planet list based on the provided GameObject
        /// </summary>
        /// 
        /// <param name="planetObject">The GameObject representing the celestial body to be added</param>
        public void AddNewCelestialBody(GameObject planetObject)
        {
            Dictionary<string, TwoObjectContainer<Func<string>, UnityAction<string>>> variableProperties = new();

            if (allowPropertyEditing)
            {
                variableProperties = PlanetListDictionaries.GetVariablePropertiesDictionary(planetObject);
            }

            var liveStats = PlanetListDictionaries.GetLiveStatsDictionary(planetObject, sun);
            
            CreateNewPlanet(planetSprites[3], planetObject.name, planetObject, variableProperties, new Dictionary<string, string>(), liveStats, planetObject.name);
        }
    }
}