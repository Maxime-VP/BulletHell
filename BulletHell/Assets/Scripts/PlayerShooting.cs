using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Asigna el prefab del proyectil
    public Transform firePoint;     // Punto desde donde disparar
    public float bulletSpeed = 20f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Cambia por tu tecla de disparo preferida
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Crear un proyectil en la posición del FirePoint con una rotación personalizada
        Quaternion customRotation = firePoint.rotation * Quaternion.Euler(90, 0, 0); // Ajustar con valores según lo necesario
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, customRotation);

        // Agregar velocidad al proyectil
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }
    }

}
