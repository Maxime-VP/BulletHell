using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(0, 5, -24); // Posición final de la cámara
    public float moveDuration = 2f; // Duración del movimiento de la cámara
    private bool moveCamera = false; // Controla si la cámara debe moverse

    void Update()
    {
        // Iniciar el movimiento si está activado
        if (moveCamera)
        {
            StartCoroutine(MoveToPosition(targetPosition, moveDuration));
            moveCamera = false; // Desactivar después de iniciar el movimiento
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target; // Asegura que la cámara esté exactamente en la posición final
    }

    // Método público para activar el movimiento desde otro script
    public void TriggerCameraMove()
    {
        moveCamera = true;
    }
}

