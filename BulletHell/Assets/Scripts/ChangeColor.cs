using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color newColor = Color.red; // Puedes cambiar el color aquí desde el Inspector

    void Start()
    {
        // Obtén el componente Renderer de la cápsula
        Renderer capsuleRenderer = GetComponent<Renderer>();

        // Asegúrate de que el objeto tiene un material asignado
        if (capsuleRenderer != null && capsuleRenderer.material != null)
        {
            // Cambia el color del material
            capsuleRenderer.material.color = newColor;
        }
        else
        {
            Debug.LogWarning("No se encontró un Renderer o Material en el objeto.");
        }
    }
}
