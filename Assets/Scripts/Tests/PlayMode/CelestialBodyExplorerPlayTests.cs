using Models;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class CelestialBodyExplorerPlayTests
    {
        private GameObject celestialBodyObject;
        private CelestialBody celestialBody;

        [SetUp]
        public void SetUp()
        {
            celestialBodyObject = new GameObject("Earth");
            celestialBody = celestialBodyObject.AddComponent<CelestialBody>();
            celestialBody.SetCelestialBodyType(CelestialBody.CelestialBodyType.Planet);
            celestialBody.SetMass(5.972e24f); // Mass of the Earth
            celestialBody.SetVelocity(new Vector3(0, 0, 11.2f)); // Velocity of the Earth
            celestialBody.SetOrbitRadius(2000f); // Orbit radius, scaled value for simulation
            celestialBody.orbitalPeriod = 365.25f; // One Earth year
            celestialBody.sideRealRotationPeriod = 50;
            celestialBody.ratioToEarthYear = 1;
            celestialBody.eccentricity = 0.0167f; // Eccentricity of Earth's orbit
            celestialBody.orbitingCenterPlanetMass = 1.989e30f; // Mass of the Sun
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Explorer;
        }

        [UnityTest]
        public IEnumerator KeplerLawsAreAppliedWhenInExplorerMode()
        {
            Vector3 initialPosition = celestialBody.transform.position;

            yield return new WaitForSecondsRealtime(5f); // Wait to allow movement

            Vector3 newPosition = celestialBody.transform.position;
            Assert.AreNotEqual(initialPosition, newPosition, "Celestial body should have moved according to Kepler's laws.");

            // Validate that the position not only changed but also has realistic new values
            Assert.IsFalse(float.IsNaN(newPosition.x) || float.IsNaN(newPosition.y) || float.IsNaN(newPosition.z), "New position should not be NaN.");
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(celestialBodyObject);
        }
    }
}
