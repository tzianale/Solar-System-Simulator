using TMPro;
using UnityEngine;
using SystemObject = System.Object;

/// <summary>
/// Controller Script for Prefab "Editable Property Prefab"
/// </summary>
public class EditablePropertyController : PropertyFieldController
{
    [SerializeField] private TMP_InputField planetPropertyText;
    
    /// <summary>
    /// Allows external scripts to edit the text inside the InputField by code
    /// </summary>
    /// 
    /// <param name="text">
    /// The new text that should be stored in the InputField
    /// </param>
    public override void SetText(SystemObject[] text)
    {
        var stringToDisplay = "";

        foreach (var stringPiece in text)
        {
            stringToDisplay += stringPiece.ToString();
        }
        
        planetPropertyText.text = stringToDisplay;
    }

    /// <summary>
    /// Destroys this instance of the Prefab
    /// </summary>
    public void DestroyProperty()
    {
        Destroy(gameObject);
    }
}
