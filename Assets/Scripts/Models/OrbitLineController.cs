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
            get { return celestialBody; }
            set { celestialBody = value; }
        }

        // Start is called before the first frame update
        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Explorer)
            {
                Vector3[] orbitPoints = celestialBody.GetExplorerLinePoints();
                lineRenderer.positionCount = orbitPoints.Length;
                lineRenderer.SetPositions(orbitPoints);
            }
            else
            {
                last100Points = new Vector3[100];
                Vector3 celestialBodyIntialPostion = celestialBody.GetPosition();
                for (int i = 0; i < last100Points.Length; i++)
                {
                    last100Points[i] = celestialBodyIntialPostion;
                }

                lineRenderer.loop = false;
            }
        }

        private void FixedUpdate()
        {
            if (SimulationModeState.currentSimulationMode == SimulationModeState.SimulationMode.Sandbox)
            {
                for (int i = last100Points.Length - 1; i > 0; i--)
                {
                    last100Points[i] = last100Points[i - 1];
                }

                last100Points[0] = celestialBody.GetPosition();

                lineRenderer.positionCount = last100Points.Length;
                lineRenderer.SetPositions(last100Points);
            }
        }

        // TODO DOCS
        public void UpdateLineWidth(float zoomScale)
        {
            if (lineRenderer != null)
            {
                lineRenderer.widthMultiplier = Mathf.Lerp(1.0f, 100.0f, zoomScale);
            }
        }
    }
}