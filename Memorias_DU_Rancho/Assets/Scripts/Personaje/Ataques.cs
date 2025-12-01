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
        // Attack1 → X
        if (Input.GetKeyDown(KeyCode.X))
        {
                anim.SetTrigger("Attack1");
                return;
         }
    }
}