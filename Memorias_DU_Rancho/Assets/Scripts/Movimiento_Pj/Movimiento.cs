using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;          // Velocidad normal
    public float runSpeed = 9f;       // Velocidad corriendo
    public float idleDelay = 3f;      // Tiempo para activar Idle_Mov

    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput;
    private float idleTimer = 0f;
    private bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // --- Movimiento y correr ---
        moveInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isRunning ? runSpeed : speed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        // --- Flip del personaje ---
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // --- Idle timer ---
        if (moveInput == 0)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0;
        }

        // --- Animaciones ---
        if (moveInput != 0) // caminando o corriendo
        {
            anim.SetBool("Idle_static", false);
            anim.SetBool("Idle_Mov", false);

            if (isRunning)
            {
                anim.SetBool("Walking", false);
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Walking", true);
                anim.SetBool("Run", false);
            }
        }
        else // quieto
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Run", false);

            if (idleTimer >= idleDelay)
            {
                anim.SetBool("Idle_static", false);
                anim.SetBool("Idle_Mov", true);
            }
            else
            {
                anim.SetBool("Idle_static", true);
                anim.SetBool("Idle_Mov", false);
            }
        }
    }
}
