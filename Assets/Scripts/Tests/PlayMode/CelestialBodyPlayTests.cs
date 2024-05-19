using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CelestialBodyPlayTests
{
    [UnityTest]
    public IEnumerator CelestialBodies_UpdateVelocityAndPositionDueToGravity()
    {
        // Arrange
        SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Sandbox;
        var sun = new GameObject("Sun").AddComponent<CelestialBody>();
        sun.SetCelestialBodyType(CelestialBody.CelestialBodyType.Sun);
        sun.SetMass(332900);
        var sunRb = sun.gameObject.AddComponent<Rigidbody>();
        sunRb.useGravity = false;
        sunRb.isKinematic = true;

        var planet = new GameObject("Planet").AddComponent<CelestialBody>();
        planet.transform.position = new Vector3(755, 0, 0);
        planet.SetCelestialBodyType(CelestialBody.CelestialBodyType.Planet);
        planet.SetMass(1);
        var planetRb = planet.gameObject.AddComponent<Rigidbody>();
        planetRb.useGravity = false;
        planetRb.isKinematic = false;

        float initialDistance = Vector3.Distance(sun.transform.position, planet.transform.position);

        // Act
        // Allow time for Unity to call FixedUpdate and simulate physics
        yield return new WaitForSecondsRealtime(0.5f); // Wait half a second of real time

        // Assert
        float newDistance = Vector3.Distance(sun.transform.position, planet.transform.position);
        Assert.Less(newDistance, initialDistance, "Planet should move closer to the sun due to gravitational attraction.");

        // Cleanup
        Object.DestroyImmediate(sun.gameObject);
        Object.DestroyImmediate(planet.gameObject);
    }
}
