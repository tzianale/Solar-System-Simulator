using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Extends the basic PlanetInfoPrefabController by implementing method GeneratePropertyDependingOnSubClass.
    /// This specific implementation replaces Text Fields with Input Fields for certain properties, allowing
    /// the user to tweak the description at will.
    /// </summary>
    public class PlanetVariableInfoPrefabController : PlanetInfoPrefabController
    {
        [SerializeField] private GameObject inputFieldPrefab;
        [SerializeField] private TMP_InputField planetDescriptionContainer;

        /// <summary>
        /// TODO
        /// </summary>
        /// 
        /// <param name="planetDescription">
        /// TODO
        /// </param>
        protected override void SetDescription(string planetDescription)
        {
            planetDescriptionContainer.text = planetDescription;
        }
    
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
            return GenerateInputFieldUsingPrefab(propertyName, propertyValue);
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
        private GameObject GenerateInputFieldInCode(string propertyName, string propertyValue)
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
        private GameObject GenerateInputFieldUsingPrefab(string propertyName, string propertyValue)
        {
            var planetPropertyGameObject = Instantiate(inputFieldPrefab);

            var planetPropertyController = planetPropertyGameObject.GetComponent<PropertyFieldController>();
        
            planetPropertyController.SetText(propertyName + NameValueSeparator + propertyValue);
        
            return planetPropertyGameObject;
        }
    
        /// <summary>
        /// Generates a new empty field for an additional Property at runtime
        /// </summary>
        public void AddNewEmptyStaticProperty()
        {
            var emptyProperty = GeneratePropertyDependingOnSubClass(UnknownPropertyText, UnknownPropertyText);
        
            emptyProperty.transform.SetParent(planetStaticPropertiesContainer.transform, false);
        
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}
