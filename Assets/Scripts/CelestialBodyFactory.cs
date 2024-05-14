using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyFactory : MonoBehaviour
{

    public static CelestialBody CreateNewCelestialBodyGameObject(string name, CelestialBody.CelestialBodyType type, Vector3 position, float mass, float radius, Vector3 velocity)
    {
        GameObject gameObject = new GameObject();
        CelestialBody celestialBody = AddNewCelestialBodyToGameObject(gameObject, type, velocity, mass);
        return celestialBody;
    }

    private static CelestialBody AddNewCelestialBodyToGameObject(GameObject gameObject, CelestialBody.CelestialBodyType type, Vector3 velocity, float mass)
    {
        CelestialBody celestialBody = gameObject.AddComponent<CelestialBody>();
        celestialBody.SetCelestialBodyType(type);
        celestialBody.SetVelocity(velocity);
        celestialBody.SetMass(mass);
        return celestialBody;
    }
}
