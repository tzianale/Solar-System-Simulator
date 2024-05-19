using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Extends the basic PlanetInfoPrefabController by implementing method GeneratePropertyDependingOnSubClass.
/// This specific implementation uses Text Fields for the planet Properties, therefore giving the user reliable,
/// unmodifiable information.
/// </summary>
public class PlanetFinalInfoPrefabController : PlanetInfoPrefabController
{
    /// <summary>
    /// Overrides the base method in class PlanetInfoPrefabController.
    /// Generates Text Fields with a given property and returns the GameObject containing the information.
    /// </summary>
    /// 
    /// <param name="propertyName">The Name of the property to be initialised</param>
    /// <param name="propertyValue">The Value of the property to be initialised</param>
    /// 
    /// <returns>
    /// A GameObject containing the information.
    /// </returns>
    private protected override GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue)
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
}
