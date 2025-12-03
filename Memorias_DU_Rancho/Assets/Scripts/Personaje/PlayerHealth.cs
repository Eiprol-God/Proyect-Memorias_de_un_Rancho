using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Manejo de Muerte")]
    [Tooltip("Arrastra aquí el objeto del Jugador que tiene el script PlayerDeathHandler.")]
    public PlayerDeathHandler deathHandler;

    [Header("UI")]
    public Sprite LiveFull;
    public Sprite LiveEmpty;
    public Image[] hearts;  // Live_1, Live_2, Live_3...

    [Header("Daño y Animaciones")]
    public float invincibleTime = 0.4f;
    private bool isInvincible = false;
    private bool isDead = false;

    private Animator anim;
    private Collider2D playerCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

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
        if (isDead) return; // Prevenir llamadas múltiples
        isDead = true;
        Debug.Log("El jugador murió.");

        // Animación de muerte
        anim.SetTrigger("Death");

        // Desactivar el movimiento si existe
        if (TryGetComponent<PlayerMovement>(out var move))
            move.enabled = false;

        // Desactivar ataque si existe
        if (TryGetComponent<PlayerAttack>(out var atk))
            atk.enabled = false;

        // Hacer el collider más pequeño
        if (playerCollider != null)
        {
            if (playerCollider is CapsuleCollider2D capsule)
            {
                // Reduce la altura de la cápsula y la baja un poco
                capsule.size = new Vector2(capsule.size.x, 0.5f);
                capsule.offset = new Vector2(capsule.offset.x, -0.75f);
            }
            else if (playerCollider is BoxCollider2D box)
            {
                // Reduce la altura de la caja y la baja un poco
                box.size = new Vector2(box.size.x, 0.5f);
                box.offset = new Vector2(box.offset.x, -0.75f);
            }
        }

        // *** NUEVA LÍNEA AÑADIDA ***
        // Llamar al manejador de la pantalla de muerte
        if (deathHandler != null)
        {
            deathHandler.HandlePlayerDeath();
        }
        else
        {
            Debug.LogError("No se ha asignado el 'deathHandler' en el script PlayerHealth. No se puede mostrar la pantalla de muerte.");
        }
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