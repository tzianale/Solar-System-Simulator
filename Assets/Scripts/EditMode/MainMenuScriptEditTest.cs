using NUnit.Framework;
using UnityEngine;

public class MainMenuScriptEditTests
{
    [Test]
    public void OpenPanel_ActivatesPanel()
    {
        // Arrange
        var menuObject = new GameObject();
        var menuScript = menuObject.AddComponent<MainMenuScript>();
        var panelObject = new GameObject();
        panelObject.SetActive(false); // Panel starts inactive
        menuScript.Panel = panelObject;

        // Act
        menuScript.openPanel();

        // Assert
        Assert.IsTrue(panelObject.activeSelf, "Panel should be active after openPanel is called");
    }

    [Test]
    public void ClosePanel_DeactivatesPanel()
    {
        // Arrange
        var menuObject = new GameObject();
        var menuScript = menuObject.AddComponent<MainMenuScript>();
        var panelObject = new GameObject();
        panelObject.SetActive(true); // Panel starts active
        menuScript.Panel = panelObject;

        // Act
        menuScript.closePanel();

        // Assert
        Assert.IsFalse(panelObject.activeSelf, "Panel should be inactive after closePanel is called");
    }
}
