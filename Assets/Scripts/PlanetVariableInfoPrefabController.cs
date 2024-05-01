using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Extends the basic PlanetInfoPrefabController by implementing method GeneratePropertyDependingOnSubClass.
/// This specific implementation replaces Text Fields with Input Fields for certain properties, allowing
/// the user to tweak the description at will.
/// </summary>
public class PlanetVariableInfoPrefabController : PlanetInfoPrefabController
{
    /// <summary>
    /// Overrides the base method in class PlanetInfoPrefabController.
    /// Generates InputFields with a given property and returns the GameObject containing the information.
    /// </summary>
    /// 
    /// <param name="propertyName">The Name of the property to be initialised</param>
    /// <param name="propertyValue">The Value of the property to be initialised</param>
    /// 
    /// <returns>
    /// A GameObject with InputField functionalities.
    /// At the start the Object will have a name and a value for each property.
    /// The user will then be able to modify both and adapt them to their likings.
    /// </returns>
    private protected override GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue)
    {
        return GenerateInputField(propertyName, propertyValue);
    }

    /// <summary>
    /// Generates an Input Field GameObjet and initialises the text inside of it depending on the provided parameters.
    /// </summary>
    /// 
    /// <param name="propertyName">The Name of the property to be initialised</param>
    /// <param name="propertyValue">The Value of the property to be initialised</param>
    /// 
    /// <returns>
    /// The generated Input Field GameObject with the corresponding text
    /// </returns>
    private GameObject GenerateInputField(string propertyName, string propertyValue)
    {
        // You want to know how I managed to get this method to running?
        // Good question, I don't know either!
        var newListElement = TMP_DefaultControls.CreateInputField(new TMP_DefaultControls.Resources());
        
        var inputField = newListElement.GetComponent<TMP_InputField>();
        var inputSprite = newListElement.GetComponent<Image>();

        inputSprite.enabled = false;
        
        inputField.lineType = TMP_InputField.LineType.MultiLineSubmit;
        inputField.pointSize = propertiesTextSize;
        inputField.textComponent.color = Color.white;
        
        inputField.text = propertyName + NameValueSeparator + propertyValue;
        
        return newListElement;
    }
}
