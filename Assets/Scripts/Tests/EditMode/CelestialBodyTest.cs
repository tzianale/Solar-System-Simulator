using Models;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class CelestialBodyTest
    {
        private CelestialBody celestialBody;
        private GameObject celestialBodyObject;

        [SetUp]
        public void SetUp()
        {
            celestialBodyObject = new GameObject();
            celestialBody = celestialBodyObject.AddComponent<CelestialBody>();
        }

        [Test]
        public void CelestialBody_Initializes_SunCorrectly()
        {
            //Arrange
            var sunMass = 1.99e+22f;

            //Act
            celestialBody.SetCelestialBodyType(CelestialBodyType.Sun);
            celestialBody.SetMass(sunMass);
            celestialBody.SetOrbitRadius(0);
            celestialBody.SetRatioToEarthYear(1);

            //Assert
            Assert.AreEqual(CelestialBodyType.Sun, celestialBody.GetCelestialBodyType());
            Assert.AreEqual(sunMass, celestialBody.GetMass());
            Assert.AreEqual(0, celestialBody.GetOrbitRadius());
            Assert.AreEqual(1, celestialBody.GetRatioToEarthYear());
        }

        [Test]
        public void CelestialBody_Initializes_EarthCorrectly()
        {
            //Arrange
            var earthMass = 5.97217e+16f;

            //Act
            celestialBody.SetCelestialBodyType(CelestialBodyType.Planet);
            celestialBody.SetMass(earthMass);

            //Assert
            Assert.AreEqual(CelestialBodyType.Planet, celestialBody.GetCelestialBodyType());
            Assert.AreEqual(earthMass, celestialBody.GetMass());
        }

        [Test]
        public void GetOrbitLinePoints_ShouldReturnCorrectNumberOfPoints()
        {
            // Arrange
            celestialBody.SetOrbitRadius(100f); // Set a test orbit radius

            // Act
            Vector3[] points = celestialBody.GetOrbitLinePoints();

            // Assert
            Assert.AreEqual(360, points.Length, "GetOrbitLinePoints should return 360 points.");
        }

        [Test]
        public void GetOrbitLinePoints_PointsShouldLieOnOrbitRadius()
        {
            // Arrange
            celestialBody.SetOrbitRadius(100f); // Set a test orbit radius

            // Act
            Vector3[] points = celestialBody.GetOrbitLinePoints();

            // Assert
            foreach (var point in points)
            {
                float distance = Vector3.Distance(Vector3.zero, point);
                Assert.AreEqual(100f, distance, 0.01f, "All points should lie on the orbit radius.");
            }
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(celestialBodyObject);
        }
    }
}
