using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; // Velocidad de movimiento
    public float tiltAmount = 15f; // Ángulo máximo de inclinación
    public float tiltSpeed = 5f; // Velocidad de inclinación

    void Update()
    {
        // Obtén la entrada del usuario
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calcula el movimiento
        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        // Aplica el movimiento
        transform.Translate(movement, Space.World);

        // Inclinar el barco en el eje Z según el movimiento horizontal
        float targetTilt = -horizontal * tiltAmount; // Inclinación proporcional al movimiento
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt);

        // Interpolación suave hacia la inclinación deseada
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }
}
