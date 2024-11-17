using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalSpeed = 10f; // Velocidad de movimiento lateral
    public float verticalSpeed = 15f;   // Velocidad de movimiento arriba/abajo
    public float tiltAmount = 15f;      // Ángulo máximo de inclinación
    public float tiltSpeed = 5f;        // Velocidad de inclinación

    void Update()
    {
        // Obtén la entrada del usuario
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calcula el movimiento separado por velocidad horizontal y vertical
        Vector3 horizontalMovement = new Vector3(horizontal, 0, 0) * horizontalSpeed * Time.deltaTime;
        Vector3 verticalMovement = new Vector3(0, 0, vertical) * verticalSpeed * Time.deltaTime;

        // Suma ambos movimientos
        Vector3 movement = horizontalMovement + verticalMovement;

        // Aplica el movimiento
        transform.Translate(movement, Space.World);

        // Inclinar el barco en el eje Z según el movimiento horizontal
        float targetTilt = -horizontal * tiltAmount; // Inclinación proporcional al movimiento
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt);

        // Interpolación suave hacia la inclinación deseada
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }
}
