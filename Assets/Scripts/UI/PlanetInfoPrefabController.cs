using TMPro;
using Utils;
using Models;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

using SystemObject = System.Object;
using Button = UnityEngine.UI.Button;

namespace UI
{
    /// <summary>
    /// Provides useful methods to control, set and handle each Planet Info Tab
    /// </summary>
    public abstract class PlanetInfoPrefabController : MonoBehaviour
    {
        private protected const string UnknownPropertyText = "Unknown";
        private protected const string NameValueSeparator = " : ";
        private protected const string ValueUnitSeparatorForProperties = " ";
        
        private const string ValueUnitSeparatorForRawData = "_";
        
        [SerializeField] private protected GameObject variablePropertiesPrefab;
        
        
        [SerializeField] private protected TextMeshProUGUI planetNameField;

        [SerializeField] private protected GameObject planetSpriteField;

        [SerializeField] private Button hidePlanetButton;

        [SerializeField] private protected GameObject planetVariablePropertiesTag;
        [SerializeField] private protected GameObject planetVariablePropertiesContainer;

        [SerializeField] private protected GameObject planetStaticPropertiesContainer;

        [SerializeField] private protected GameObject planetLiveStatsContainer;

        [SerializeField] private protected int propertiesTextSize;

        [SerializeField] private Button closeTabButton;
        

        /// <summary>
        /// Getter method for the Button that closes this info tab
        /// </summary>
        public Button CloseTabButton => closeTabButton;

        private readonly List<ObservablePropertyController> _refreshableVariableTextFields = new();
        private readonly List<TextMeshProUGUI> _refreshableLiveTextFields = new();

        protected CameraControlV2 CameraControl;
        
        private Dictionary<string, TwoObjectContainer<Func<string>, UnityAction<string>>> _variableProperties;
        
        private Dictionary<string, Func<string>> _liveStats;

        private bool _planetActive = true;
        
        /// <summary>
        /// Constructor-like method for initialising the fields of a Planet info tab
        /// </summary>
        /// 
        /// <param name="planetName">The name of the planet</param>
        /// <param name="planetSprite">The picture that will be used as icon for the planet</param>
        /// <param name="planetObject">The GameObject associated with the planet</param>
        /// <param name="cameraControl">The CameraControl script to lock when the properties are being edited</param>
        /// <param name="variableProperties">The Dictionary of properties that will update the simulation when changed by the user</param>
        /// <param name="planetStaticProperties">The Dictionary of properties that won't need to be updated and their values</param>
        /// <param name="planetLiveStats">The Dictionary of properties that will be updated and the methods to retrieve the updated data</param>
        /// <param name="planetDescription">The description of the planet</param>
        public void SetPlanetInfo(string planetName, Sprite planetSprite, GameObject planetObject, CameraControlV2 cameraControl,
            Dictionary<string, TwoObjectContainer<Func<string>, UnityAction<string>>> variableProperties,
            Dictionary<string, string> planetStaticProperties, 
            Dictionary<string, Func<string>> planetLiveStats,
            string planetDescription)
        {
            planetNameField.text = planetName;
            planetSpriteField.GetComponent<Image>().sprite = planetSprite;

            CameraControl = cameraControl;

            _variableProperties = variableProperties;
            _liveStats = planetLiveStats;
            
            hidePlanetButton.onClick.AddListener(() =>
            {
                _planetActive = !_planetActive;
                planetObject.SetActive(_planetActive);
            });
            
            foreach (var propertyField in GenerateVariablePropertiesList())
            {
                propertyField.transform.SetParent(planetVariablePropertiesContainer.transform, false);
            }

            foreach (var propertyField in GenerateStaticPropertiesList(planetStaticProperties))
            {
                propertyField.transform.SetParent(planetStaticPropertiesContainer.transform, false);
            }

            foreach (var propertyField in GenerateLiveStatsList(_liveStats))
            {
                propertyField.transform.SetParent(planetLiveStatsContainer.transform, false);
            }

            SetDescription(planetDescription);
        }

        
        /// <summary>
        /// Generates the list of properties that, when changed, will influence the simulation (variable Properties)
        /// The method also assigns the observers and initialises the data
        /// </summary>
        /// 
        /// <returns>
        /// A list of GameObjects, which contain the various Properties Fields, ready to be added under the variable properties tab
        /// </returns>
        private List<GameObject> GenerateVariablePropertiesList()
        {
            var resultList = new List<GameObject>();
            
            if (_variableProperties.Count == 0)
            {
                planetVariablePropertiesTag.SetActive(false);
                planetVariablePropertiesContainer.SetActive(false);
            }
            else
            {
                foreach (var variableProperty in _variableProperties)
                {
                    var variablePropertyGameObject = Instantiate(variablePropertiesPrefab);

                    var variablePropertyController = variablePropertyGameObject.GetComponent<ObservablePropertyController>();

                    var propertyName = variableProperty.Key;
                    var propertyValueAndUnit = SeparateValueAndUnit(variableProperty.Value.FirstObject.Invoke());
                    
                    var propertyValue = propertyValueAndUnit.FirstObject;
                    var propertyUnit = propertyValueAndUnit.SecondObject;
                    
                    var propertyText = GenerateObservablePropertyText(propertyName, propertyValue, propertyUnit);

                    variablePropertyController.SetText(propertyText);
                    
                    variablePropertyController.AddListenerToAllOnSelection(_ => CameraControl.SetKeyboardLock(true));
                    variablePropertyController.AddListenerToAllOnEditEnd(_ => CameraControl.SetKeyboardLock(false));
                    
                    variablePropertyController.AddListenerToPropertyValueEditEnd(variableProperty.Value.SecondObject);

                    resultList.Add(variablePropertyGameObject);
                    _refreshableVariableTextFields.Add(variablePropertyController);
                }
            }

            return resultList;
        }


