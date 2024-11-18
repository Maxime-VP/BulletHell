using UnityEngine;
using UnityEngine.UI; // Necesario para manejar UI

public class SpaceShip : MonoBehaviour
{
    public GameObject explosionEffect; // Efecto visual de explosión
    public Camera cameraController; // Referencia al controlador de la cámara
    public static int remainingEnemies = 5; // Número inicial de enemigos en la escena
    public Text enemiesCounterText; // Referencia al texto de UI que mostrará los enemigos restantes

    void Start()
    {
        // Inicializamos el texto con el número de enemigos restantes
        UpdateEnemiesCounter();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Instanciar efecto de explosión
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Reducir el número de enemigos restantes
        remainingEnemies--;

        // Actualizar el contador de enemigos en la UI
        UpdateEnemiesCounter();

        // Verificar si todos los enemigos han sido destruidos
        if (remainingEnemies <= 0)
        {
            // Activar el movimiento de la cámara
            if (cameraController != null)
            {
                Debug.Log("Todos los enemigos han sido destruidos. Iniciando movimiento de la cámara.");
                cameraController.TriggerCameraMove();
            }
            else
            {
                Debug.LogError("CameraController no está asignado en el Inspector.");
            }
        }

        // Destruir la nave
        Destroy(gameObject);
    }

    void UpdateEnemiesCounter()
    {
        if (enemiesCounterText != null)
        {
            enemiesCounterText.text = "Enemigos: " + remainingEnemies;
        }
        else
        {
            Debug.LogError("EnemiesCounterText no está asignado en el Inspector.");
        }
    }
}
