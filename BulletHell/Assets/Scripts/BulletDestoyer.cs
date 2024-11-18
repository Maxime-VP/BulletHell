using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    public float boundary = 20f; // Límite para destruir la bala
    public int damage = 10; // Daño que causa la bala
    public GameObject destructionEffect; // Efecto visual al destruir la bala

    void Update()
    {
        // Verifica si la bala está fuera de los límites
        if (Mathf.Abs(transform.position.x) > boundary || Mathf.Abs(transform.position.z) > boundary)
        {
            DestroyBullet(); // Llamar al método para destruir la bala
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si la bala golpea al jefe
        BossController boss = collision.gameObject.GetComponent<BossController>();

        if (boss != null)
        {
            // Aplicar daño al jefe
            boss.TakeDamage(damage);
        }

        // Destruir la bala con efecto visual
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        // Instanciar el efecto visual al destruir la bala
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        // Destruir el objeto de la bala
        Destroy(gameObject);
    }
}
