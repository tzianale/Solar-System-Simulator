using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SystemObject = System.Object;

namespace Models.Tests
{
    public class EditablePropertyControllerTests
    {
        private GameObject gameObject;
        private EditablePropertyController controller;
        private TMP_InputField inputField;

        [SetUp]
        public void Setup()
        {
            // Setup the game object and add required components
            gameObject = new GameObject();
            inputField = gameObject.AddComponent<TMP_InputField>();
            controller = gameObject.AddComponent<EditablePropertyController>();

            // Use reflection or direct assignment to set private fields if necessary
            var field = typeof(EditablePropertyController).GetField("planetPropertyText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(controller, inputField);
        }

        [Test]
        public void SetText_SetsInputFieldText()
        {
            // Arrange
            SystemObject[] texts = {"Hello", ", ", "World"};

            // Act
            controller.SetText(texts);

            // Assert
            Assert.AreEqual("Hello, World", inputField.text);
        }

        [Test]
        public void DestroyProperty_DestroysGameObject()
        {
            // Act
            controller.DestroyProperty();

            // Assert
            Assert.IsTrue(gameObject == null);
        }
    }
}
