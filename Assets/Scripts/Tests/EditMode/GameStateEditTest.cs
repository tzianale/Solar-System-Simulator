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
        private TextMeshProUGUI dayText;
        private TextMeshProUGUI simulationSpeedText;
        private Slider simulationSpeedSlider;
        private Toggle realTimeToggle;

        [SetUp]
        public void Setup()
        {
            // Setup the game object and add required components
            gameObject = new GameObject();
            gameStateController = gameObject.AddComponent<GameStateController>();

            // Adding required components
            dayText = new GameObject().AddComponent<TextMeshProUGUI>();
            simulationSpeedText = new GameObject().AddComponent<TextMeshProUGUI>();
            simulationSpeedSlider = new GameObject().AddComponent<Slider>();
            realTimeToggle = new GameObject().AddComponent<Toggle>();

            // Adding mock buttons for coloring
            var playPauseButton = new GameObject("Play / Pause").AddComponent<Image>();
            var forwardButton = new GameObject("ForwardButton").AddComponent<Image>();
            var backwardButton = new GameObject("BackwardButton").AddComponent<Image>();

            // Assigning components to the GameStateController
            gameStateController.dayText = dayText;
            gameStateController.simulationSpeedText = simulationSpeedText;
            gameStateController.simulationSpeedSlider = simulationSpeedSlider;
            gameStateController.realTimeToggle = realTimeToggle;

            // Initial setup for slider
            simulationSpeedSlider.minValue = 0;
            simulationSpeedSlider.maxValue = 10;
            simulationSpeedSlider.value = 1;
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
            Assert.IsFalse(GameStateController.isPaused, "Initially, isPaused should be false");

            // Act - Pause the game
            gameStateController.playPause();
            // Assert - Check if game is paused
            Assert.IsTrue(GameStateController.isPaused, "isPaused should be true after pausing");

            // Act - Unpause the game
            gameStateController.playPause();
            // Assert - Check if game is not paused
            Assert.IsFalse(GameStateController.isPaused, "isPaused should be false after unpausing");
        }

        [Test]
        public void SetTimeScale_SetsCorrectly()
        {
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Sandbox;

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
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Sandbox;

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
