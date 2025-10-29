using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackBehaviour
{
    public void Execute(
        Transform enemyTransform,
        PlayerContoller target,
        int attackPower,
        GameObject projectilePrefab = null,
        Transform muzzle = null);
}
