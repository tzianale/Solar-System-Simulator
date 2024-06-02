using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class PlanetDatabaseTests
    {
        private List<string> logOutput;

        [SetUp]
        public void SetUp()
        {
            // Initialize the log output list and subscribe to the log message event
            logOutput = new List<string>();
            Application.logMessageReceived += LogMessageReceived;
        }

        [TearDown]
        public void TearDown()
        {
            // Unsubscribe from the log message event to clean up
            Application.logMessageReceived -= LogMessageReceived;
            logOutput = null;
        }

        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            logOutput.Add(condition);
        }

        [Test]
        public void TestPrintKeys()
        {
            // Call the PrintKeys method
            PlanetDatabase.PrintKeys();

            // Check the log output contains all the planet keys
            foreach (var key in PlanetDatabase.Planets.Keys)
            {
                Assert.IsTrue(logOutput.Contains($"Planet Key: {key}"), $"Log output does not contain expected key: {key}");
            }
        }
    }
}