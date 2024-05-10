using TMPro;
using UnityEngine;

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
    /// The new script that should be stored in the InputField
    /// </param>
    public override void SetText(string text)
    {
        planetPropertyText.text = text;
    }

    /// <summary>
    /// Destroys this instance of the Prefab
    /// </summary>
    public void DestroyProperty()
    {
        Destroy(gameObject);
    }
}
