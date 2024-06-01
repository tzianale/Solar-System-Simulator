using Models;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class CelestialBodyExceptionTests
    {
        private GameObject celestialBodyObject;
        private CelestialBody celestialBody;

        [SetUp]
        public void SetUp()
        {
            celestialBodyObject = new GameObject("TestCelestialBody");
            celestialBody = celestialBodyObject.AddComponent<CelestialBody>();
            celestialBody.SetCelestialBodyType(CelestialBodyType.Planet);
            celestialBody.SetMass(5.972e24f);
            celestialBody.SetOrbitRadius(2000f);
            celestialBody.eccentricity = 0.0167f;
            celestialBody.orbitalPeriod = 365.25f;
            celestialBody.orbitingCenterPlanetMass = 1.989e30f;
            celestialBody.SetVelocity(new Vector3(0, 0, 0));
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Explorer;
        }

        [UnityTest]
        public IEnumerator ExceptionTest_OrbitalPeriodZero()
        {
            celestialBody.orbitalPeriod = 0; // Set to provoke an error.

            
            LogAssert.Expect(LogType.Exception, "InvalidOperationException: Eccentric Anomaly calculation resulted in NaN");


            yield return new WaitForFixedUpdate(); // Wait for the next FixedUpdate to provoke the errors
        }




        [UnityTest]
        public IEnumerator ExceptionTest_InvalidEccentricity()
        {
            celestialBody.eccentricity = 1.1f; // Set an invalid eccentricity value

            LogAssert.Expect(LogType.Exception, "ArgumentException: Eccentricity must be between 0 and 1 for elliptical orbits.");


            yield return new WaitForFixedUpdate(); // Wait for the next FixedUpdate to provoke the errors
        }

        [UnityTest]
        public IEnumerator ExceptionTest_OrbitalPeriodIsNaN()
        {
            celestialBody.orbitalPeriod = float.NaN; // This could theoretically cause NaN in calculations

            LogAssert.Expect(LogType.Exception, "InvalidOperationException: Eccentric Anomaly calculation resulted in NaN");


            yield return new WaitForFixedUpdate(); // Wait for the next FixedUpdate to provoke the errors
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(celestialBodyObject);
        }
    }
}
