using UnityEngine;

/// <summary>
/// Abstract class for Prefabs that are willing to display properties into the planet info tab
/// </summary>
public abstract class PropertyFieldController : MonoBehaviour
{
    /// <summary>
    /// Erases the old text that was being displayed and updates it to the newly given value
    /// </summary>
    /// 
    /// <param name="text">
    /// The updated Text
    /// </param>
    public abstract void SetText(string text);
}