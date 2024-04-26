using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlanetInfoPrefabController : MonoBehaviour
{
    private const string UnknownPropertyText = "Unknown";
    private const string NameValueSeparator = " : ";
    
    [SerializeField] private TextMeshProUGUI planetNameField;

    [SerializeField] private GameObject planetSpriteField;

    [SerializeField] private GameObject planetStaticPropertiesContainer;

    [SerializeField] private GameObject planetVariablePropertiesContainer;

    [SerializeField] private TextMeshProUGUI planetDescriptionContainer;

    [SerializeField] private float propertiesTextSize;

    private GameObject _linkedPlanet;
    private GameObject _linkedSun;

    public void SetPlanetInfo(string planetName, Sprite planetSprite, 
        IEnumerable<string> planetPropertiesNames, IEnumerable<string> planetPropertiesValues, 
        GameObject planetObject, GameObject sunObject, string planetDescription)
    {
        planetNameField.text = planetName;
        planetSpriteField.GetComponent<Image>().sprite = planetSprite;
        
        _linkedPlanet = planetObject;
        _linkedSun = sunObject;

        foreach (var propertyField in GenerateStaticPropertiesList(planetPropertiesNames, planetPropertiesValues))
        {
            propertyField.transform.SetParent(planetStaticPropertiesContainer.transform, false);
        }

        foreach (var propertyField in GenerateVariablePropertiesList())
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

        for (int propertyIndex = 0; propertyIndex < planetVariablePropertiesContainer.transform.childCount; propertyIndex++)
        {
            var textElement = planetVariablePropertiesContainer.transform.GetChild(propertyIndex).GetComponent<TextMeshProUGUI>();
        
            textElement.text = newVariablePropertiesList[propertyIndex];
        }
    }


    private List<GameObject> GenerateStaticPropertiesList(IEnumerable<string> planetPropertiesNames, 
        IEnumerable<string> planetPropertiesValues)
    {
        planetPropertiesNames = planetPropertiesNames.ToArray();
        planetPropertiesValues = planetPropertiesValues.ToArray();

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

            resultList.Add(GenerateProperty(propertyName, propertyValue));
        }

        return resultList;
    }


    private List<GameObject> GenerateVariablePropertiesList()
    {
        var resultList = new List<GameObject>();
        var bodySpeed = _linkedPlanet.GetComponent<CelestialBody>().velocity.magnitude.ToString("n2");
        
        resultList.Add(GenerateProperty("Speed", bodySpeed));

        var distanceToSun = _linkedPlanet.transform.position - _linkedSun.transform.position;
        
        resultList.Add(GenerateProperty("Distance to Sun", distanceToSun.magnitude.ToString("n2")));

        return resultList;
    }


    private List<string> GenerateVariablePropertiesValues()
    {
        var resultList = new List<string>();
        
        resultList.Add("Speed" + NameValueSeparator + _linkedPlanet.GetComponent<CelestialBody>().velocity.magnitude);

        var distanceToSun = _linkedPlanet.transform.position - _linkedSun.transform.position;
        
        resultList.Add("Distance to Sun" + NameValueSeparator + distanceToSun.magnitude);

        return resultList;
    }


    private GameObject GenerateProperty(string propertyName, string propertyValue)
    {
        var newListElement = new GameObject(propertyName + " Property");

        var contentSizeFitter = newListElement.AddComponent<ContentSizeFitter>();

        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
        var textElement = newListElement.AddComponent<TextMeshProUGUI>();
        
        textElement.text = propertyName + NameValueSeparator + propertyValue;
        textElement.fontSize = propertiesTextSize;

        return newListElement;
    }


    private static int GetMaxInt(int firstNumber, int secondNumber)
    {
        return (firstNumber >= secondNumber) ? firstNumber : secondNumber;
    }
}
