using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    [Header("Canvas a mostrar/ocultar")]
    public GameObject canvasToToggle;

    // -----------------------------
    // Mostrar el canvas
    // -----------------------------
    public void ShowCanvas()
    {
        if (canvasToToggle != null)
            canvasToToggle.SetActive(true);
    }

    // -----------------------------
    // Ocultar el canvas
    // -----------------------------
    public void HideCanvas()
    {
        if (canvasToToggle != null)
            canvasToToggle.SetActive(false);
    }

    // -----------------------------
    // Alternar canvas (opcional)
    // -----------------------------
    public void Toggle()
    {
        if (canvasToToggle != null)
            canvasToToggle.SetActive(!canvasToToggle.activeSelf);
    }
}