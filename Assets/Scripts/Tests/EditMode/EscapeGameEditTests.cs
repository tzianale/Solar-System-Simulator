using NUnit.Framework;
using UnityEngine;

public class EscapeScriptTests
{
    private GameObject panelObject;
    private EscapeScript escapeScript;

    [SetUp]
    public void Setup()
    {
        var menuObject = new GameObject();
        escapeScript = menuObject.AddComponent<EscapeScript>();
        panelObject = new GameObject();
        escapeScript.Panel = panelObject;
    }

    [Test]
    public void OpenPanel_ActivatesPanel()
    {
        // Arrange
        panelObject.SetActive(false); // Panel starts inactive

        // Act
        escapeScript.openPanel();

        // Assert
        Assert.IsTrue(panelObject.activeSelf, "Panel should be active after openPanel is called");
    }

    [Test]
    public void ClosePanel_DeactivatesPanel()
    {
        // Arrange
        panelObject.SetActive(true); // Panel starts active

        // Act
        escapeScript.closePanel();

        // Assert
        Assert.IsFalse(panelObject.activeSelf, "Panel should be inactive after closePanel is called");
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(panelObject);
        Object.DestroyImmediate(escapeScript.gameObject);
    }
}
