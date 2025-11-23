using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    public float slideSpeed = 10f;      // velocidad del derrape
    public float slideDuration = 0.4f;  // cuánto dura el derrape
    
    private bool isSliding = false;
    private float slideTimer;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleSlideInput();
        UpdateSlideState();
    }

    void HandleSlideInput()
    {
        // Shift + W o Shift + S
        if (!isSliding && Input.GetKey(KeyCode.LeftShift) &&
            (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)))
        {
            StartSlide();
        }
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;

        anim.SetBool("Slide", true);

        // Dirección del derrape (arriba o abajo)
        float direction = Input.GetKey(KeyCode.W) ? 1f : -1f;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, direction * slideSpeed);
    }

    void UpdateSlideState()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                isSliding = false;
                anim.SetBool("Slide", false);
            }
        }
    }
}