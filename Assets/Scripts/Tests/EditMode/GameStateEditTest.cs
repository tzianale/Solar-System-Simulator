using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Models; 

namespace Tests.EditMode
{
    public class GameStateControllerTests
    {
        private GameObject gameObject;
        private GameStateController gameStateController;

        [SetUp]
        public void Setup()
        {
            // Setup the game object and add required components
            gameObject = new GameObject();
            gameStateController = gameObject.AddComponent<GameStateController>();
        }

        [TearDown]
        public void Teardown()
        {
            // Cleanup the objects created for the test
            if (gameObject != null)
                Object.DestroyImmediate(gameObject);

            // Reset Time.timeScale to avoid side effects in other tests
            Time.timeScale = 1;
        }

        [Test]
        public void PlayPause_ShouldToggleIsPaused()
        {
            // Arrange
            var gameState = new GameObject().AddComponent<GameStateController>();
            Assert.IsFalse(GameStateController.isPaused, "Initially, isPaused should be false");

            // Act - Pause the game
            gameState.playPause();
            // Assert - Check if game is paused
            Assert.IsTrue(GameStateController.isPaused, "isPaused should be true after pausing");

            // Act - Unpause the game
            gameState.playPause();
            // Assert - Check if game is not paused
            Assert.IsFalse(GameStateController.isPaused, "isPaused should be false after unpausing");
        }

        [Test]
        public void SetTimeScale_SetsCorrectly()
        {
            // Test setting normal speed
            gameStateController.setTimeScale(1);
            Assert.AreEqual(1, Time.timeScale, "Time.timeScale should be set to 1 for normal speed.");

            // Test increasing speed
            gameStateController.setTimeScale(5);
            Assert.AreEqual(5, Time.timeScale, "Time.timeScale should be set to 5 when increased.");

            // Test maximum speed
            gameStateController.setTimeScale(10);
            Assert.AreEqual(10, Time.timeScale, "Time.timeScale should be set to 10 for maximum speed.");
        }

        [Test]
        public void ResetTimeScale_ResetsToDefaultAfterPause()
        {
            // Set to a specific time scale
            gameStateController.setTimeScale(2);
            Assert.AreEqual(2, Time.timeScale, "Time.timeScale should initially be set to 2.");

            // Pause the game
            gameStateController.playPause();
            gameStateController.setTimeScale(1); // Reset to normal when paused

            // Resume the game
            gameStateController.playPause();
            Assert.AreEqual(1, Time.timeScale, "Time.timeScale should reset to 1 after resuming the game.");
        }
    }
}
