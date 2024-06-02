using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControlV2 : MonoBehaviour
{
    [SerializeField]
    private GameObject sun;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject camHolder;

    [SerializeField]
    private GameObject neptune;

    [SerializeField]
    private float minDistanceToSurfaceRadiusFactor;

    private Vector3 pivotPoint;

    private bool followModeActive = false;
    private Transform followTarget;

    private float camToPivotDistance;
    private Vector3 camToPivotDirection;


    // Defaults
    private float defaultCamToPivotDistance;
    private Vector3 defaultCamToPivotDirection;
    private Quaternion defaultRotation;


    // Zoom stuff
    private float zoomStrength = 1f;
    private float minZoomDistance = 10.0f;
    private float maxZoomDistance;

    // Rotation stuff
    private Vector3 lastMousePosition;
    private float rotationSpeed = 1f;

    // Variables for panning
    private Vector3 initialMousePosition;
    private Vector3 initialPivotPoint;
    private bool panMode = false;

    // Keymappings
    private Dictionary<KeyCode, System.Action> keyMappings;

    void Start()
    {
        InitializeKeyMappings();

        maxZoomDistance = neptune.transform.position.magnitude * 2;

        pivotPoint = sun.transform.position;

        defaultRotation = camHolder.transform.rotation;
        var camToPivotCombined = camHolder.transform.position - pivotPoint;
        camToPivotDirection = camToPivotCombined.normalized;
        defaultCamToPivotDirection = camToPivotDirection;
        camToPivotDistance = camToPivotCombined.magnitude;
        defaultCamToPivotDistance = camToPivotDistance;
    }

    void Update()
    {

        if (followModeActive)
        {
            FollowTarget();
        }

        else
        {
            HandlePan();
        }

        HandleDefaultMovement();

        UpdateCameraPosition();
    }

    private void InitializeKeyMappings()
    {
        keyMappings = new Dictionary<KeyCode, System.Action>
        {
            { KeyCode.Keypad1, () => SetFixedView(Vector3.up) }, // np1
            { KeyCode.Alpha1, () => SetFixedView(Vector3.up) }, // kb1
            { KeyCode.Keypad3, () => SetFixedView(Vector3.forward) },
            { KeyCode.Alpha3, () => SetFixedView(Vector3.forward) },
            { KeyCode.Keypad7, () => SetFixedView(new Vector3(1, 1, 1)) },
            { KeyCode.Alpha7, () => SetFixedView(new Vector3(1, 1, 1)) },
            { KeyCode.R, () => ResetCameraView() },
            { KeyCode.Keypad6, () => rotateCam45DegRight() },
            { KeyCode.Alpha6, () => rotateCam45DegRight() },
            { KeyCode.Keypad4, () => rotateCam45DegLeft() },
            { KeyCode.Alpha4, () => rotateCam45DegLeft() },
            { KeyCode.Keypad8, () => rotateCam45DegUp() },
            { KeyCode.Alpha8, () => rotateCam45DegUp() },
            { KeyCode.Keypad2, () => rotateCam45DegDown() },
            { KeyCode.Alpha2, () => rotateCam45DegDown() }
        };
    }

    private void CheckForKeyPresses()
    {
        foreach (var keyMapping in keyMappings)
        {
            if (Input.GetKeyDown(keyMapping.Key))
            {
                keyMapping.Value.Invoke();
            }
        }
    }

    private void FollowTarget()
    {
        pivotPoint = followTarget.position;
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                initialMousePosition = Input.mousePosition;
                initialPivotPoint = pivotPoint;
                panMode = true;
            }
            else
            {
                panMode = false;
            }


        }

        if (Input.GetMouseButton(0) && panMode)
        {
            Vector3 currentMousePosition = Input.mousePosition;

            Ray ray = cam.ScreenPointToRay(initialMousePosition);
            Plane plane = new Plane(cam.transform.forward, initialPivotPoint);
            float distance;

            if (plane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                ray = cam.ScreenPointToRay(currentMousePosition);
                plane.Raycast(ray, out distance);
                Vector3 currentHitPoint = ray.GetPoint(distance);
                Vector3 panDelta = hitPoint - currentHitPoint;

                pivotPoint = initialPivotPoint + panDelta;
            }
        }
    }

    private void HandleDefaultMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UpdateLastMousePosition();
        }

        if (Input.GetMouseButton(1))
        {
            CamOrbit();
        }

        var mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll != 0)
        {
            Zoom(mouseScroll);
        }

        CheckForKeyPresses();

    }

    private void UpdateLastMousePosition()
    {
        lastMousePosition = Input.mousePosition;
    }

    private void CamOrbit(float customDeltaX = 0, float customDeltaY = 0)
    {
        Vector3 delta = Input.mousePosition - lastMousePosition;
        lastMousePosition = Input.mousePosition;

        float horizontalInput = delta.x * rotationSpeed * 0.1f;
        float verticalInput = delta.y * rotationSpeed * 0.1f;

        if (customDeltaX != 0 || customDeltaY != 0)
        {
            horizontalInput = customDeltaX;
            verticalInput = customDeltaY;
        }

        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalInput, Vector3.up);
        Quaternion verticalRotation = Quaternion.AngleAxis(-verticalInput, camHolder.transform.right);

        camToPivotDirection = verticalRotation * horizontalRotation * camToPivotDirection;

        camToPivotDirection = camToPivotDirection.normalized;

        camHolder.transform.rotation = horizontalRotation * verticalRotation * camHolder.transform.rotation;
    }

    private void rotateCam45DegRight()
    {
        CamOrbit(-45f, 0f);
    }

    private void rotateCam45DegLeft()
    {
        CamOrbit(45f, 0f);
    }

    private void rotateCam45DegUp()
    {
        CamOrbit(0f, -45f);
    }

    private void rotateCam45DegDown()
    {
        CamOrbit(0f, 45f);
    }

    private void Zoom(float mouseWheelStep)
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            zoomStrength = 3f;
        }
        else
        {
            zoomStrength = 1f;
        }
        camToPivotDistance += -mouseWheelStep * CalculateDynamicZoomStep(camToPivotDistance) * 2 * zoomStrength;
    }

    void ResetCameraView()
    {
        Debug.Log(camHolder.transform.rotation);
        followModeActive = false;
        camToPivotDirection = defaultCamToPivotDirection;
        camToPivotDistance = defaultCamToPivotDistance;
        camHolder.transform.rotation = defaultRotation;
        pivotPoint = sun.transform.position;
        followTarget = null;
    }

    public void StopFollowing()
    {
        followModeActive = false;

    }

    public Transform GetFollowingTarget()
    {
        return followTarget;
    }

    public void FollowObject(Transform target)
    {
        followModeActive = true;
        followTarget = target;
        camToPivotDistance = target.transform.lossyScale.x * 3;

    }

    void SetFixedView(Vector3 direction)
    {
        Debug.Log("Before direction: " + camToPivotDirection);
        camToPivotDirection = direction; // Ensure direction is normalized
        camHolder.transform.rotation = Quaternion.LookRotation(-direction);
        Debug.Log("Quaternion: " + camHolder.transform.rotation);
    }

    private void UpdateCameraPosition()
    {
        var minZoomDistance = RayCastPlanetRadius();
        camToPivotDistance = Mathf.Clamp(camToPivotDistance, minZoomDistance, maxZoomDistance);
        var camToPivotVector = camToPivotDirection * camToPivotDistance;
        camHolder.transform.position = pivotPoint + camToPivotVector;
    }

    private float RayCastPlanetRadius()
    {
        Ray ray = new Ray(camHolder.transform.position, cam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            float planetRadius = hit.transform.lossyScale.x / 2f;
            return planetRadius * minDistanceToSurfaceRadiusFactor;
        }
        else
        {
            return minZoomDistance;
        }
    }

    private float CalculateDynamicZoomStep(float distanceCamPivot)
    {
        var step = Mathf.Pow(0.0003f * (distanceCamPivot + 10900), 3) - 34;
        return Mathf.Clamp(step, 1.0f, 300.0f);
    }

    public float GetZoomScale()
    {
        return Mathf.InverseLerp(minZoomDistance, maxZoomDistance, camToPivotDistance);
    }
}