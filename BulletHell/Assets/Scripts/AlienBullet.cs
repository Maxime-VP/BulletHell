using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    public int damage = 10; // Daño que causa la bala al jugador
    public GameObject destructionEffect; // Efecto visual al destruir la bala

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si la bala golpea al jugador
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            // Aplicar daño al jugador
            player.TakeDamage(damage);
        }

        // Destruir la bala después de la colisión
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        // Instanciar efecto visual al destruir la bala
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        // Destruir el objeto de la bala
        Destroy(gameObject);
    }
}
