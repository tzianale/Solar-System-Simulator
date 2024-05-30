using NUnit.Framework;
using UnityEngine;
using Models;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.TestTools;

namespace Tests.EditMode
{
    public class ObservablePropertyControllerTests
    {
        private GameObject testObject;
        private ObservablePropertyController controller;
        private TMP_InputField descriptionField, valueField, unitField;

        [SetUp]
        public void SetUp()
        {
            // Setup for the GameObject and its components
            testObject = new GameObject();
            controller = testObject.AddComponent<ObservablePropertyController>();

            // Create and add TMP_InputFields
            descriptionField = new GameObject("Description").AddComponent<TMP_InputField>();
            valueField = new GameObject("Value").AddComponent<TMP_InputField>();
            unitField = new GameObject("Unit").AddComponent<TMP_InputField>();

            // Set TMP_InputFields in the controller
            controller.GetType().GetField("planetPropertyDescription", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(controller, descriptionField);
            controller.GetType().GetField("planetPropertyValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(controller, valueField);
            controller.GetType().GetField("planetPropertyMeasurementUnit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(controller, unitField);
        }

        [Test]
        public void SetText_UpdatesAllFieldsCorrectly()
        {
            var text = new object[] { "Description", "Value", "Unit" };
            controller.SetText(text);

            Assert.AreEqual("Description", descriptionField.text);
            Assert.AreEqual("Value", valueField.text);
            Assert.AreEqual("Unit", unitField.text);
        }

        [Test]
        public void AddListenerToPropertyValue_AddsListenerCorrectly()
        {
            bool wasCalled = false;
            void Listener(string s) { wasCalled = true; }

            controller.AddListenerToPropertyValue(Listener);
            valueField.onEndEdit.Invoke("test");

            Assert.IsTrue(wasCalled);
        }

        [TearDown]
        public void TearDown()
        {
            // Destroy all game objects created during the test to prevent memory leaks and test cross-contamination
            GameObject.DestroyImmediate(testObject);
            GameObject.DestroyImmediate(descriptionField.gameObject);
            GameObject.DestroyImmediate(valueField.gameObject);
            GameObject.DestroyImmediate(unitField.gameObject);
        }
    }
}
