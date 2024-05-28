using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SystemObject = System.Object;

namespace UI
{
    /// <summary>
    /// Extends the basic PlanetInfoPrefabController by implementing method GeneratePropertyDependingOnSubClass.
    /// This specific implementation replaces Text Fields with Input Fields for certain properties, allowing
    /// the user to tweak the description at will.
    /// </summary>
    public class PlanetVariableInfoPrefabController : PlanetInfoPrefabController
    {
        [SerializeField] private GameObject editablePropertiesFieldPrefab;
        [SerializeField] private TMP_InputField planetDescriptionContainer;

        /// <summary>
        /// Sets a description string to the Info Prefab
        /// </summary>
        /// 
        /// <param name="planetDescription">
        /// The string to set as Planet Description
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
        /// <param name="measurementUnit">The Unit of the property to be initialised</param>
        /// 
        /// <returns>
        /// A GameObject with InputField functionalities.
        /// At the start the Object will have a name and a value for each property.
        /// The user will then be able to modify both and adapt them to their likings.
        /// </returns>
        private protected override GameObject GeneratePropertyDependingOnSubClass(string propertyName,
            string propertyValue, string measurementUnit)
        {
            return GenerateInputFieldUsingPrefab(propertyName, propertyValue, measurementUnit);
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
        /// <param name="measurementUnit">The Unit of the property to be initialised</param>
        /// 
        /// <returns>
        /// The generated Input Field GameObject with the corresponding text
        /// </returns>
        private GameObject GenerateInputFieldUsingPrefab(string propertyName, string propertyValue,
            string measurementUnit)
        {
            var planetPropertyGameObject = Instantiate(editablePropertiesFieldPrefab);

            var planetPropertyController = planetPropertyGameObject.GetComponent<PropertyFieldController>();

            var propertyText = new SystemObject[]
                { propertyName + NameValueSeparator, propertyValue, ValueUnitSeparatorForProperties + measurementUnit };

            planetPropertyController.SetText(propertyText);

            return planetPropertyGameObject;
        }

        /// <summary>
        /// Generates a new empty field for an additional Property at runtime
        /// </summary>
        public void AddNewEmptyStaticProperty()
        {
            var emptyProperty =
                GeneratePropertyDependingOnSubClass(UnknownPropertyText, UnknownPropertyText, UnknownPropertyText);

            emptyProperty.transform.SetParent(planetStaticPropertiesContainer.transform, false);

            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}