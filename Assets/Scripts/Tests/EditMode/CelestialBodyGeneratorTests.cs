using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System;
using Models;


namespace Tests.EditMode
{
    [TestFixture]

    public class CelestialBodyGeneratorTests
    {
        private GameObject createdObject;

        [Test]
        public void CreateNewCelestialBodyGameObject_CreatesGameObjectWithCorrectProperties()
        {

            // Arrange
            string expectedName = "Earth";
            Vector3 expectedPosition = new Vector3(0, 1, 0);
            float expectedMass = 5.97f; // Earth mass approximation in some units
            float expectedDiameter = 12742f; // Earth diameter approximation in km
            Vector3 expectedVelocity = new Vector3(0, 29.78f, 0); // Earth orbital velocity in km/s
            CelestialBodyType expectedType = CelestialBodyType.Planet;
            Color expectedColor = Color.red;

            // Act
            createdObject = CelestialBodyGenerator.CreateNewCelestialBodyGameObject(expectedName, expectedType, expectedPosition, expectedMass, expectedDiameter, expectedVelocity, expectedColor);


            // Assert
            Assert.NotNull(createdObject);
            Assert.AreEqual(expectedName, createdObject.name);
            Assert.AreEqual(expectedPosition, createdObject.transform.position);
            Assert.AreEqual(new Vector3(expectedDiameter, expectedDiameter, expectedDiameter), createdObject.transform.localScale);

            CelestialBody createdBody = createdObject.GetComponent<CelestialBody>();
            Renderer createdBodyRenderer = createdObject.GetComponent<Renderer>();
            Assert.IsNotNull(createdBody);
            Assert.AreEqual(expectedMass, createdBody.GetMass());
            Assert.AreEqual(expectedVelocity, createdBody.GetVelocity());
            Assert.AreEqual(expectedType, createdBody.GetCelestialBodyType());
            Assert.AreEqual(expectedColor, createdBodyRenderer.sharedMaterial.color);
        }

        [Test]
        public void CreateNewCelestialBodyGameObject_WithNullName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                CelestialBodyGenerator.CreateNewCelestialBodyGameObject(null, CelestialBodyType.Planet, new Vector3(0, 1, 0), 1.0f, 1000f, new Vector3(0, 0, 0), Color.red),
                "Expected ArgumentException for null name not thrown."
            );
        }

        [Test]
        public void CreateNewCelestialBodyGameObject_WithNegativeMass_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                CelestialBodyGenerator.CreateNewCelestialBodyGameObject("Earth", CelestialBodyType.Planet, new Vector3(0, 1, 0), -1.0f, 1000f, new Vector3(0, 0, 0), Color.red),
                "Expected ArgumentException for negative mass not thrown."
            );
        }

        [Test]
        public void CreateNewCelestialBodyGameObject_WithNegativeDiameter_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                CelestialBodyGenerator.CreateNewCelestialBodyGameObject("Earth", CelestialBodyType.Planet, new Vector3(0, 1, 0), 1.0f, -1000f, new Vector3(0, 0, 0), Color.red),
                "Expected ArgumentException for negative diameter not thrown."
            );
        }
    }

}