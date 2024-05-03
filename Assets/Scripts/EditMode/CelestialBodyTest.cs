using NUnit.Framework;
using UnityEngine;


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
        celestialBody.SetCelestialBodyType(CelestialBody.CelestialBodyType.Sun);
        celestialBody.SetMass(sunMass);
        celestialBody.SetOrbitRadius(0);
        celestialBody.SetRatioToEarthYear(1);

        //Assert
        Assert.AreEqual(CelestialBody.CelestialBodyType.Sun, celestialBody.GetCelestialBodyType());
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
        celestialBody.SetCelestialBodyType(CelestialBody.CelestialBodyType.Planet);
        celestialBody.SetMass(earthMass);

        //Assert
        Assert.AreEqual(CelestialBody.CelestialBodyType.Planet, celestialBody.GetCelestialBodyType());
        Assert.AreEqual(earthMass, celestialBody.GetMass());
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(celestialBodyObject);
    }
}
