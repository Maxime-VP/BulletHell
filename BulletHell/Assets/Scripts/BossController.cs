using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab de las balas
    public Transform[] firePoints; // Puntos de disparo
    public Text bulletCounterText; // Contador de balas activas
    public float modeDuration = 10f; // Duración de cada modo
    public float bulletSpeed = 10f; // Velocidad de las balas

    private int activeMode = 0; // Modo actual (0, 1 o 2)
    private int activeBullets = 0; // Contador de balas activas
    private Coroutine currentShootingRoutine;

    void Start()
    {
        // Iniciar el cambio de modos y el patrón de disparo
        StartCoroutine(ChangeModes());
    }

    void Update()
    {
        // Actualizar el texto del contador de balas
        bulletCounterText.text = $"Bullets: {activeBullets}";
    }

    private IEnumerator ChangeModes()
    {
        while (true)
        {
            // Cambiar al siguiente modo
            activeMode = (activeMode + 1) % 3;

            // Detener cualquier patrón previo de disparo
            if (currentShootingRoutine != null)
            {
                StopCoroutine(currentShootingRoutine);
            }

            // Iniciar el patrón de disparo correspondiente al modo
            currentShootingRoutine = StartCoroutine(ShootingPattern(activeMode));

            // Esperar la duración del modo
            yield return new WaitForSeconds(modeDuration);
        }
    }

    private IEnumerator ShootingPattern(int mode)
    {
        while (true)
        {
            switch (mode)
            {
                case 0:
                    yield return StartCoroutine(SpiralPattern());
                    break;
                case 1:
                    yield return StartCoroutine(WavePattern());
                    break;
                case 2:
                    yield return StartCoroutine(RandomPattern());
                    break;
            }
        }
    }

    private IEnumerator SpiralPattern()
    {
        float angle = 0f;
        while (true)
        {
            for (int i = 0; i < firePoints.Length; i++)
            {
                // Disparos en espiral hacia abajo
                Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.down;
                SpawnBullet(firePoints[i].position, direction);
            }

            angle += 45f; // Incrementar el ángulo para el efecto espiral
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator WavePattern()
    {
        while (true)
        {
            for (int i = 0; i < firePoints.Length; i++)
            {
                // Onda en movimiento vertical
                Vector3 direction = Vector3.down + new Vector3(Mathf.Sin(Time.time * 2), 0, 0);
                SpawnBullet(firePoints[i].position, direction.normalized);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator RandomPattern()
    {
        while (true)
        {
            for (int i = 0; i < firePoints.Length; i++)
            {
                // Dirección aleatoria hacia abajo
                Vector3 randomDirection = new Vector3(Random.Range(-0.5f, 0.5f), -1f, Random.Range(-0.5f, 0.5f)).normalized;
                SpawnBullet(firePoints[i].position, randomDirection);
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SpawnBullet(Vector3 position, Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed; // Configurar la velocidad de la bala
        }

        // Evitar que las balas colisionen con la nave
        Collider bulletCollider = bullet.GetComponent<Collider>();
        Collider bossCollider = GetComponent<Collider>();

        if (bulletCollider != null && bossCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, bossCollider);
        }

        // Incrementar el contador de balas activas
        activeBullets++;

        // Destruir la bala después de 5 segundos y decrementar el contador
        Destroy(bullet, 5f);
        StartCoroutine(DecrementBulletCounterAfterDelay(5f));
    }

    private IEnumerator DecrementBulletCounterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        activeBullets--;
    }
}
