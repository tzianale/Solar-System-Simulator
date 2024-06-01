using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Models;
using System.Collections;
using System;
using UnityEngine.UIElements;

namespace Tests.PlayMode
{
    public class OrbitLineControllerTests
    {
        private GameObject gameObject;
        private OrbitLineController orbitLineController;
        private LineRenderer lineRenderer;
        private CelestialBody celestialBody;

        [SetUp]
        public void Setup()
        {
            gameObject = new GameObject();
            orbitLineController = gameObject.AddComponent<OrbitLineController>();
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            celestialBody = gameObject.AddComponent<CelestialBody>();
            orbitLineController.CelestialBody = celestialBody;

            // Initialize LineRenderer with a capacity for 100 positions
            orbitLineController.InitializeLineRenderer(100);
        }

        // Use UnityTest attribute for coroutine tests
        [UnityTest]
        public IEnumerator Start_SetsUpLineRenderer_WhenInExplorerMode()
        {
            // Setup required initial conditions
            celestialBody.SetOrbitRadius(100); // Example setup
            celestialBody.SetVelocity(new Vector3(1, 0, 1)); // Example velocity

            // Set the mode to trigger correct behavior in Start()
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Explorer;

            // Ensure the GameObject is active
            gameObject.SetActive(true);

            // Wait one frame to allow Start() to be called
            yield return null;

            // Test conditions after Start() has been called
            Assert.Greater(lineRenderer.positionCount, 0);
        }

        [UnityTest]
        public IEnumerator FixedUpdate_UpdatesPositions_WhenInSandboxMode()
        {
            // Change to Sandbox Mode
            SimulationModeState.currentSimulationMode = SimulationModeState.SimulationMode.Sandbox;

            // Initialize CelestialBody
            celestialBody.transform.position = Vector3.zero; // Initial position
            celestialBody.SetVelocity(new Vector3(1, 0, 0)); // Set velocity

            // Activate GameObject and its components
            gameObject.SetActive(true);

            // Set up the line renderer to store last 100 positions
            orbitLineController.InitializeLineRenderer(100); // Assume we have this method to set up line renderer

            // Simulate several FixedUpdate cycles
            float timeToSimulate = 2.02f; // 2.02 seconds
            float startTime = Time.fixedTime;
            while (Time.fixedTime < startTime + timeToSimulate)
            {
                yield return new WaitForFixedUpdate();
            }
            
            // Assert that the LineRenderer's last position is not zero after movement
            Vector3 lastPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            Assert.AreNotEqual(Vector3.zero, lastPosition, "Last position of the line renderer should not be zero.");

            // Optional: Check if all positions are updated or at least the array is filled
            Assert.IsTrue(lineRenderer.positionCount >= 100, "LineRenderer should have at least 100 points after 2 seconds of simulation.");
        }

        // Test für die InitializeLineRenderer Methode
        [Test]
        public void InitializeLineRenderer_SetsCorrectProperties()
        {
            // Anzahl der Positionen, die initialisiert werden sollen
            int expectedCount = 100;

            // Rufe die InitializeLineRenderer Methode auf
            orbitLineController.InitializeLineRenderer(expectedCount);

            // Überprüfe, ob die Positionen korrekt gesetzt sind
            Assert.AreEqual(expectedCount, lineRenderer.positionCount, "LineRenderer should have the correct number of positions.");

            // Überprüfe weitere Eigenschaften wie Linienbreite und Material
            Assert.AreEqual(0.1f, lineRenderer.startWidth, "LineRenderer should have the correct start width.");
            Assert.AreEqual(0.1f, lineRenderer.endWidth, "LineRenderer should have the correct end width.");
            Assert.IsTrue(lineRenderer.useWorldSpace, "LineRenderer should use world space coordinates.");
            Assert.IsNotNull(lineRenderer.material, "LineRenderer should have a material set.");
            Assert.AreEqual(Color.white, lineRenderer.startColor, "LineRenderer should have the correct start color.");
            Assert.AreEqual(Color.white, lineRenderer.endColor, "LineRenderer should have the correct end color.");
        }


        [TearDown]
        public void Teardown()
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }
}