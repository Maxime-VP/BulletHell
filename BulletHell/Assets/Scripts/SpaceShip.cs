using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    private bool isHit = false; // Verifica si la nave fue golpeada

    void OnCollisionEnter(Collision collision)
    {
        if (!isHit) // Solo iniciar el temporizador si aún no fue golpeada
        {
            isHit = true;
            StartCoroutine(DestroyAfterDelay(3f)); // Llama a la corrutina con un retraso de 3 segundos
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo definido
        Destroy(gameObject); // Destruye la nave
    }
}

