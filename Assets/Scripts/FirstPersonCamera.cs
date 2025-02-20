using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    private bool isRightClickHeld = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Free cursor at start
    }

    void Update()
    {
        // Right-click to enable camera rotation
        if (Input.GetMouseButtonDown(1))
        {
            isRightClickHeld = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightClickHeld = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (isRightClickHeld)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent flipping over

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical Look
            playerBody.Rotate(Vector3.up * mouseX); // Horizontal Turn
        }
    }
}
