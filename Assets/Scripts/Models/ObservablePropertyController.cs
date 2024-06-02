using TMPro;
using UnityEngine;
using UnityEngine.Events;
using SystemObject = System.Object;


namespace Models
{
    /// <summary>
    /// Controller Script for Prefab "Observable Property Prefab"
    /// </summary>
    public class ObservablePropertyController : PropertyFieldController
    {
        [SerializeField] private TMP_InputField planetPropertyDescription;
        [SerializeField] private TMP_InputField planetPropertyValue;
        [SerializeField] private TMP_InputField planetPropertyMeasurementUnit;

        public bool IsBeingEdited { get; private set; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="listener"></param>
        public void AddListenerToPropertyValueEditEnd(UnityAction<string> listener)
        {
            planetPropertyValue.onEndEdit.AddListener(listener);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="listener"></param>
        public override void AddListenerToAllOnSelection(UnityAction<string> listener)
        {
            planetPropertyDescription.onSelect.AddListener(listener);
            planetPropertyValue.onSelect.AddListener(listener);
            planetPropertyMeasurementUnit.onSelect.AddListener(listener);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="listener"></param>
        public override void AddListenerToAllOnEditEnd(UnityAction<string> listener)
        {
            planetPropertyDescription.onEndEdit.AddListener(listener);
            planetPropertyValue.onEndEdit.AddListener(listener);
            planetPropertyMeasurementUnit.onEndEdit.AddListener(listener);
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void Update()
        {
            IsBeingEdited = planetPropertyValue.isFocused;
        }


        /// <summary>
        /// Allows external scripts to edit the text inside the InputField by code
        /// </summary>
        /// 
        /// <param name="text">
        /// The new values that should be stored in the InputField. Text is intended to be an array with three elements:
        /// - The name of the property
        /// - The value of the property
        /// - The measurement unit of the property
        /// </param>
        public override void SetText(SystemObject[] text)
        {
            planetPropertyDescription.text = text[(int) DataIndexes.PropertyDescription].ToString();
            
            SetValue(text[(int) DataIndexes.PropertyValue].ToString());
            SetUnit(text[(int) DataIndexes.PropertyUnit].ToString());
        }
        
        
        /// <summary>
        /// Allows external scripts to edit the text inside the InputField by code
        /// </summary>
        /// 
        /// <param name="value">
        /// The new script that should be stored in the InputField
        /// </param>
        private void SetValue(string value)
        {
            planetPropertyValue.text = value;
        }
        
        
        /// <summary>
        /// Allows external scripts to edit the text inside the InputField by code
        /// </summary>
        /// 
        /// <param name="text">
        /// The new script that should be stored in the InputField
        /// </param>
        private void SetUnit(string text)
        {
            text ??= "";
            
            planetPropertyMeasurementUnit.text = text;
        }
    }
}

