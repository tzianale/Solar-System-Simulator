using TMPro;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Button = UnityEngine.UI.Button;

/// <summary>
/// Provides useful methods to control, set and handle each Planet Info Tab
/// </summary>
public abstract class PlanetInfoPrefabController : MonoBehaviour
{
    private protected const string UnknownPropertyText = "Unknown";
    private protected const string NameValueSeparator = " : ";
    
    [SerializeField] private protected TextMeshProUGUI planetNameField;

    [SerializeField] private protected GameObject planetSpriteField;

    [SerializeField] private protected GameObject planetStaticPropertiesContainer;

    [SerializeField] private protected GameObject planetVariablePropertiesContainer;

    [SerializeField] private protected int propertiesTextSize;
    
    [SerializeField] private protected TMP_InputField planetDescriptionContainer;

    [SerializeField] private Button closeTabButton;

    /// <summary>
    /// Getter method for the Button that closes this info tab
    /// </summary>
    public Button CloseTabButton { get => closeTabButton; }

    private readonly List<TextMeshProUGUI> _refreshableTextFields = new();
    
    private Dictionary<string, Func<string>> _variableProperties;
    
    /// <summary>
    /// Constructor-like method for initialising the fields of a Planet info tab
    /// </summary>
    /// 
    /// <param name="planetName">The name of the planet</param>
    /// <param name="planetSprite">The picture that will be used as icon for the planet</param>
    /// <param name="planetStaticProperties">The Dictionary of properties that won't need to be updated and their values</param>
    /// <param name="planetVariableProperties">The Dictionary of properties that will be updated and the methods to retrieve the updated data</param>
    /// <param name="planetDescription">The description of the planet</param>
    public void SetPlanetInfo(string planetName, Sprite planetSprite, 
        Dictionary<string, string> planetStaticProperties, 
        Dictionary<string, Func<string>> planetVariableProperties,
        string planetDescription)
    {
        planetNameField.text = planetName;
        planetSpriteField.GetComponent<Image>().sprite = planetSprite;
        
        _variableProperties = planetVariableProperties;

        foreach (var propertyField in GenerateStaticPropertiesList(planetStaticProperties))
        {
            propertyField.transform.SetParent(planetStaticPropertiesContainer.transform, false);
        }

        foreach (var propertyField in GenerateVariablePropertiesList(_variableProperties))
        {
            propertyField.transform.SetParent(planetVariablePropertiesContainer.transform, false);
        }

        planetDescriptionContainer.text = planetDescription;
    }
    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        RefreshVariableInfo();
    }

    /// <summary>
    /// Calls each of the methods provided by the planetVariableProperties Dictionary, actualising the stored data
    /// </summary>
    private void RefreshVariableInfo()
    {
        var newVariablePropertiesList = GenerateVariablePropertiesValues();

        for (var propertyIndex = 0; propertyIndex < _refreshableTextFields.Count; propertyIndex++)
        {
            _refreshableTextFields[propertyIndex].text = newVariablePropertiesList[propertyIndex];
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
            var propertyValue = UnknownPropertyText;

            if (elementIndex < propertiesNamesCount) propertyName = planetPropertiesNames.ElementAt(elementIndex);
            if (elementIndex < propertiesValuesCount) propertyValue = planetPropertiesValues.ElementAt(elementIndex);

            resultList.Add(GeneratePropertyDependingOnSubClass(propertyName, propertyValue));
        }

        return resultList;
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
    private List<GameObject> GenerateVariablePropertiesList(Dictionary<string, Func<string>> planetProperties)
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
    private List<string> GenerateVariablePropertiesValues()
    {
        var properties = new List<string>();

        foreach (var property in _variableProperties)
        {
            var propertyName = property.Key;
            var propertyValue = property.Value();
            
            properties.Add(propertyName + NameValueSeparator + propertyValue);
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

        _refreshableTextFields.Add(textElement);
        
        return newListElement;
    }

    /// <summary>
    /// Overridable method, gives the possibility to generate either TextFields or InputFields depending
    /// on the subclass implementation.
    /// </summary>
    /// 
    /// <param name="propertyName">The Name of the property to be initialised</param>
    /// <param name="propertyValue">The Value of the property to be initialised</param>
    /// 
    /// <returns>
    /// A GameObject containing the property information.
    /// </returns>
    private protected abstract GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue);

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
