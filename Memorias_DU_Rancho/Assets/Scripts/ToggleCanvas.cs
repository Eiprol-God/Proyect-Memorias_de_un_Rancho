using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    [Header("Lista de Canvases a controlar")]
    public GameObject[] canvases;

    // -----------------------------------------------------
    // Muestra un canvas específico según su índice en la lista
    // -----------------------------------------------------
    public void ShowCanvas(int index)
    {
        if (canvases != null && index >= 0 && index < canvases.Length && canvases[index] != null)
        {
            canvases[index].SetActive(true);
        }
    }

    // ------------------------------------------------------
    // Oculta un canvas específico según su índice en la lista
    // ------------------------------------------------------
    public void HideCanvas(int index)
    {
        if (canvases != null && index >= 0 && index < canvases.Length && canvases[index] != null)
        {
            canvases[index].SetActive(false);
        }
    }

    // -----------------------------------------------------------
    // Alterna un canvas específico según su índice en la lista
    // -----------------------------------------------------------
    public void Toggle(int index)
    {
        if (canvases != null && index >= 0 && index < canvases.Length && canvases[index] != null)
        {
            canvases[index].SetActive(!canvases[index].activeSelf);
        }
    }

    // ----------------------------------------------------------------
    // Muestra un único canvas y oculta todos los demás de la lista
    // ----------------------------------------------------------------
    public void ShowOnly(int indexToShow)
    {
        if (canvases == null) return;

        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i] != null)
            {
                canvases[i].SetActive(i == indexToShow);
            }
        }
    }

    // -----------------------------------------
    // Oculta todos los canvases de la lista
    // -----------------------------------------
    public void HideAll()
    {
        if (canvases == null) return;

        foreach (GameObject canvas in canvases)
        {
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }
}