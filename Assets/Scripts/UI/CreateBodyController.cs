using Models;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CreateBodyController : MonoBehaviour
    {
        public TMP_InputField InputFieldName;
        [SerializeField] private TMP_InputField InputFieldMass;
        [SerializeField] private TMP_InputField InputFieldDiameter;
        [SerializeField] private TMP_InputField InputFieldPositionX;
        [SerializeField] private TMP_InputField InputFieldPositionY;
        [SerializeField] private TMP_InputField InputFieldPositionZ;
        [SerializeField] private TMP_InputField InputFieldInitialVelocityX;
        [SerializeField] private TMP_InputField InputFieldInitialVelocityY;
        [SerializeField] private TMP_InputField InputFieldInitialVelocityZ;
        [SerializeField] private TMP_InputField InputFieldColorR;
        [SerializeField] private TMP_InputField InputFieldColorG;
        [SerializeField] private TMP_InputField InputFieldColorB;
        [SerializeField] private GameObject windowPanel;

        public void CreateNewCelesitalBody()
        {
            CelestialBodyGenerator.CreateNewCelestialBodyGameObject(
                InputFieldName.text,
                CelestialBody.CelestialBodyType.Planet,
                new Vector3(float.Parse(InputFieldPositionX.text), float.Parse(InputFieldPositionY.text), float.Parse(InputFieldPositionZ.text)),
                float.Parse(InputFieldMass.text),
                float.Parse(InputFieldDiameter.text),
                new Vector3(float.Parse(InputFieldInitialVelocityX.text), float.Parse(InputFieldInitialVelocityY.text), float.Parse(InputFieldInitialVelocityZ.text))
            );

            windowPanel.SetActive(false);
        }
    }
}
