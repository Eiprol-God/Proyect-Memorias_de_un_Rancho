using UnityEngine;

public class CanvasActivator : MonoBehaviour
{
    public GameObject canvasToActivate; // Arrastra tu Canvas aquí desde el Inspector

    [SerializeField]
    private string playerTag = "Player"; // Asegúrate de que tu jugador tenga este Tag

    private bool hasBeenTriggered = false; // Para que solo se active una vez, si es necesario

    void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el objeto que entró en el trigger es el jugador
        if (other.CompareTag(playerTag) && !hasBeenTriggered)
        {
            if (canvasToActivate != null)
            {
                canvasToActivate.SetActive(true); // Activa el Canvas
                hasBeenTriggered = true; // Marca como activado
                Debug.Log("Canvas activado por el jugador.");
                
                // Opcional: Desactivar este script o el GameObject después de activarse
                // gameObject.SetActive(false); 
                // this.enabled = false;
            }
            else
            {
                Debug.LogWarning("CanvasToActivate no está asignado en el Inspector para " + gameObject.name);
            }
        }
    }

    // Opcional: Para resetear el trigger si el jugador sale del área
    /*
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && hasBeenTriggered)
        {
            if (canvasToActivate != null)
            {
                canvasToActivate.SetActive(false); // Desactiva el Canvas cuando el jugador sale
                hasBeenTriggered = false; // Resetea para poder volver a activarlo
                Debug.Log("Canvas desactivado por el jugador.");
            }
        }
    }
    */
}
