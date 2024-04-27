using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetFinalInfoPrefabController : PlanetInfoPrefabController
{
    private protected override GameObject GeneratePropertyDependingOnSubClass(string propertyName, string propertyValue)
    {
        var newListElement = new GameObject(propertyName + " Property");

        var contentSizeFitter = newListElement.AddComponent<ContentSizeFitter>();

        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        Debug.Log("Why am I here?");
        
        var textElement = newListElement.AddComponent<TextMeshProUGUI>();
        
        textElement.text = propertyName + NameValueSeparator + propertyValue;
        textElement.fontSize = propertiesTextSize;

        return newListElement;
    }
}
