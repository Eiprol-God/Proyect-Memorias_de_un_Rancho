using UnityEngine;

/// <summary>
/// Este script causa muerte instantánea al jugador si entra en su trigger.
/// Debe ser colocado en un objeto de "peligro" (ej. pinchos, lava, precipicio).
/// El objeto DEBE tener un Collider (ej. BoxCollider) con la opción "Is Trigger" activada.
/// </summary>
[RequireComponent(typeof(Collider))] // Fuerza a que el objeto tenga un Collider.
public class InstantDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Primero, comprobamos si el objeto que entró en el trigger tiene la etiqueta "Player".
        // ¡Es muy importante que tu jugador tenga esta etiqueta!
        if (other.CompareTag("Player"))
        {
            // Si es el jugador, buscamos su componente PlayerDeathHandler.
            PlayerDeathHandler deathHandler = other.GetComponent<PlayerDeathHandler>();

            if (deathHandler != null)
            {
                // Si encontramos el script, llamamos a su método para manejar la muerte.
                deathHandler.HandlePlayerDeath();
                Debug.Log("El jugador (" + other.name + ") ha entrado en una zona de muerte instantánea.");
            }
            else
            {
                // Este error te avisará si a tu jugador le falta el script necesario.
                Debug.LogError("El objeto con tag 'Player' no tiene el componente PlayerDeathHandler. No se puede procesar la muerte.");
            }
        }
    }

    // --- Ayuda Visual en el Editor de Unity ---
    private void OnDrawGizmos()
    {
        // Dibuja un cubo rojo semitransparente en la posición y tamaño del BoxCollider del objeto.
        // Esto te ayuda a visualizar la zona de muerte directamente en la vista de Escena.
        // No se verá en el juego final.
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.4f); // Rojo semitransparente
            Gizmos.matrix = transform.localToWorldMatrix; // Asegura que la rotación y escala del objeto se apliquen al gizmo
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        }
    }
}