        private static SystemObject[] GenerateObservablePropertyText(string propertyName, string propertyValue, string propertyUnit)
        {
            return new SystemObject[] { propertyName + NameValueSeparator, propertyValue, ValueUnitSeparatorForProperties + propertyUnit};
        }

        /// <summary>
        /// Sets the description of this planet
        /// </summary>
        /// 
        /// <param name="planetDescription">The string that will be used as description</param>
        protected abstract void SetDescription(string planetDescription);
        
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            RefreshLiveInfo();
            RefreshVariableInfo();
        }

        /// <summary>
        /// Calls each of the methods provided by the planetLiveStats Dictionary, actualising the stored data
        /// </summary>
        private void RefreshLiveInfo()
        {
            var newLiveStatsList = GenerateLiveStatsValues();

            for (var propertyIndex = 0; propertyIndex < _refreshableLiveTextFields.Count; propertyIndex++)
            {
                _refreshableLiveTextFields[propertyIndex].text = newLiveStatsList[propertyIndex];
            }
        }

        /// <summary>
        /// Calls each of the methods provided by the planetLiveStats Dictionary, actualising the stored data
        /// </summary>
        private void RefreshVariableInfo()
        {
            var newLiveStatsList = GenerateVariableStatsValues();

            for (var propertyIndex = 0; propertyIndex < _refreshableVariableTextFields.Count; propertyIndex++)
            {
                var currentController = _refreshableVariableTextFields[propertyIndex];

                if (!currentController.IsBeingEdited)
                {
                    currentController.SetText(newLiveStatsList[propertyIndex]);
                }
            }
        }

        /// <summary>
        /// Generate a list of GameObjects containing the various static properties of the planet
        /// depending on the Dictionary given at the application start
        /// </summary>
        /// 
        /// <param name="planetProperties">A dictionary containing the static properties (name and value) of the planet</param>
        /// 
        /// <returns>
        /// Returns a list of GameObjects to append as children of the Static Properties Container.
        /// In practice, it can either be a list of TextFields of one of InputFields, depending on the subclass
        /// </returns>
        private List<GameObject> GenerateStaticPropertiesList(Dictionary<string, string> planetProperties)
        {
            var planetPropertiesNames = planetProperties.Keys.ToArray();
            var planetPropertiesValues = planetProperties.Values.ToArray();

            var resultList = new List<GameObject>();
            
            var propertiesNamesCount = planetPropertiesNames.Count();
            var propertiesValuesCount = planetPropertiesValues.Count();
            
            var elementCount = GetMaxInt(propertiesNamesCount, propertiesValuesCount);

            for (var elementIndex = 0; elementIndex < elementCount; elementIndex++)
            {
                var propertyName = UnknownPropertyText;
                var propertyValueAndUnit = UnknownPropertyText;

                if (elementIndex < propertiesNamesCount) propertyName = planetPropertiesNames.ElementAt(elementIndex);
                if (elementIndex < propertiesValuesCount) propertyValueAndUnit = planetPropertiesValues.ElementAt(elementIndex);

                var propertyValueAndUnitSeparated = SeparateValueAndUnit(propertyValueAndUnit);

                var propertyValue = propertyValueAndUnitSeparated.FirstObject;
                var propertyUnit = propertyValueAndUnitSeparated.SecondObject;

                resultList.Add(GeneratePropertyDependingOnSubClass(propertyName, propertyValue, propertyUnit));
            }

            return resultList;
        }


