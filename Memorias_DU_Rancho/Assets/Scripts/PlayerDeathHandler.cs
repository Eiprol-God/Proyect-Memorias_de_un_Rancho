using UnityEngine;

/// <summary>
/// Este script se encarga de mostrar un canvas específico (generalmente el de muerte) 
/// cuando el jugador muere. Debe ser colocado en el GameObject del jugador.
/// </summary>
public class PlayerDeathHandler : MonoBehaviour
{
    [Header("Configuración del Canvas de Muerte")]
    [Tooltip("Arrastra aquí el GameObject que tiene el script ToggleCanvas.")]
    public ToggleCanvas canvasManager;

    [Tooltip("El índice del Canvas de Muerte en la lista del script ToggleCanvas.")]
    public int deathCanvasIndex = 0; // Se puede ajustar en el Inspector.

    /// <summary>
    /// Este es el método que debes llamar desde tu script de jugador cuando muere.
    /// Se encargará de mostrar el canvas de muerte y ocultar los demás.
    /// </summary>
    public void HandlePlayerDeath()
    {
        if (canvasManager != null)
        {
            // Usamos ShowOnly para mostrar el canvas de muerte y ocultar los demás.
            canvasManager.ShowOnly(deathCanvasIndex);
            Debug.Log("Manejador de muerte: Mostrando canvas en el índice " + deathCanvasIndex);
        }
        else
        {
            Debug.LogError("¡No se ha asignado el 'canvasManager' (ToggleCanvas) en el PlayerDeathHandler!");
        }
    }
}
