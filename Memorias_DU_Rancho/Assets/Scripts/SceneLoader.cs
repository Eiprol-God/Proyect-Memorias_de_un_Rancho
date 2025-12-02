using UnityEngine;
using UnityEngine.SceneManagement;

// Este script contiene funciones útiles para manejar escenas.
public class SceneLoader : MonoBehaviour
{
    // Esta función pública puede ser llamada por un Animation Event.
    public void LoadSceneByName(string sceneName)
    {
        // Asegúrate de que la escena ha sido añadida en File > Build Settings
        SceneManager.LoadScene(sceneName);
    }
}
