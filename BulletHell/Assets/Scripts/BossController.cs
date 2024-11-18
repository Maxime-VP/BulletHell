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

        while (true)
        {
            foreach (var firePoint in activeFirePoints)
            {
                if (phase == 3) // Disparo con ángulo aleatorio para la fase 3
                {
                    SpawnBulletWithAngle(firePoint.position, Vector3.back);
                }
                else // Disparo recto para las otras fases
                {
                    SpawnBullet(firePoint.position, Vector3.back);
                }
            }

            yield return new WaitForSeconds(fireRate); // Usar el fireRate correspondiente
        }
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
            rb.linearVelocity = direction * bulletSpeed; // Usar linearVelocity para disparar en el eje Z negativo
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
        float randomAngle = Random.Range(-60f, 60f); // Generar ángulo aleatorio entre -60 y 60 grados
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0); // Rotar sobre el eje Y
        Vector3 rotatedDirection = rotation * direction; // Aplicar la rotación a la dirección original

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
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
