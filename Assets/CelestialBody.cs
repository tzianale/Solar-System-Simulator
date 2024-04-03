using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public enum CelestialBodyType { Sun, Planet, Moon };
    Rigidbody rb;
    public CelestialBodyType celestType;
    public Vector3 velocity;
    public float mass;
    public bool isGravityOn;
    public int day;
    public float orbitRadius;
    public float ratioToEarthYear = 1;
    private CelestialBody[] celestialBodies;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        celestialBodies = FindObjectsOfType<CelestialBody>();
    }

    void FixedUpdate()
    {
        if (isGravityOn)
        {
            foreach (CelestialBody planet in celestialBodies)
            {
                if (planet != this)
                {
                    UpdateVelocity(planet);
                }
            }
            UpdatePosition();
        }
        else
        {
            if (celestType != CelestialBodyType.Sun)
            {
                UpdatePositionByDate();
            }
        }
    }

    private void UpdateVelocity(CelestialBody planet)
    {
        float gravitationalConstant = 6.67f * (float) Math.Pow(10, -6);
        float rSqr = (planet.rb.position - rb.position).sqrMagnitude;
        Vector3 forceDir = (planet.rb.position - rb.position).normalized;
        Vector3 force = forceDir * gravitationalConstant * mass * planet.mass / rSqr;
        velocity += force * (float)Math.Pow(10, -12) / mass;
    }

    private void UpdatePosition()
    {
        rb.position += velocity;
    }

    private void UpdatePositionByDate()
    {
        float x = (float) Math.Cos(2 * Math.PI / (365.256363004 * ratioToEarthYear) * day) * orbitRadius;
        float z = (float) Math.Sin(2 * Math.PI / (365.256363004 * ratioToEarthYear) * day) * orbitRadius;
        rb.MovePosition(new Vector3(x, 0, z));
    }

}
