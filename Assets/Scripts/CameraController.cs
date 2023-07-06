using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // Asegúrate de asignar esto al jugador en el inspector
    public Vector3 offset;    // Ajusta esto a la posición relativa que quieres para la cámara
    public float cameraSpeed = 1.0f;
    public float targetSpeed;

    private void Update()
    {
        float smoothSpeed = cameraSpeed * Time.deltaTime;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }

    public void SetCameraSpeed(float newSpeed)
    {
        targetSpeed = newSpeed;
        cameraSpeed = Mathf.Lerp(cameraSpeed, targetSpeed, Time.deltaTime);
    }
}
