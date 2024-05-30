using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Models
{
    public class CelestialBody : MonoBehaviour
    {
        public enum CelestialBodyType
        {
            Sun,
            Planet,
            Moon
        };

        public CelestialBodyType celestType;
        public float orbitingCenterPlanetMass;
        public Vector3 velocity;
        public float mass;
        public float orbitRadius;
        public float ratioToEarthYear = 1;
        private List<CelestialBody> celestialBodies;
        public float rotaionSpeed;

        // Kepler Parameters
        public float perihelion;
        public float eccentricity;
        public float orbitalPeriod;
        public float orbitalSpeed;
        private float currentTime = 0;
        private float gravitationalConstant = 1.1904e-19f;
        private float k;
        private float L;

        // Constants
        private const float AU = 1.496e8f; // kilometers
        private const float SM = 1.989e30f;
        private const float YEAR_S = 365.25f * 24f * 3600f; // seconds
        private const float scaleFactor = 5e-9f; // Unity units per AU

        // Start is called before the first frame update
        void Start()
        {
            celestialBodies = new List<CelestialBody>(FindObjectsOfType<CelestialBody>());

            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer)
            {
                // CONVERSIONS
                perihelion /= AU;
                mass /= SM;
                orbitalSpeed /= AU;

                // Calculate constants k and L
                k = gravitationalConstant * (orbitingCenterPlanetMass / SM) * mass;
                L = mass * perihelion * orbitalSpeed;
            }
            else
            {
                GenerateOrbitLine();
            }
        }

        void FixedUpdate()
        {
            if (!GameStateController.isPaused)
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
                        UpdatePositionByKepler();
                    }
                }

                transform.Rotate(Vector3.down, rotaionSpeed * Time.deltaTime);
            }
        }

        private void UpdateVelocity(CelestialBody planet)
        {
            float gravitationalConstant = 10000000000f;
            float rSqr = (planet.transform.position - transform.position).sqrMagnitude;
            Vector3 forceDir = (planet.transform.position - transform.position).normalized;
            Vector3 force = gravitationalConstant * mass * planet.mass * forceDir / rSqr;
            velocity += force * (float)Math.Pow(10, -12) / mass;
        }

        private void UpdatePosition()
        {
            transform.position += velocity;
        }

        private void UpdatePositionByKepler()
        {
            try
            {
                // Calculate the mean anomaly
                float M = mean_anomaly(currentTime, orbitalPeriod);

                // Calculate the eccentric anomaly
                float E = eccentric_anomaly(M, eccentricity);

                if (float.IsNaN(E))
                {
                    throw new InvalidOperationException("Eccentric Anomaly calculation resulted in NaN");
                }

                // Calculate the true anomaly
                float theta = true_anomaly(E, eccentricity);
                if (float.IsNaN(theta))
                {
                    throw new InvalidOperationException("True Anomaly calculation resulted in NaN");
                }

                // Calculate the radial distance
                float r = radius_of_orbit(L, k, mass, eccentricity, theta);
                if (float.IsNaN(r))
                {
                    throw new InvalidOperationException("Radius of Orbit calculation resulted in NaN");
                }

                // Calculate the position in the orbital plane
                float x = r * Mathf.Cos(theta);
                float z = r * Mathf.Sin(theta);

                x *= scaleFactor;
                z *= scaleFactor;

                Vector3 newPosition = new Vector3(x, 0, z);

                if (float.IsNaN(newPosition.x) || float.IsNaN(newPosition.z))
                {
                    throw new InvalidOperationException($"Computed position is NaN: {newPosition}");
                }

                // Update the position
                transform.position = newPosition;

                // Increment time
                currentTime += Time.deltaTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private float mean_anomaly(float t, float T)
        {
            if (float.IsNaN(t) || float.IsNaN(T) || T == 0)
            {
                throw new InvalidOperationException("Eccentric Anomaly calculation resulted in NaN");
            }

            return 2 * Mathf.PI * (t % T) / T;
        }

        private float eccentric_anomaly(float M, float e)
        {
            if (float.IsNaN(M) || float.IsNaN(e))
            {
                throw new ArgumentException("Input values cannot be NaN.");
            }


            if (e < 0 || e >= 1)
            {
                throw new ArgumentException("Eccentricity must be between 0 and 1 for elliptical orbits.");
            }



            float E = M; // Initial guess: E ≈ M
            int maxIterations = 10;
            float tolerance = 1e-6f;

            for (int i = 0; i < maxIterations; i++) // Newton-Raphson iteration
            {
                float f = M - (E - e * Mathf.Sin(E));
                float fPrime = 1 - e * Mathf.Cos(E);
                if (Mathf.Abs(fPrime) < Mathf.Epsilon)  // Check to avoid division by zero
                {
                    throw new InvalidOperationException("Denominator in Newton-Raphson method became too small, preventing convergence.");
                }

                float deltaE = f / fPrime;
                E += deltaE;

                if (Mathf.Abs(deltaE) < tolerance)
                {
                    return E;
                }
            }
            throw new InvalidOperationException("Eccentric anomaly calculation failed to converge.");
        }


        private float true_anomaly(float E, float e)
        {
            return 2 * Mathf.Atan2(Mathf.Sqrt(1 + e) * Mathf.Sin(E / 2), Mathf.Sqrt(1 - e) * Mathf.Cos(E / 2));
        }

        private float radius_of_orbit(float L, float k, float m, float epsilon, float theta)
        {
            return (L * L / (k * m)) / (1 + epsilon * Mathf.Cos(theta));
        }

        public Vector3[] GetOrbitLinePoints()
        {
            int pointsLentgth = 360;
            Vector3[] points = new Vector3[pointsLentgth];

            for (int i = 0; i < pointsLentgth; i++)
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
            return transform.position;
        }

        //getter and setter methods
        public CelestialBodyType GetCelestialBodyType() => celestType;
        public void SetCelestialBodyType(CelestialBodyType type) => celestType = type;

        public float GetMass() => mass;
        public void SetMass(float value) => mass = value;

        public float GetOrbitRadius() => orbitRadius;
        public void SetOrbitRadius(float radius) => orbitRadius = radius;

        public float GetRatioToEarthYear() => ratioToEarthYear;
        public void SetRatioToEarthYear(float ratio) => ratioToEarthYear = ratio;

        public void SetVelocity(Vector3 velocity) => this.velocity = velocity;
        public Vector3 GetVelocity() => velocity;

        public List<CelestialBody> GetCelestialBodies() => celestialBodies;
        public void SetCelesitalBodies(List<CelestialBody> celestialBodies) => this.celestialBodies = celestialBodies;
    }
}