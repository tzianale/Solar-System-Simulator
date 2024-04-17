using NUnit.Framework;
using UnityEngine;
using TMPro;

public class PlanetListContainerScriptTests
{
    private GameObject containerGameObject;
    private PlanetListContainerScript containerScript;

    [SetUp]
public void Setup()
{
    containerGameObject = new GameObject();
    containerScript = containerGameObject.AddComponent<PlanetListContainerScript>();
    containerScript.buttonText = new GameObject("TextObject").AddComponent<TextMeshProUGUI>();
    containerScript.buttonText.text = ""; // Initialize text to ensure it's not null

    // Directly use the existing transform rather than assigning a new one
    containerScript._planetListContainerTransform = containerGameObject.transform;
    containerScript.Start();
}



    [Test]
    public void OnArrowClick_TogglesListAndChangesButtonText()
    {
        // Act
        containerScript.OnArrowClick();

        // Assert
        //Panel is openeing
        Assert.IsTrue(containerScript._listIsOpen, "List should be open after first arrow click.");
        Assert.AreEqual("↓", containerScript.buttonText.text, "Button text should be '↓' after first arrow click.");
        

        // Act
        containerScript.OnArrowClick();

        // Assert
        //Panel is closing
        Assert.IsFalse(containerScript._listIsOpen, "List should be not open after second arrow click.");
        Assert.AreEqual("↑", containerScript.buttonText.text, "Button text should be '↑' after second arrow click.");
        
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(containerGameObject);
    }
}
