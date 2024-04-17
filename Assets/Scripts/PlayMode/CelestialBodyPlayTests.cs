using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CelestialBodyPlayTests
{
    [UnityTest]
    public IEnumerator Gravity_AttractsCelestialBodies()
    {
        var sun = new GameObject().AddComponent<CelestialBody>();
        sun.transform.position = new Vector3(0, 0, 0);
        sun.mass = 1.99e+22f;
        sun.celestType = CelestialBody.CelestialBodyType.Sun;
        sun.isGravityOn = true;
        sun.orbitRadius = 0;
        var sunRb = sun.gameObject.AddComponent<Rigidbody>();
        sunRb.useGravity = false; // Deactivating Unity's own gravity
        sunRb.isKinematic = true; // Prevents rigid body physics from influencing the position

        var mercury = new GameObject().AddComponent<CelestialBody>();
        mercury.transform.position = new Vector3(755, 0, 0); // Start 10 units away
        mercury.mass = 3.3e+15f;
        mercury.celestType = CelestialBody.CelestialBodyType.Planet;
        mercury.isGravityOn = true;
        mercury.orbitRadius = 775f;
        var planetRb = mercury.gameObject.AddComponent<Rigidbody>();
        planetRb.useGravity = false; // Deactivating Unity's own gravity
        planetRb.isKinematic = true; // Prevents rigid body physics from influencing the position
  
        // Allow some time for gravity to act
        yield return new WaitForSeconds(0.004f);

        // Check if the planet has moved closer to the sun
        float newDistance = Vector3.Distance(sun.transform.position, mercury.transform.position);
        Assert.Less(newDistance, 755);
    }
}
