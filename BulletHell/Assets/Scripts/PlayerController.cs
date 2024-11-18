using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float normalHorizontalSpeed = 10f; // Velocidad horizontal normal
    public float normalVerticalSpeed = 15f;   // Velocidad vertical normal
    public float slowHorizontalSpeed = 5f;   // Velocidad horizontal lenta
    public float slowVerticalSpeed = 7.5f;   // Velocidad vertical lenta
    public KeyCode slowKey = KeyCode.LeftShift; // Tecla para activar el movimiento lento

    public float tiltAmount = 15f;      // Ángulo máximo de inclinación
    public float tiltSpeed = 5f;        // Velocidad de inclinación

    private float currentHorizontalSpeed; // Velocidad horizontal actual
    private float currentVerticalSpeed;   // Velocidad vertical actual

    void Start()
    {
        // Configurar las velocidades iniciales como las normales
        currentHorizontalSpeed = normalHorizontalSpeed;
        currentVerticalSpeed = normalVerticalSpeed;
    }

    void Update()
    {
        // Comprobar si la tecla de movimiento lento está presionada
        if (Input.GetKey(slowKey))
        {
            currentHorizontalSpeed = slowHorizontalSpeed;
            currentVerticalSpeed = slowVerticalSpeed;
        }
        else
        {
            currentHorizontalSpeed = normalHorizontalSpeed;
            currentVerticalSpeed = normalVerticalSpeed;
        }

        // Obtén la entrada del usuario
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calcula el movimiento separado por velocidad horizontal y vertical
        Vector3 horizontalMovement = new Vector3(horizontal, 0, 0) * currentHorizontalSpeed * Time.deltaTime;
        Vector3 verticalMovement = new Vector3(0, 0, vertical) * currentVerticalSpeed * Time.deltaTime;

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
