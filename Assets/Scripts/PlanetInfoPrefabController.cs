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

    [SerializeField] private GameObject planetPropertiesContainer;

    [SerializeField] private TextMeshProUGUI planetDescriptionContainer;

    [SerializeField] private float propertiesTextSize;


    public void SetPlanetInfo(string planetName, Sprite planetSprite, IEnumerable<string> planetPropertiesNames, IEnumerable<string> planetPropertiesValues, string planetDescription)
    {
        planetNameField.text = planetName;
        planetSpriteField.GetComponent<Image>().sprite = planetSprite;

        foreach (var childWithPropertyToAdd in GeneratePropertiesList(planetPropertiesNames, planetPropertiesValues))
        {
            childWithPropertyToAdd.transform.SetParent(planetPropertiesContainer.transform, false);
        }

        planetDescriptionContainer.text = planetDescription;
    }


    private List<GameObject> GeneratePropertiesList(IEnumerable<string> planetPropertiesNames, IEnumerable<string> planetPropertiesValues)
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
            
            var newListElement = new GameObject("Property " + elementIndex);

            var contentSizeFitter = newListElement.AddComponent<ContentSizeFitter>();

            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            var textElement = newListElement.AddComponent<TextMeshProUGUI>();
            
            textElement.text = propertyName + NameValueSeparator + propertyValue;
            textElement.fontSize = propertiesTextSize;
            
            resultList.Add(newListElement);
        }

        return resultList;
    }


    private static int GetMaxInt(int firstNumber, int secondNumber)
    {
        return (firstNumber >= secondNumber) ? firstNumber : secondNumber;
    }
}
