using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CelestialBodyPlayTests
{
    [UnityTest]
    public IEnumerator Gravity_AttractsCelestialBodies()
    {
        // Create the sun object
        var sun = new GameObject("Sun").AddComponent<CelestialBody>();
        sun.transform.position = new Vector3(0, 0, 0);
        sun.SetMass(1.99e+22f); // Set mass of the sun
        sun.SetCelestialBodyType(CelestialBody.CelestialBodyType.Sun); // Type of the sun
        sun.SetGravityEnabled(true); // Enable gravity
        var sunRb = sun.gameObject.AddComponent<Rigidbody>();
        sunRb.useGravity = false; // Deactivate Unity's own gravity
        sunRb.isKinematic = true; // Prevent Rigidbody physics from affecting the position

        // Create Mercury object
        var mercury = new GameObject("Mercury").AddComponent<CelestialBody>();
        mercury.transform.position = new Vector3(755, 0, 0); // Start 755 units away
        mercury.SetMass(3.3e+15f); // Set mass of Mercury
        mercury.SetCelestialBodyType(CelestialBody.CelestialBodyType.Planet); // Type of the planet
        mercury.SetGravityEnabled(true); // Enable gravity
        var planetRb = mercury.gameObject.AddComponent<Rigidbody>();
        planetRb.useGravity = false; // Deactivate Unity's own gravity
        planetRb.isKinematic = true; // Prevent Rigidbody physics from affecting the position

        // Allow some time for gravity to act
        yield return new WaitForSeconds(0.004f); // Short wait for the gravitational effect

        // Check if the planet has moved closer to the sun
        float newDistance = Vector3.Distance(sun.transform.position, mercury.transform.position);
        Assert.Less(newDistance, 755); // Expect the new distance to be less than 755
    }
}
