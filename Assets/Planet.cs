using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 velocity;
    public float gravitationalPull;
    public float mass;
    private Planet[] planets;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        planets = FindObjectsOfType<Planet>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Planet planet in planets)
        {
            if (planet != this)
            {
                UpdateVelocity(planet);
            }
        }
        UpdatePosition();
    }

    private void UpdateVelocity(Planet planet)
    {
        float gravitationalConstant = 0.00001f;
        float rSqr = (planet.rb.position - rb.position).sqrMagnitude;
        Vector3 forceDir = (planet.rb.position - rb.position).normalized;
        Vector3 force = forceDir * gravitationalConstant * mass * planet.mass / rSqr;
        velocity += force / mass;
    }

    private void UpdatePosition()
    {   
        rb.position += velocity;
    }

}
