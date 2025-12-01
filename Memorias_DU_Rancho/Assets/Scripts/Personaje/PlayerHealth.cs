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
    public Image[] hearts;  // Live_1, Live_2, Live_3...

    [Header("Daño y Animaciones")]
    public float invincibleTime = 0.4f;
    private bool isInvincible = false;
    private bool isDead = false;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Inicia con la mitad de la vida (lo dejamos como tú lo tenías)
        currentHealth = 5;
        UpdateHeartsUI();
    }

    // ------------------------------
    // MÉTODOS DE VIDA
    // ------------------------------

    public void TakeDamage(int amount)
{
    Debug.Log("TakeDamage FUE LLAMADO. Daño a recibir: " + amount);
    if (isDead || isInvincible)
    {
        Debug.Log("TakeDamage ignorado. Razón: isDead=" + isDead + ", isInvincible=" + isInvincible);
        return;
    }

    currentHealth -= amount;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    UpdateHeartsUI();

    // Si la vida llega a 0 NO reproducimos Hurt
    if (currentHealth <= 0)
    {
        PlayerDie();
        return;
    }

    // Animación de daño SOLO si está vivo
    Debug.Log("Intentando activar trigger 'Hurt'. Vida actual: " + currentHealth);
    anim.SetTrigger("Hurt");

    StartCoroutine(InvincibilityFrames());
}


    public void Heal(int amount)
    {
        if (isDead) return;

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
        isDead = true;
        Debug.Log("El jugador murió.");

        // Animación de muerte
        anim.SetBool("Death", true);

        // Desactivar el movimiento si existe
        if (TryGetComponent<PlayerMovement>(out var move))
            move.enabled = false;

        // Desactivar ataque si existe
        if (TryGetComponent<PlayerAttack>(out var atk))
            atk.enabled = false;

        // Opcional:
        // GetComponent<Rigidbody2D>().simulated = false;
        // GetComponent<Collider2D>().enabled = false;
    }

    // ------------------------------
    // INVINCIBILITY FRAMES
    // ------------------------------

    private System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}