        /// <summary>
        /// Takes a String input from the csv Planet Properties file and separates values and their (eventual) measurement units
        /// </summary>
        /// 
        /// <param name="inputString">
        /// The string with the data to separate. As separator a constant is used,
        /// which is defined in this script as "ValueUnitSeparatorForRawData"
        /// </param>
        /// 
        /// <returns>
        /// A TwoObjectContainer, with the "value" as first object and the measurement unit as second
        /// </returns>
        private static TwoObjectContainer<string, string> SeparateValueAndUnit(string inputString)
        {
            var propertyUnit = "";

            var valueAndUnitSeparated = inputString.Split(ValueUnitSeparatorForRawData);
                
            var propertyValue = valueAndUnitSeparated[0];

            if (valueAndUnitSeparated.Length > 1)
            { 
                propertyUnit = valueAndUnitSeparated[1];
            }

            return new TwoObjectContainer<string, string>(propertyValue, propertyUnit);
        }
        

        /// <summary>
        /// Generate a list of GameObjects containing the various variable properties of the planet
        /// depending on the Dictionary given at the application start
        /// </summary>
        /// 
        /// <param name="planetProperties">A dictionary containing the variable properties (name and function) of the planet</param>
        /// 
        /// <returns>
        /// Returns a list of GameObjects to append as children of the Variable Properties Container.
        /// Properties will be automatically updated at each Update() method call
        /// </returns>
        private List<GameObject> GenerateLiveStatsList(Dictionary<string, Func<string>> planetProperties)
        {
            var resultList = new List<GameObject>();

            foreach (var property in planetProperties)
            {
                resultList.Add(GeneratePropertySubClassIndependently(property.Key, property.Value()));
            }
            
            return resultList;
        }
        
        /// <summary>
        /// In order to avoid regenerating GameObjects for the Variable Properties List,
        /// the following method will generate a list of strings to update the values.
        /// </summary>
        /// 
        /// <returns>
        /// A list of strings to replace the outdated variable properties values
        /// </returns>
        private List<string> GenerateLiveStatsValues()
        {
            var properties = new List<string>();

            foreach (var property in _liveStats)
            {
                var propertyName = property.Key;
                var propertyValue = property.Value();
                
                properties.Add(propertyName + NameValueSeparator + propertyValue);
            }

            return properties;
        }
        
        /// <summary>
        /// In order to avoid regenerating GameObjects for the Observable Properties List,
        /// the following method will generate a list of strings to update the values.
        /// </summary>
        /// 
        /// <returns>
        /// A list of strings to replace the outdated observable properties values
        /// </returns>
        private List<SystemObject[]> GenerateVariableStatsValues()
        {
            var properties = new List<SystemObject[]>();

            foreach (var property in _variableProperties)
            {
                var propertyName = property.Key;
                var propertyRawValue = property.Value.FirstObject.Invoke();

                var propertyValueAndUnit = SeparateValueAndUnit(propertyRawValue);

                var propertyValue = propertyValueAndUnit.FirstObject;
                var propertyUnit = propertyValueAndUnit.SecondObject;
                
                properties.Add(GenerateObservablePropertyText(propertyName, propertyValue, propertyUnit));
            }

            return properties;
        }
        
        /// <summary>
        /// Generates a Property Game Object no matter what the subclass is.
        /// The game object contains the text provided in the parameters and is sized correctly vertically and horizontally
        /// </summary>
        /// 
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyValue">The value of the property</param>
        /// 
        /// <returns>
        /// A TextField GameObject containing the property information
        /// </returns>
        private GameObject GeneratePropertySubClassIndependently(string propertyName, string propertyValue)
        {
            var newListElement = new GameObject(propertyName + " Property");

            var contentSizeFitter = newListElement.AddComponent<ContentSizeFitter>();

            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                
            var textElement = newListElement.AddComponent<TextMeshProUGUI>();
            
            textElement.text = propertyName + NameValueSeparator + propertyValue;
            textElement.fontSize = propertiesTextSize;

            _refreshableLiveTextFields.Add(textElement);
            
            return newListElement;
        }

        /// <summary>
        /// Overridable method, gives the possibility to generate either TextFields or InputFields depending
        /// on the subclass implementation.
        /// </summary>
        /// 
        /// <param name="propertyName">The Name of the property to be initialised</param>
        /// <param name="propertyValue">The Value of the property to be initialised</param>
        /// <param name="measurementUnit">The Unit of the property to be initialised</param>
        /// 
        /// <returns>
        /// A GameObject containing the property information.
        /// </returns>
        private protected abstract GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue, string measurementUnit);

        /// <summary>
        /// Simple utility method to get the maximum value between two integers
        /// </summary>
        /// 
        /// <param name="firstNumber">The first number</param>
        /// <param name="secondNumber">The second number</param>
        /// 
        /// <returns>
        /// The number that, out of the two, has the greater value
        /// </returns>
        private static int GetMaxInt(int firstNumber, int secondNumber)
        {
            return (firstNumber >= secondNumber) ? firstNumber : secondNumber;
        }
    }
}