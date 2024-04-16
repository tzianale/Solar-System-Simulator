using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitLineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private CelestialBody celestialBody;

    public CelestialBody CelestialBody
    {
        set { celestialBody = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3[] orbitPoints = celestialBody.GetOrbitLinePoints();
        lineRenderer.positionCount = orbitPoints.Length;
        lineRenderer.SetPositions(orbitPoints);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
