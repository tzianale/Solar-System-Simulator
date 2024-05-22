using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyGenerator : MonoBehaviour
{

    public static void CreateNewCelestialBodyGameObject(string name, CelestialBody.CelestialBodyType type, Vector3 position, float mass, float diameter, Vector3 velocity)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.transform.localScale = new Vector3(diameter, diameter, diameter);
        gameObject.transform.position = position;
        gameObject.name = name;

        AddNewCelestialBodyToGameObject(gameObject, type, velocity, mass);
    }

    private static void AddNewCelestialBodyToGameObject(GameObject gameObject, CelestialBody.CelestialBodyType type, Vector3 velocity, float mass)
    {
        CelestialBody newBody = gameObject.AddComponent<CelestialBody>();
        newBody.SetMass(mass);
        newBody.SetVelocity(velocity);
        newBody.SetCelestialBodyType(type);
        List<CelestialBody> bodies = newBody.GetCelestialBodies();

        foreach (CelestialBody body in bodies)
        {
            body.SetCelesitalBodies(bodies);
        }
    }
}
