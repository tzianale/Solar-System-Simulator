using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CelestialBodyGenerator : MonoBehaviour
{

    public static GameObject CreateNewCelestialBodyGameObject(string name, CelestialBody.CelestialBodyType type, Vector3 position, float mass, float diameter, Vector3 velocity)
    {
        // Validate name
    if (string.IsNullOrEmpty(name))
        throw new ArgumentException("Name cannot be null or empty.", nameof(name));
    
    // Validate mass
    if (mass <= 0)
        throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
    
    // Validate diameter
    if (diameter <= 0)
        throw new ArgumentException("Diameter must be greater than zero.", nameof(diameter));

        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.transform.localScale = new Vector3(diameter, diameter, diameter);
        gameObject.transform.position = position;
        gameObject.name = name;

        AddNewCelestialBodyToGameObject(gameObject, type, velocity, mass);

        return gameObject;
    }

    private static CelestialBody AddNewCelestialBodyToGameObject(GameObject gameObject, CelestialBody.CelestialBodyType type, Vector3 velocity, float mass)
    {
        CelestialBody newBody = gameObject.AddComponent<CelestialBody>();

        newBody.SetMass(mass);
        newBody.SetVelocity(velocity);
        newBody.SetCelestialBodyType(type);
        List<CelestialBody> bodies = newBody.GetCelestialBodies() ?? new List<CelestialBody>();

        foreach (CelestialBody body in bodies)
        {
            body.SetCelesitalBodies(bodies);
        }
        return newBody;
    }
}
