using System.Collections;
using System.Collections.Generic;
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
            Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        }
    }
}
