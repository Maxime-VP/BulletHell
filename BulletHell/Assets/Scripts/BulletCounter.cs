using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour
{
    public Text bulletCounterText; // Texto en pantalla para el contador de balas
    private int activeBullets = 0; // Número de balas activas

    // Incrementar el contador
    public void IncrementBullet()
    {
        activeBullets++;
        UpdateCounter();
    }

    // Decrementar el contador
    public void DecrementBullet()
    {
        activeBullets = Mathf.Max(0, activeBullets - 1); // Asegurar que no sea negativo
        UpdateCounter();
    }

    // Actualizar el texto en pantalla
    private void UpdateCounter()
    {
        if (bulletCounterText != null)
        {
            bulletCounterText.text = $"Bullets: {activeBullets}";
        }
    }
}
