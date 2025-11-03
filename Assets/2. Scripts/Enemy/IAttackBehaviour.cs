using UnityEngine;

public interface IAttackBehaviour
{
    public void Execute(
        Transform enemyTransform,
        PlayerController target,
        int attackPower,
        GameObject projectilePrefab = null,
        Transform muzzle = null);
}
