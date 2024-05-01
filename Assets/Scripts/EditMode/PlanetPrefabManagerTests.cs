using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetPrefabManagerTests
{
    private GameObject planetObject;
    private PlanetPrefabManager planetManager;
    private GameObject planetSpriteObject;
    private GameObject planetNameObject;
    private Sprite sampleSprite;
    private Camera sampleCamera;

    [SetUp]
public void Setup()
{
    // Create a larger texture
    Texture2D largeTexture = new Texture2D(100, 100, TextureFormat.RGBA32, false);

    // Fill the texture with a solid color
    Color fill = Color.white;
    Color[] fillPixels = new Color[largeTexture.width * largeTexture.height];
    for (int i = 0; i < fillPixels.Length; i++)
    {
        fillPixels[i] = fill;
    }
    largeTexture.SetPixels(fillPixels);
    largeTexture.Apply();

    // Create the planet manager object
    planetObject = new GameObject();
    planetManager = planetObject.AddComponent<PlanetPrefabManager>();

    // Create objects and add necessary components
    planetSpriteObject = new GameObject();
    planetSpriteObject.AddComponent<Image>();
    planetNameObject = new GameObject();
    planetNameObject.AddComponent<TextMeshProUGUI>();

    // Set fields via reflection or by exposing in the script
    planetManager.GetType().GetField("planetSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(planetManager, planetSpriteObject);
    planetManager.GetType().GetField("planetName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(planetManager, planetNameObject);

    // Setup sample data
    sampleSprite = Sprite.Create(largeTexture, new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f));
    sampleCamera = new GameObject().AddComponent<Camera>();
}


    [Test]
    public void SetPlanetInfo_SetsExpectedValues()
    {
        // Arrange
        var inputName = "Earth";
        var inputModel = new GameObject();
        var cameraAdjustment = 10;

        // Act
        planetManager.SetPlanetInfo(sampleSprite, inputName, inputModel, cameraAdjustment, sampleCamera);

        // Assert
        Assert.AreEqual(inputName, planetNameObject.GetComponent<TextMeshProUGUI>().text);
        Assert.AreEqual(sampleSprite, planetSpriteObject.GetComponent<Image>().sprite);
        Assert.AreEqual(inputModel, planetManager.Planet3DObject);
        Assert.AreEqual(sampleCamera, planetManager.LinkedCamera);
        Assert.AreEqual(cameraAdjustment, planetManager.CameraAdjustmentOnPlanetClick);
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up
        Object.DestroyImmediate(planetObject);
        Object.DestroyImmediate(planetSpriteObject);
        Object.DestroyImmediate(planetNameObject);
    }
}
