using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public enum CelestialBodyType { Sun, Planet, Moon };
    Rigidbody rb;
    private CelestialBodyType celestType;
    private Vector3 velocity;
    private float mass;
    private bool isGravityOn;
    private int day;
    private float orbitRadius;
    private float ratioToEarthYear = 1;
    private CelestialBody[] celestialBodies;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        celestialBodies = FindObjectsOfType<CelestialBody>();

        GenerateOrbitLine();
    }

    void FixedUpdate()
    {   if (!GameStateController.isPaused)
        {
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Sandbox)
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
        float x = (float) Math.Cos(2 * Math.PI / (365.256363004 * ratioToEarthYear) * GameStateController.explorerModeDay) * orbitRadius;
        float z = (float) Math.Sin(2 * Math.PI / (365.256363004 * ratioToEarthYear) * GameStateController.explorerModeDay) * orbitRadius;
        rb.MovePosition(new Vector3(x, 0, z));
    }

    public Vector3[] GetOrbitLinePoints()
    {
        int pointsLentgth = 360;
        Vector3[] points = new Vector3[pointsLentgth];

        for (int i = 0; i < pointsLentgth;  i++)
        {
            float x = (float)Math.Cos(2 * Math.PI / pointsLentgth * i) * orbitRadius;
            float z = (float)Math.Sin(2 * Math.PI / pointsLentgth * i) * orbitRadius;
            points[i] = new Vector3(x, 0, z);
        }

        return points;
    }

    private void GenerateOrbitLine()
    {
        GameObject orbitLineGameObject = new GameObject("OrbitLine");
        orbitLineGameObject.transform.SetParent(transform);

        LineRenderer lineRenderer = orbitLineGameObject.AddComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 10;

        OrbitLineController orbitLineController = orbitLineGameObject.AddComponent<OrbitLineController>();
        orbitLineController.CelestialBody = this;
    }

    public Vector3 getPostion()
    {
        return GetComponent<Transform>().position;
    }
    
    // getter- und setter-methods
    public CelestialBodyType GetCelestialBodyType() => celestType;
    public void SetCelestialBodyType(CelestialBodyType type) => celestType = type;

    public float GetMass() => mass;
    public void SetMass(float value) => mass = value;

    public bool IsGravityEnabled() => isGravityOn;
    public void SetGravityEnabled(bool enabled) => isGravityOn = enabled;

    public int GetDayOfYear() => day;
    public void SetDayOfYear(int dayOfYear) => day = dayOfYear;

    public float GetOrbitRadius() => orbitRadius;
    public void SetOrbitRadius(float radius) => orbitRadius = radius;

    public float GetRatioToEarthYear() => ratioToEarthYear;
    public void SetRatioToEarthYear(float ratio) => ratioToEarthYear = ratio;

}
