using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public GameObject explosionEffect; // Efecto visual de explosión
    public Camera cameraController; // Referencia al controlador de la cámara

    void OnCollisionEnter(Collision collision)
    {
        // Instanciar efecto de explosión
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Activar el movimiento de la cámara
        if (cameraController != null)
        {
            Debug.Log("Iniciando movimiento de la cámara desde el script de la nave.");
            cameraController.TriggerCameraMove();
        }
        else
        {
            Debug.LogError("CameraController no está asignado en el Inspector.");
        }

        // Destruir la nave instantáneamente
        Destroy(gameObject);
    }
}
