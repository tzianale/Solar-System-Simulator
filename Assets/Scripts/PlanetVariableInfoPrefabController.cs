using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetVariableInfoPrefabController : PlanetInfoPrefabController
{
    private protected override GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue)
    {
        var newListElement = GenerateInputField(propertyName, propertyValue);

        return newListElement;
    }

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
