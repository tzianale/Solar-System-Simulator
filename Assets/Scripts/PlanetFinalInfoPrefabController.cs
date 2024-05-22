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
    [SerializeField] private protected TextMeshProUGUI planetDescriptionContainer;

    /// <summary>
    /// Sets a description string to the Info Prefab
    /// </summary>
    /// 
    /// <param name="planetDescription">
    /// The string to set as Planet Description
    /// </param>
    protected override void SetDescription(string planetDescription)
    {
        planetDescriptionContainer.SetText(planetDescription);
    }

    /// <summary>
    /// Overrides the base method in class PlanetInfoPrefabController.
    /// Generates Text Fields with a given property and returns the GameObject containing the information.
    /// </summary>
    /// 
    /// <param name="propertyName">The Name of the property to be initialised</param>
    /// <param name="propertyValue">The Value of the property to be initialised</param>
    /// <param name="measurementUnit">The Unit of the property to be initialised</param> 
    /// 
    /// <returns>
    /// A GameObject containing the information.
    /// </returns>
    private protected override GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue, string measurementUnit)
    {
        var newListElement = new GameObject(propertyName + " Property");

        var contentSizeFitter = newListElement.AddComponent<ContentSizeFitter>();

        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        var textElement = newListElement.AddComponent<TextMeshProUGUI>();
        
        textElement.text = propertyName + NameValueSeparator + propertyValue + ValueUnitSeparatorForProperties + measurementUnit;
        textElement.fontSize = propertiesTextSize;

        return newListElement;
    }
}
