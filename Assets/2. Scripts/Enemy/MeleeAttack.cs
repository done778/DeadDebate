using UnityEngine;

public class MeleeAttack : MonoBehaviour , IAttackBehaviour
{
    public void Execute(
        Transform enemyTransform, 
        PlayerController target, 
        int attackPower, 
        GameObject projectilePrefab = null, 
        Transform muzzle = null)
    {
        if(target != null){
            target.TakeDamage(attackPower);
        }
    }
}
