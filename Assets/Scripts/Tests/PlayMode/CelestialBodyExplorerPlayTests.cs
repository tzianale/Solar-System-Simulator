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
            celestialBody.SetMass(5.972e24f); // Masse der Erde
            celestialBody.SetVelocity(new Vector3(0, 0, 11.2f)); // Geschwindigkeit der Erde
            celestialBody.SetOrbitRadius(2000f); // Umlaufradius, skalierter Wert für Simulation
            celestialBody.orbitalPeriod = 365.25f; // Ein Erdenjahr
            celestialBody.orbitalSpeed = 30290;
            celestialBody.rotaionSpeed = 50;
            celestialBody.ratioToEarthYear = 1;
            celestialBody.perihelion = 1.47095e+08f;
            celestialBody.eccentricity = 0.0167f; // Exzentrizität der Erdumlaufbahn
            celestialBody.orbitingCenterPlanetMass = 1.989e30f; // Masse der Sonne
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Explorer;
        }

        [UnityTest]
        public IEnumerator KeplerLawsAreAppliedWhenInExplorerMode()
        {
            Vector3 initialPosition = celestialBody.transform.position;

            yield return new WaitForSecondsRealtime(5f); // Warten, um Bewegung zu erlauben

            Vector3 newPosition = celestialBody.transform.position;
            Assert.AreNotEqual(initialPosition, newPosition, "Celestial body should have moved according to Kepler's laws.");

            // Validierung, dass die Position sich nicht nur geändert hat, sondern auch realistische neue Werte aufweist
            Assert.IsFalse(float.IsNaN(newPosition.x) || float.IsNaN(newPosition.y) || float.IsNaN(newPosition.z), "New position should not be NaN.");
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(celestialBodyObject);
        }
    }
}