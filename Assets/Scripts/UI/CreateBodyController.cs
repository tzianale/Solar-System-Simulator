using System;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class CreateBodyController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputFieldName;
        [SerializeField] private TMP_Dropdown inputFieldType;
        [SerializeField] private TMP_InputField inputFieldMass;
        [SerializeField] private TMP_InputField inputFieldDiameter;
        [SerializeField] private TMP_InputField inputFieldPositionX;
        [SerializeField] private TMP_InputField inputFieldPositionY;
        [SerializeField] private TMP_InputField inputFieldPositionZ;
        [SerializeField] private TMP_InputField inputFieldInitialVelocityX;
        [SerializeField] private TMP_InputField inputFieldInitialVelocityY;
        [SerializeField] private TMP_InputField inputFieldInitialVelocityZ;
        [SerializeField] private Image colorPickerButtonImage;
        [SerializeField] private GameObject colorPickerPanel;
        [SerializeField] private PlanetListManager planetListManager;
        
        private Color _selectedColor;

        public void CreateNewCelestialBody()
        {
            GameObject newCelestialBody = CelestialBodyGenerator.CreateNewCelestialBodyGameObject(
                inputFieldName.text,
                (CelestialBodyType)inputFieldType.value,
                new Vector3(float.Parse(inputFieldPositionX.text), float.Parse(inputFieldPositionY.text), float.Parse(inputFieldPositionZ.text)),
                float.Parse(inputFieldMass.text),
                float.Parse(inputFieldDiameter.text),
                new Vector3(float.Parse(inputFieldInitialVelocityX.text), float.Parse(inputFieldInitialVelocityY.text), float.Parse(inputFieldInitialVelocityZ.text)),
                _selectedColor
            );
            
            planetListManager.AddNewCelestialBody(newCelestialBody);
        }

        public void OnSelectedColorChange(Color color)
        {
            colorPickerButtonImage.color = color;
            _selectedColor = color;
        }
    }
}
