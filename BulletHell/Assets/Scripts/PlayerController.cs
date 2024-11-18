using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float normalHorizontalSpeed = 10f;
    public float normalVerticalSpeed = 15f;
    public float slowHorizontalSpeed = 5f;
    public float slowVerticalSpeed = 7.5f;
    public KeyCode slowKey = KeyCode.LeftShift;

    public float tiltAmount = 15f;
    public float tiltSpeed = 5f;

    public int maxHealth = 100; // Salud máxima del jugador
    private int currentHealth;  // Salud actual del jugador

    public Text healthText; // Referencia al texto de la salud
    public Text endGameText; // Referencia al texto de "Victory" o "Game Over"

    void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud actual
        UpdateHealthText(); // Actualizar el texto de la salud al inicio
        endGameText.gameObject.SetActive(false); // Ocultar el texto de fin del juego
    }

    void Update()
    {
        // Movimiento del jugador
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentHorizontalSpeed = Input.GetKey(slowKey) ? slowHorizontalSpeed : normalHorizontalSpeed;
        float currentVerticalSpeed = Input.GetKey(slowKey) ? slowVerticalSpeed : normalVerticalSpeed;

        Vector3 movement = new Vector3(horizontal * currentHorizontalSpeed, 0, vertical * currentVerticalSpeed) * Time.deltaTime;
        transform.Translate(movement, Space.World);

        float targetTilt = -horizontal * tiltAmount;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reducir salud
        UpdateHealthText(); // Actualizar la visualización de la salud
        Debug.Log($"Jugador recibió {damage} de daño. Salud restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // Manejar la muerte del jugador
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}";
        }
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        ShowEndGameText("Game Over");
        Destroy(gameObject); // Destruir el objeto del jugador
    }

    public void ShowEndGameText(string message)
    {
        if (endGameText != null)
        {
            endGameText.text = message;
            endGameText.gameObject.SetActive(true);
        }
    }
}
