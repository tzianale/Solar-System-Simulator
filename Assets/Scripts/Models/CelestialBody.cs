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
        public float sideRealRotationPeriod;

        // Kepler Parameters
        // public float perihelion;
        public float semiMajorAxis; // in AU
        public float eccentricity;
        public float orbitalPeriod;
        // public float orbitalSpeed;
        public float inclination;
        public float longitudeOfAscendingNode;
        public float argumentOfPerihelion;
        public float obliquityToOrbit;
        private float currentTime = 0;
        private float gravitationalConstant = 1.1904e-19f;
        private float k_const;
        private float L_const;

        // Constants
        private const float AU = 1.496e8f; // kilometers
        private const float SM = 1.989e30f;
        private const float scaleFactor = 1000f;

        // Start is called before the first frame update
        void Start()
        {
            celestialBodies = new List<CelestialBody>(FindObjectsOfType<CelestialBody>());
            transform.Rotate(Vector3.right, -obliquityToOrbit);
            sideRealRotationPeriod = 360f / sideRealRotationPeriod;

            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer)
            {
                // CONVERSIONS
                // perihelion /= AU;
                mass /= SM;
                // orbitalSpeed /= AU;

                // Forward to current time
                if (celestType != CelestialBodyType.Sun)
                {
                    UpdatePositionByKepler();
                    UpdateRotation(true, (float)GameStateController.explorerModeDay);
                    transform.Rotate(Vector3.up, -90f, Space.Self);
                    // GenerateOrbitLine();
                }
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
                    Debug.Log("lol");
                    foreach (CelestialBody planet in celestialBodies)
                    {
                        if (planet != this)
                        {
                            UpdateVelocity(planet);
                            Debug.Log("UPDATING");
                        }
                    }

                    UpdatePosition();
                    UpdateRotation();
                }
                else
                {
                    var currentExplorerTimeStep = (float)GameStateController.currentExplorerTimeStep;
                    if (celestType != CelestialBodyType.Sun)
                    {
                        UpdatePositionByKepler();
                        UpdateRotation(true, currentExplorerTimeStep);
                    }
                }
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

        private void UpdateRotation(bool isExplorerMode = false, float currentExplorerTimeStep = 0)
        {

            if (isExplorerMode)
            {

                transform.Rotate(Vector3.up, -sideRealRotationPeriod * currentExplorerTimeStep * 24, Space.Self);
            } else
            {
                transform.Rotate(Vector3.up, -sideRealRotationPeriod * Time.deltaTime, Space.Self);
            }
        }

        private void UpdatePositionByKepler()
        {
            try
            {
                currentTime = (float)GameStateController.explorerModeDay;
    
                // Calculate the mean anomaly
                float M = mean_anomaly(currentTime, orbitalPeriod);

                // Calculate the eccentric anomaly
                float E = eccentric_anomaly(M, eccentricity);

                // Calculate the true anomaly
                float theta = true_anomaly(E, eccentricity);

                // Calculate the radial distance
                // float r = radius_of_orbit(L, k, mass, eccentricity, theta);
                float r = semiMajorAxis * (1 - eccentricity * Mathf.Cos(E));

                // Calculate position in perifocal coordinates
                float x = r * Mathf.Cos(theta);
                float z = r * Mathf.Sin(theta);
                Vector3 perifocalPosition = new Vector3(x, 0, z);

                // Transform to global coordinates
                Vector3 globalPosition = ApplyOrbitalElements(perifocalPosition, inclination, longitudeOfAscendingNode, argumentOfPerihelion);
                // Vector3 globalPosition = perifocalPosition;

                // Scale the position to make it more manageable in Unity
                globalPosition *= scaleFactor;


                // Update position relative to the orbiting center (e.g., Sun)
                Vector3 orbitingCenterPosition = Vector3.zero; // Assuming Sun is at origin
                transform.position = orbitingCenterPosition + globalPosition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private float mean_anomaly(float t, float T)
        {
            Debug.Log("mean_anomaly: t = " + t + ", and T = " + T);
            // return 2 * Mathf.PI * (t % T) / T;
            float n = (2 * Mathf.PI) / T; // Mean motion (rad/day)
            return n * (t % T); // Mean anomaly
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
            int maxIterations = 20;
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
            float sinE = Mathf.Sin(E);
            float cosE = Mathf.Cos(E);
            return 2 * Mathf.Atan2(Mathf.Sqrt(1 + e) * Mathf.Sin(E / 2), Mathf.Sqrt(1 - e) * Mathf.Cos(E / 2));
            // return Mathf.Asin(Mathf.Sqrt(1 - Mathf.Pow(e, 2) * sinE) / (1 - e * cosE));
            // return (cosE - e) / (1 - e * cosE);
            // return 2 * Mathf.Atan2(Mathf.Sqrt(1 + e) * sinE, Mathf.Sqrt(1 - e) * cosE);
        }

        private float radius_of_orbit(float L, float k, float m, float epsilon, float theta)
        {
            Debug.Log("THIS SHOULDN'T BE USED ANYMORE!");
            return (L * L / (k * m)) / (1 + epsilon * Mathf.Cos(theta));
        }

        private Vector3 ApplyOrbitalElements(Vector3 perifocalPosition, float inclination, float longitudeOfAscendingNode, float argumentOfPerihelion)
        {
            // Convert angles from degrees to radians
            float iRad = inclination * Mathf.Deg2Rad;
            float ΩRad = longitudeOfAscendingNode * Mathf.Deg2Rad;
            float ωRad = argumentOfPerihelion * Mathf.Deg2Rad;

            // Rotation matrices
            Matrix4x4 Rω = Matrix4x4.Rotate(Quaternion.Euler(0, ωRad * Mathf.Rad2Deg, 0));  // Rotate around y-axis
            Matrix4x4 Ri = Matrix4x4.Rotate(Quaternion.Euler(iRad * Mathf.Rad2Deg, 0, 0));  // Rotate around x-axis
            Matrix4x4 RΩ = Matrix4x4.Rotate(Quaternion.Euler(0, ΩRad * Mathf.Rad2Deg, 0));  // Rotate around y-axis

            // Combined rotation matrix
            Matrix4x4 rotationMatrix = RΩ * Ri * Rω;

            // Apply rotation
            Vector3 globalPosition = rotationMatrix.MultiplyPoint3x4(perifocalPosition);

            return globalPosition;
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