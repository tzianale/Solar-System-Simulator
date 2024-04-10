using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject parentModel;
    private Vector3 pivotPoint;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private float rotationSpeed = 1200.0f;
    private static float panSpeed = 0.2f;
    private static float zoomSpeed = 30.0f;
    public static float maxZoomOut = 100000f;
    public static float maxZoomIn = 800.0f;

    private Camera mainCamera;

    private float verticalAngle = 0f;
    private float distanceToPivot;

    private bool isFollowingObject = false;
    
    private Transform objectPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        pivotPoint = parentModel.transform.position;

        defaultPosition = pivotPoint + new Vector3(0, 1, -1) * distanceToPivot;
        transform.position = defaultPosition;
        transform.LookAt(pivotPoint);
        defaultRotation = transform.rotation;

        Zoom(0.2f);

        mainCamera.fieldOfView = 60f;
    }


    private void Update()
    {
        if (isFollowingObject) pivotPoint = objectPosition.position;
        
        if (Input.GetMouseButton(1)) // Right-click to rotate
        {
            CamOrbit();
        }

        if (Input.GetMouseButton(0)) // Left-click to pan
        {
            Pan();
        }

        Zoom(-Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            mainCamera.orthographic = !mainCamera.orthographic;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCameraView();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetFixedView(Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetFixedView(new Vector3(-1, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            SetFixedView(Vector3.right);
        }
    }

    void CamOrbit()
    {
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Max(Time.deltaTime, 0.01f);
        float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Max(Time.deltaTime, 0.01f);

        verticalAngle = Mathf.Clamp(verticalAngle - verticalInput, -90f, 90f);

        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalInput, Vector3.up);
        Quaternion verticalRotation = Quaternion.AngleAxis(-verticalInput, transform.right);

        Vector3 direction = (transform.position - pivotPoint).normalized;
        Quaternion targetRotation = horizontalRotation * verticalRotation * transform.rotation;
        transform.rotation = targetRotation;
        transform.position = pivotPoint + direction * distanceToPivot;
    }


    void Pan()
    {
        float dynamicPanSpeed = panSpeed * (-distanceToPivot / (maxZoomOut * 0.001f));

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            dynamicPanSpeed *= 20f;
        }

        dynamicPanSpeed = Mathf.Max(dynamicPanSpeed, 0.05f);

        Vector3 move = -mainCamera.transform.right * Input.GetAxis("Mouse X") * dynamicPanSpeed
                       - mainCamera.transform.up * Input.GetAxis("Mouse Y") * dynamicPanSpeed;
        pivotPoint += move;
        transform.position += move;
    }




    void Zoom(float zoomDiff)
    {
        float dynamicZoomSpeed = zoomSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            dynamicZoomSpeed *= 8.0f;
        }

        float newDistance = distanceToPivot + zoomDiff * dynamicZoomSpeed;

        Debug.Log(newDistance);

        newDistance = Mathf.Clamp(newDistance, -maxZoomOut, maxZoomIn);

        distanceToPivot = newDistance;
        transform.position = pivotPoint - (transform.up * distanceToPivot);
    }


    void SetFixedView(Vector3 direction)
    {
        transform.position = pivotPoint + direction * distanceToPivot;
        transform.LookAt(pivotPoint);
    }


    public void SetFocusedObject(Transform objectToFollow)
    {
        isFollowingObject = true;
        objectPosition = objectToFollow;
    }


    public void ClearFocusedObject()
    {
        isFollowingObject = false;
    }


    void ResetCameraView()
    {
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
        pivotPoint = parentModel.transform.position;
        distanceToPivot = Vector3.Distance(transform.position, pivotPoint);
        Zoom(0.2f);
    }
}
