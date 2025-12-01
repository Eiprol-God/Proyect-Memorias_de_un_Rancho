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
        // Attack1 → X + W
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetTrigger("Attack1");
                return;
            }

            if (Input.GetKey(KeyCode.S))
            {
                anim.SetTrigger("Attack2");
                return;
            }
        }
    }
}