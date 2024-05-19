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
    private CameraControl sampleCameraControl;

    // Setup method: initializes all necessary objects and components for the tests.
    [SetUp]
    public void Setup()
    {
        // Create the main game object which will hold the PlanetPrefabManager component.
        planetObject = new GameObject("PlanetManager");
        planetManager = planetObject.AddComponent<PlanetPrefabManager>();

        // Create and setup the planet sprite object with an Image component.
        planetSpriteObject = new GameObject("PlanetSprite");
        planetSpriteObject.AddComponent<Image>();

        // Create and setup the planet name object with a TextMeshProUGUI component.
        planetNameObject = new GameObject("PlanetName");
        planetNameObject.AddComponent<TextMeshProUGUI>();

        // Assign the created objects manually to the private fields in PlanetPrefabManager using reflection
        // This is necessary because these fields are private and not directly accessible.
        var fields = planetManager.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy);
        foreach (var field in fields)
        {
            if (field.Name == "planetSprite")
                field.SetValue(planetManager, planetSpriteObject);
            else if (field.Name == "planetName")
                field.SetValue(planetManager, planetNameObject);
        }

        // Create a sample sprite to be used in tests.
        sampleSprite = Sprite.Create(new Texture2D(100, 100), new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f));

        // Create a sample camera control object to be used in tests.
        sampleCameraControl = new GameObject("CameraControl").AddComponent<CameraControl>();
    }

    // Test method to verify if the SetPlanetInfo correctly assigns the provided values.
    [Test]
    public void SetPlanetInfo_SetsExpectedValues()
    {
        // Arrange: Define the input values for the test.
        var inputName = "Earth";
        var inputModel = new GameObject("Model");

        // Act: Call the method under test with the arranged values.
        planetManager.SetPlanetInfo(sampleSprite, inputName, inputModel, sampleCameraControl);

        // Assert: Verify that the method correctly set the values.
        Assert.AreEqual(inputName, planetNameObject.GetComponent<TextMeshProUGUI>().text, "The planet name should be set to the expected value.");
        Assert.AreEqual(sampleSprite, planetSpriteObject.GetComponent<Image>().sprite, "The sprite should be set to the expected value.");
        Assert.AreEqual(inputModel, planetManager.Planet3DObject, "The planet model should be set to the expected value.");
        Assert.AreEqual(sampleCameraControl, planetManager.CameraControl, "The camera control should be set to the expected value.");
    }

    // Teardown method: clean up after each test.
    [TearDown]
    public void Teardown()
    {
        // Destroy all created game objects to clean up after tests.
        Object.DestroyImmediate(planetObject);
    }
}
