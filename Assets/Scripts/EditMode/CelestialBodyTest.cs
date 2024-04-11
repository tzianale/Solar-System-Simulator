using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CelestialBodyTest
{
    
    // A Test behaves as an ordinary method
    [Test]
public void CelestialBody_InitializesCorrectly()
{
    //Arange
    // Erstelle ein GameObject und füge das CelestialBody Skript hinzu
    var gameObject = new GameObject();
    var celestialBody = gameObject.AddComponent<CelestialBody>();
    celestialBody.mass = 5.0f;
    celestialBody.day = 1;
    celestialBody.orbitRadius = 100f;
    celestialBody.ratioToEarthYear = 1f;

    //Act
    // Führe die Initialisierung durch (z.B. indem du die Start-Methode direkt aufrufst, wenn sie public ist)
    // Da Start private ist und bei der Komponenteninitialisierung aufgerufen wird, könnten wir hier direkt testen
    var rb = gameObject.GetComponent<Rigidbody>();

    //Assert
    // Überprüfe, ob die Komponenten wie erwartet initialisiert wurden
    Assert.IsNotNull(rb, "Rigidbody wurde nicht korrekt initialisiert");
    Assert.AreEqual(5.0f, celestialBody.mass, "Die Masse wurde nicht korrekt gesetzt");
}
}
