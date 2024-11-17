using UnityEngine;

public class BulletDestoyer : MonoBehaviour

{
    public float boundary = 20f; // Límite para destruir la bala

    void Update()
    {
        // Verifica si la bala está fuera de los límites
        if (Mathf.Abs(transform.position.x) > boundary || Mathf.Abs(transform.position.z) > boundary)
        {
            Destroy(gameObject); // Destruye la bala
        }
    }
}

