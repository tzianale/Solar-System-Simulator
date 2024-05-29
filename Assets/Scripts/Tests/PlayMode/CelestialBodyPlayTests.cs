using System.Collections;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
namespace Tests.PlayMode
{
    public class CelestialBodyPlayTests
    {
        private GameObject sun;
        private GameObject planet;
        private CelestialBody sunCB;
        private CelestialBody planetCB;

        [UnityTest]
        public IEnumerator CelestialBodies_UpdateVelocityAndPositionDueToGravity()
        {
            // Arrange
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Sandbox;
            sun = new GameObject("Sun");
            sunCB = sun.AddComponent<CelestialBody>(); // Correctly adding the CelestialBody component
            sunCB.SetCelestialBodyType(CelestialBody.CelestialBodyType.Sun);
            sunCB.SetMass(332900);
            var sunRb = sun.AddComponent<Rigidbody>();
            sunRb.useGravity = false;
            sunRb.isKinematic = true;

            planet = new GameObject("Planet");
            planetCB = planet.AddComponent<CelestialBody>(); // Correctly adding the CelestialBody component
            planet.transform.position = new Vector3(755, 0, 0);
            planetCB.SetCelestialBodyType(CelestialBody.CelestialBodyType.Planet);
            planetCB.SetMass(1);
            var planetRb = planet.AddComponent<Rigidbody>();
            planetRb.useGravity = false;
            planetRb.isKinematic = false;

            float initialDistance = Vector3.Distance(sun.transform.position, planet.transform.position);

            // Act
            yield return new WaitForSecondsRealtime(2f); // Wait 2 seconds of real time

            // Assert
            float newDistance = Vector3.Distance(sun.transform.position, planet.transform.position);
            Assert.Less(newDistance, initialDistance,
                "Planet should move closer to the sun due to gravitational attraction.");
        }

        [TearDown]
        public void Teardown()
        {
            // Cleanup
            if (sun != null)
                Object.DestroyImmediate(sun);
            if (planet != null)
                Object.DestroyImmediate(planet);
        }
    }
}
