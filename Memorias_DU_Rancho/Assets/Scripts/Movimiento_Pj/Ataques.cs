using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAttackInput();
    }

    void HandleAttackInput()
    {
        // --- Attack 1 : X + W ---
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.W))
        {
            anim.SetTrigger("Attack1");
        }

        // --- Attack 2 : X + S ---
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.S))
        {
            anim.SetTrigger("Attack2");
        }
    }
}