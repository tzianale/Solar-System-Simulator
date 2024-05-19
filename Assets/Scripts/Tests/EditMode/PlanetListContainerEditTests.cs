using NUnit.Framework;
using UnityEngine;
using TMPro;

public class PlanetListAnimatorTests
{
    private GameObject containerGameObject;
    private PlanetListAnimator _animator;

    [SetUp]
public void Setup()
{
    containerGameObject = new GameObject();
    _animator = containerGameObject.AddComponent<PlanetListAnimator>();
    _animator.buttonText = new GameObject("TextObject").AddComponent<TextMeshProUGUI>();
    _animator.buttonText.text = ""; // Initialize text to ensure it's not null

    // Directly use the existing transform rather than assigning a new one
    _animator.PlanetListContainerTransform = containerGameObject.transform;
}



    [Test]
    public void OnArrowClick_TogglesListAndChangesButtonText()
    {
        // Act
        _animator.OnArrowClick();

        // Assert
        //Panel is openeing
        Assert.IsTrue(_animator.ListIsOpen, "List should be open after first arrow click.");
        Assert.AreEqual("↓", _animator.buttonText.text, "Button text should be '↓' after first arrow click.");
        

        // Act
        _animator.OnArrowClick();

        // Assert
        //Panel is closing
        Assert.IsFalse(_animator.ListIsOpen, "List should be not open after second arrow click.");
        Assert.AreEqual("↑", _animator.buttonText.text, "Button text should be '↑' after second arrow click.");
        
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(containerGameObject);
    }
}
