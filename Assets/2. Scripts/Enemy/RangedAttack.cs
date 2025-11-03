using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttackBehaviour
{
    public void Execute(
        Transform enemyTransform,
        PlayerController target,
        int attackPower,
        GameObject projectilePrefab = null,
        Transform muzzle = null)
    {
        if (target != projectilePrefab && muzzle != null)
        {
            GameObject bullet = ObjectManager.Instance.GetEnemyBullet();
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;
        }
    }
}
