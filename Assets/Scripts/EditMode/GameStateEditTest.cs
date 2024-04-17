using NUnit.Framework;
using UnityEngine;

public class GameStateScriptTests
{
    [Test]
    public void PlayPause_TogglesTimeScaleCorrectly()
    {
        // Arrange
        var gameState = new GameObject().AddComponent<GameStateScript>();

        // Act - Simulate Pause
        gameState.playPause();

        // Assert
        Assert.AreEqual(0, Time.timeScale, "Time.timeScale should be set to 0 when paused");
        Assert.IsTrue(gameState.isPaused, "isPaused should be true when game is paused");

        // Act - Simulate Unpause
        gameState.playPause();

        // Assert
        Assert.AreNotEqual(0, Time.timeScale, "Time.timeScale should not be 0 when unpaused");
        Assert.IsFalse(gameState.isPaused, "isPaused should be false when game is not paused");
    }

    [Test]
    public void SetNormalSpeed_SetsTimeScaleToOne()
    {
        // Arrange
        var gameState = new GameObject().AddComponent<GameStateScript>();

        // Act
        gameState.setNormalSpeed();

        // Assert
        Assert.AreEqual(1, Time.timeScale, "Time.timeScale should be set to 1 for normal speed");
    }

    [Test]
    public void IncreaseSpeedTen_SetsTimeScaleToTen()
    {
        // Arrange
        var gameState = new GameObject().AddComponent<GameStateScript>();

        // Act
        gameState.increaseSpeedTen();

        // Assert
        Assert.AreEqual(10, Time.timeScale, "Time.timeScale should be set to 10 when increased by ten times");
    }

    [Test]
    public void SimSpeedMaximum_SetsTimeScaleToOneHundred()
    {
        // Arrange
        var gameState = new GameObject().AddComponent<GameStateScript>();

        // Act
        gameState.simSpeedMaximum();

        // Assert
        Assert.AreEqual(100, Time.timeScale, "Time.timeScale should be set to 100 for maximum simulation speed");
    }
}
