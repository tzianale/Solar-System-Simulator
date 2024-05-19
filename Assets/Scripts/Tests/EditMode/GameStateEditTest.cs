using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class GameStateScriptTests
    {
        [Test]
        public void PlayPause_TogglesTimeScaleCorrectly()
        {
            // Arrange
            var gameState = new GameObject().AddComponent<GameStateController>();
    
            // Act - Simulate Pause
            gameState.playPause();
    
            // Assert
            Assert.IsTrue(GameStateController.isPaused, "isPaused should be true when game is paused");
    
            // Act - Simulate Unpause
            gameState.playPause();
    
            // Assert
            Assert.IsFalse(GameStateController.isPaused, "isPaused should be false when game is not paused");
        }
    
        [Test]
        public void SetNormalSpeed_SetsTimeScaleToOne()
        {
            // Arrange
            var gameState = new GameObject().AddComponent<GameStateController>();
    
            // Act
            gameState.setTimeScale(1);
    
            // Assert
            Assert.AreEqual(1, Time.timeScale, "Time.timeScale should be set to 1 for normal speed");
        }
    
        [Test]
        public void IncreaseSpeedTen_SetsTimeScaleToTen()
        {
            // Arrange
            var gameState = new GameObject().AddComponent<GameStateController>();
    
            // Act
            gameState.setTimeScale(10);
    
            // Assert
            Assert.AreEqual(10, Time.timeScale, "Time.timeScale should be set to 10 when increased by ten times");
        }
    
        [Test]
        public void SimSpeedMaximum_SetsTimeScaleToOneHundred()
        {
            // Arrange
            var gameState = new GameObject().AddComponent<GameStateController>();
    
            // Act
            gameState.setTimeScale(100);
    
            // Assert
            Assert.AreEqual(100, Time.timeScale, "Time.timeScale should be set to 100 for maximum simulation speed");
        }

        [OneTimeTearDown]
        public void RevertChanges()
        {
            Time.timeScale = 1;
        }
    }
}

