using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CelestialBodyTest
{
    
    [Test]
    public void CelestialBody_Initializes_SunCorrectly()
    {
        //Arrange
        GameObject celestialBodyObject  = new GameObject();
        var sun = celestialBodyObject.AddComponent<CelestialBody>();
        var sunMass = 1.99e+22f;
        var sunDays = 0;
        var sunOrbitRadius = 0;
        var sunRatioToEarth = 1;

        //Act
        sun.celestType = CelestialBody.CelestialBodyType.Sun;
        sun.mass = sunMass;
        sun.isGravityOn = false;
        sun.day = sunDays;
        sun.orbitRadius = sunOrbitRadius;
        sun.ratioToEarthYear = 1;

        //Assert
        //CelestialBodyTyp gets testet
        Assert.AreEqual(CelestialBody.CelestialBodyType.Sun, sun.celestType);
        //Does the sun have got the right weight
        Assert.AreEqual(sunMass, sun.mass);
        //Is gravity activated
        Assert.False(sun.isGravityOn);
        //Is the variable day right
        Assert.AreEqual(sunDays, sun.day);
        Assert.AreEqual(sunOrbitRadius, sun.orbitRadius);
        Assert.AreEqual(sunRatioToEarth, sun.ratioToEarthYear);

    }

    [Test]
    public void CelestialBody_Initializes_EarthCorrectly()
    {
        //Arrange
        
        var earth = new GameObject().AddComponent<CelestialBody>();
        var earthMass = 5.97217e+16f;

        //Act
        earth.celestType = CelestialBody.CelestialBodyType.Planet;
        earth.mass = earthMass;

        //Assert
        Assert.AreEqual(CelestialBody.CelestialBodyType.Planet, earth.celestType);
        Assert.AreEqual(earthMass, earth.mass);
    }
}
