using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target Setup")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    [Header("Rotation Settings")]
    [Tooltip("Degrees to turn each time Q or E is pressed.")]
    public float stepAngle = 45f;

    public float rotationSmoothSpeed = 10f;

    private float targetYAngle;
    private float currentYAngle;

    private void Start()
    {
        targetYAngle = transform.eulerAngles.y;
        currentYAngle = targetYAngle;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetYAngle -= stepAngle;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            targetYAngle += stepAngle;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        currentYAngle = Mathf.LerpAngle(currentYAngle, targetYAngle, Time.deltaTime * rotationSmoothSpeed);
        Quaternion currentRotation = Quaternion.Euler(0f, currentYAngle, 0f);

        Vector3 desiredPosition = target.position + currentRotation * offset;
        transform.position = desiredPosition;

        transform.LookAt(target.position + Vector3.up * 0.5f);
    }
}