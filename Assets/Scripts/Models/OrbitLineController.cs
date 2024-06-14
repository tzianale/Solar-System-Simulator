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
                Vector3[] orbitPoints = celestialBody.GetOrbitLinePoints();
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

        /// <summary>
        /// Initializes or resets the settings of the LineRenderer component attached to the GameObject.
        /// </summary>
        /// <param name="count">The number of positions to set for the LineRenderer.</param>
        public void InitializeLineRenderer(int count)
        {
            // Ensure the lineRenderer component is attached and correctly set up
            if (lineRenderer == null)
                lineRenderer = gameObject.GetComponent<LineRenderer>();

            // Initialize or reset the LineRenderer's settings
            lineRenderer.positionCount = 0;  // Reset any existing data
            lineRenderer.positionCount = count;  // Set the expected number of positions
            lineRenderer.startWidth = 0.1f;  // Set the width of the line at the start
            lineRenderer.endWidth = 0.1f;  // Set the width of the line at the end
            lineRenderer.useWorldSpace = true;  // Use world space coordinates

            // Optional: Configure materials and colors
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;

            // Initialize the positions array if needed
            Vector3[] positions = new Vector3[count];
            for (int i = 0; i < count; i++)
                positions[i] = Vector3.zero;  // Initialize all positions to zero

            lineRenderer.SetPositions(positions);  // Apply the initialized positions
        }

    }
}