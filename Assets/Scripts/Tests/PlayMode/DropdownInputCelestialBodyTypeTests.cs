using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using TMPro;
using System.Collections;
using System.Linq;
using UI;
using System;

namespace Tests.PlayMode
{
    public class DropdownInputCelestialBodyTypeTests
    {
        private GameObject gameObject;
        private TMP_Dropdown dropdown;

        [SetUp]
        public void Setup()
        {
            gameObject = new GameObject();
            dropdown = gameObject.AddComponent<TMP_Dropdown>();
            gameObject.AddComponent<DropdownInputCelestialBodyType>();
        }

        [UnityTest]
        public IEnumerator DropdownIsPopulatedAfterInitialization()
        {
            yield return null;
            Assert.IsNotNull(dropdown, "Dropdown component is not attached.");
            
            string[] expectedOptions = Enum.GetNames(typeof(Models.CelestialBodyType));
            bool allOptionsPresent = expectedOptions.All(expected => dropdown.options.Select(option => option.text).Contains(expected));

            Assert.IsTrue(allOptionsPresent, "Not all celestial body types are present in the dropdown.");
        }

        [TearDown]
        public void Teardown()
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
