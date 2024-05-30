using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class CelestialBodyGenerator : MonoBehaviour
    {
        public static GameObject CreateNewCelestialBodyGameObject(string name, CelestialBody.CelestialBodyType type, Vector3 position, float mass, float diameter, Vector3 velocity, Color color)
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
            Material material = new Material(Shader.Find("Standard"));
            material.color = color;
            gameObject.GetComponent<Renderer>().material = material;

            AddNewCelestialBodyToGameObject(gameObject, type, velocity, mass);

            return gameObject;
        }

        private static CelestialBody AddNewCelestialBodyToGameObject(GameObject gameObject,
            CelestialBody.CelestialBodyType type, Vector3 velocity, float mass)
        {
            CelestialBody newBody = gameObject.AddComponent<CelestialBody>();

            newBody.SetMass(mass);
            newBody.SetVelocity(velocity);
            newBody.SetCelestialBodyType(type);
            List<CelestialBody> bodies = newBody.GetCelestialBodies() ?? new List<CelestialBody>(FindObjectsOfType<CelestialBody>());;

            foreach (CelestialBody body in bodies)
            {
                body.SetCelesitalBodies(bodies);
            }

            return newBody;
        }
    }
}