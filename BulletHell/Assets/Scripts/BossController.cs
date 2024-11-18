using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    // Configuración de disparo
    public GameObject bulletPrefab;
    public Transform[] firePointsPhase1;
    public Transform[] firePointsPhase2;
    public Transform[] firePointsPhase3;
    public float fireRatePhase1 = 1f;
    public float fireRatePhase2 = 0.5f;
    public float fireRatePhase3 = 0.3f;
    public float modeDuration = 10f;
    public float bulletSpeed = 10f;

    // Configuración de movimiento
    public float movementSpeed = 8f; // Velocidad de movimiento durante la fase 2
    public float movementDurationLeftToRight = 2.5f;
    public float movementDurationRightToLeft = 5f;

    // Configuración de vida
    public int maxHealth = 100;
    public Text healthCounterText;
    public GameObject explosionEffect;

    // Contador de balas
    public BulletCounter bulletCounter;

    private int currentHealth;
    private int currentPhaseIndex = 0; // Índice actual para el orden personalizado
    private int[] phaseOrder = { 1, 3, 2 }; // Orden personalizado de fases
    private Coroutine currentShootingRoutine;
    private bool canTakeDamage = false;

    // Variables para el disparo incremental
    private float currentAngle = -60f; // Ángulo inicial para fase 1
    private const float angleIncrement = 10f; // Incremento del ángulo

    void Start()
    {
        enabled = false; // Desactivar hasta que se active
    }

    public void ActivateBoss()
    {
        enabled = true;
        currentHealth = maxHealth;
        UpdateHealthUI();
        canTakeDamage = true;
        StartCoroutine(ChangeModes());
    }

    void Update()
    {
        UpdateHealthUI();
    }

    private IEnumerator ChangeModes()
    {
        while (true)
        {
            if (currentShootingRoutine != null)
            {
                StopCoroutine(currentShootingRoutine);
            }

            // Cambiar de fase según el orden personalizado
            currentPhaseIndex = (currentPhaseIndex + 1) % phaseOrder.Length;
            int currentPhase = phaseOrder[currentPhaseIndex];
            currentShootingRoutine = StartCoroutine(ShootingPattern(currentPhase));

            yield return new WaitForSeconds(modeDuration);
        }
    }

    private IEnumerator ShootingPattern(int phase)
    {
        Transform[] activeFirePoints = GetFirePointsForPhase(phase);
        float fireRate = GetFireRateForPhase(phase);

        if (phase == 2)
        {
            // Iniciar el patrón de movimiento mientras dispara
            StartCoroutine(MoveBossPattern());
        }

        while (true)
        {
            foreach (var firePoint in activeFirePoints)
            {
                if (phase == 1) // Disparo incremental para la fase 1
                {
                    SpawnBulletWithIncrementalAngle(firePoint.position, Vector3.back);
                }
                else if (phase == 3) // Disparo con ángulo aleatorio para la fase 3
                {
                    SpawnBulletWithAngle(firePoint.position, Vector3.back);
                }
                else // Disparo recto para la fase 2
                {
                    SpawnBullet(firePoint.position, Vector3.back);
                }
            }

            yield return new WaitForSeconds(fireRate); // Usar el fireRate correspondiente
        }
    }

    private IEnumerator MoveBossPattern()
    {
        // Calcular límites basados en la posición inicial
        Vector3 initialPosition = transform.position;
        Vector3 leftLimit = initialPosition + Vector3.left * 10f;
        Vector3 rightLimit = initialPosition + Vector3.right * 10f;

        // Movimiento de izquierda a derecha (2.5 segundos)
        yield return MoveBetweenPoints(initialPosition, rightLimit, movementDurationLeftToRight);

        // Movimiento de derecha a izquierda (5 segundos)
        yield return MoveBetweenPoints(rightLimit, leftLimit, movementDurationRightToLeft);

        // Movimiento de vuelta a la posición inicial (2.5 segundos)
        yield return MoveBetweenPoints(leftLimit, initialPosition, movementDurationLeftToRight);

    }

    private IEnumerator MoveBetweenPoints(Vector3 startPoint, Vector3 endPoint, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPoint; // Asegurarse de terminar exactamente en el punto final
    }

    private Transform[] GetFirePointsForPhase(int phase)
    {
        switch (phase)
        {
            case 1:
                return firePointsPhase1;
            case 2:
                return firePointsPhase2;
            case 3:
                return firePointsPhase3;
            default:
                return firePointsPhase1;
        }
    }

    private float GetFireRateForPhase(int phase)
    {
        switch (phase)
        {
            case 1:
                return fireRatePhase1;
            case 2:
                return fireRatePhase2;
            case 3:
                return fireRatePhase3;
            default:
                return fireRatePhase1;
        }
    }

    private void SpawnBullet(Vector3 position, Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        Collider bulletCollider = bullet.GetComponent<Collider>();
        Collider bossCollider = GetComponent<Collider>();

        if (bulletCollider != null && bossCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, bossCollider);
        }

        bulletCounter?.IncrementBullet();
        Destroy(bullet, 5f);
        StartCoroutine(DecrementBulletCounterAfterDelay(5f));
    }

    private void SpawnBulletWithAngle(Vector3 position, Vector3 direction)
    {
        float randomAngle = Random.Range(-60f, 60f);
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
        Vector3 rotatedDirection = rotation * direction;

        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = rotatedDirection * bulletSpeed;
        }

        Collider bulletCollider = bullet.GetComponent<Collider>();
        Collider bossCollider = GetComponent<Collider>();

        if (bulletCollider != null && bossCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, bossCollider);
        }

        bulletCounter?.IncrementBullet();
        Destroy(bullet, 5f);
        StartCoroutine(DecrementBulletCounterAfterDelay(5f));
    }

    private void SpawnBulletWithIncrementalAngle(Vector3 position, Vector3 direction)
    {
        Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
        Vector3 rotatedDirection = rotation * direction;

        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = rotatedDirection * bulletSpeed;
        }

        Collider bulletCollider = bullet.GetComponent<Collider>();
        Collider bossCollider = GetComponent<Collider>();

        if (bulletCollider != null && bossCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, bossCollider);
        }

        currentAngle += angleIncrement;
        if (currentAngle > 60f)
        {
            currentAngle = -60f;
        }

        bulletCounter?.IncrementBullet();
        Destroy(bullet, 5f);
        StartCoroutine(DecrementBulletCounterAfterDelay(5f));
    }

    private IEnumerator DecrementBulletCounterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        bulletCounter?.DecrementBullet();
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthCounterText != null)
        {
            healthCounterText.text = $"Health: {currentHealth}";
        }
    }

    private void Die()
    {
        Debug.Log("El jefe ha sido derrotado.");
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.ShowEndGameText("Victory");
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
