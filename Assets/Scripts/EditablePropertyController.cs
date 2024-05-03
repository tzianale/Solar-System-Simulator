using TMPro;
using UnityEngine;

public class EditablePropertyController : PropertyFieldController
{
    [SerializeField] private TMP_InputField planetPropertyText;
    
    public override void SetText(string text)
    {
        planetPropertyText.text = text;
    }

    public void DestroyProperty()
    {
        Destroy(gameObject);
    }
}
