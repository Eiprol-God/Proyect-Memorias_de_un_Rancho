using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    // Nombre de la escena a cargar
    public string sceneToLoad = "Test_Level";

    // Este método se llamará al presionar el botón
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
