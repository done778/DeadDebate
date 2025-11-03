using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    private bool isAttacking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.Log("Animator 연결필요");
        }
    }
    public void UpdateMovementAnimation(Vector3 direction)
    {
        if (isDead || isAttacking)
            return;

        if (direction.magnitude > 0)
        {
            PlayRun();
        }
        else
        {
            PlayIdle();
        }
    }

    public void PlayIdle()
    {
        if (isDead)
            return;
        animator.CrossFade("Idle_1", 0.1f);
    }
    public void PlayRun()
    {
        if (isDead)
            return;
        animator.SetFloat("move", 1);
    }
    public void PlayRunAttack()
    {
        if (isDead)
            return;
        isAttacking = true;
        animator.SetTrigger("isAttack");
        Invoke(nameof(ResetAttackState), 0.3f);
    }
    public void PlayDeath()
    {
        animator.SetBool("isDead", true);
    }
    private void ResetAttackState()
    {
        isAttacking = false;
    }
}

