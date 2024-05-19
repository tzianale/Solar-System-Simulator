using UnityEngine;

namespace Models
{
    public class OrbitLineController : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        [SerializeField] private CelestialBody celestialBody;
        private Vector3[] last100Points = new Vector3[100];

        public CelestialBody CelestialBody
        {
            set { celestialBody = value; }
        }

        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer)
            {
                Vector3[] orbitPoints = celestialBody.GetOrbitLinePoints();
                lineRenderer.positionCount = orbitPoints.Length;
                lineRenderer.SetPositions(orbitPoints);
            } else
            {
                last100Points = new Vector3[100];
                Vector3 celestialBodyIntialPostion = celestialBody.getPostion();
                for (int i = 0; i < last100Points.Length; i++)
                {
                    last100Points[i] = celestialBodyIntialPostion;
                }
                lineRenderer.loop = false;
            }

        }

        void FixedUpdate()
        {
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Sandbox)
            {
                for (int i = last100Points.Length - 1; i > 0; i--)
                {
                    last100Points[i] = last100Points[i - 1];
                }
                last100Points[0] = celestialBody.getPostion();

                lineRenderer.positionCount = last100Points.Length;
                lineRenderer.SetPositions(last100Points);
            }
        }
    }
}
