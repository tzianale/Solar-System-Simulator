using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetVariableInfoPrefabController : PlanetInfoPrefabController
{
    private protected override GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue)
    {
        var newListElement = GenerateInputField(propertyName, propertyValue);

        var contentSizeFitter = newListElement.AddComponent<ContentSizeFitter>();

        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        return newListElement;
    }

    private GameObject GenerateInputField(string propertyName, string propertyValue)
    {
        // You want to know how I managed to get this method to running?
        // Good question, I don't know either!
        var newListElement = TMP_DefaultControls.CreateInputField(new TMP_DefaultControls.Resources());
        
        
        
        return newListElement;
        
        var textComponent = newListElement.GetComponent<TextMeshProUGUI>();
        
        textComponent.text = propertyName + NameValueSeparator + propertyValue;
        textComponent.fontSize = propertiesTextSize;

        return newListElement;
    }
}
