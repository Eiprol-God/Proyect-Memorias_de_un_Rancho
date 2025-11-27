using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("UI")]
    public Sprite LiveFull;
    public Sprite LiveEmpty;
    public Image[] hearts;     // Live_1, Live_2, Live_3... en orden

    void Start()
    {
        // Inicia con la mitad de la vida
        currentHealth = 5;
        UpdateHeartsUI();
    }

    // ------------------------------
    // MÉTODOS DE VIDA
    // ------------------------------

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHeartsUI();

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHeartsUI();
    }

    // ------------------------------
    // ACTUALIZAR UI
    // ------------------------------

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = LiveFull;
            else
                hearts[i].sprite = LiveEmpty;
        }
    }

    // ------------------------------
    // MUERTE DEL JUGADOR
    // ------------------------------

    void PlayerDie()
    {
        Debug.Log("El jugador murió.");
        // Aquí puedes poner animación, respawn, reiniciar nivel, etc.
    }
}