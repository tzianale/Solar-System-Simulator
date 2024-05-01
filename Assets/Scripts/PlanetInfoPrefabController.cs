using TMPro;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class PlanetInfoPrefabController : MonoBehaviour
{
    private const string UnknownPropertyText = "Unknown";
    private protected const string NameValueSeparator = " : ";
    
    [SerializeField] private protected TextMeshProUGUI planetNameField;

    [SerializeField] private protected GameObject planetSpriteField;

    [SerializeField] private protected GameObject planetStaticPropertiesContainer;

    [SerializeField] private protected GameObject planetVariablePropertiesContainer;

    [SerializeField] private protected int propertiesTextSize;
    
    [SerializeField] private protected TextMeshProUGUI planetDescriptionContainer;

    private readonly List<TextMeshProUGUI> _refreshableTextFields = new();
    
    private Dictionary<string, Func<string>> _variableProperties;

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


    private void Update()
    {
        RefreshVariableInfo();
    }


    private void RefreshVariableInfo()
    {
        var newVariablePropertiesList = GenerateVariablePropertiesValues();

        for (var propertyIndex = 0; propertyIndex < _refreshableTextFields.Count; propertyIndex++)
        {
            _refreshableTextFields[propertyIndex].text = newVariablePropertiesList[propertyIndex];
        }
    }


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

    private List<GameObject> GenerateVariablePropertiesList(Dictionary<string, Func<string>> properties)
    {
        var resultList = new List<GameObject>();

        foreach (var property in properties)
        {
            resultList.Add(GeneratePropertySubClassIndependently(property.Key, property.Value()));
        }
        
        return resultList;
    }
    
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

    private protected abstract GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue);


    private static int GetMaxInt(int firstNumber, int secondNumber)
    {
        return (firstNumber >= secondNumber) ? firstNumber : secondNumber;
    }
}
