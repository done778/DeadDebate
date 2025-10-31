using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBulletMode : MonoBehaviour, IAttackMode
{
    public void Attack(PlayerController player)
    {        
        List<EnemyController> enemies = EnemyPool.Instance.GetEnemies();

        if (enemies.Count == 0) // 적이 없으면
            return;

        EnemyController nearest = null; // 가장 가까운놈
        float minDistance = float.MaxValue; // 적 최소거리(임의 큰값)

        foreach (EnemyController enemyController in enemies)
        {
            //죽은 척 감지 안되게            
            if (enemyController == null || enemyController.IsDie)
                continue;

            float distance = Vector3.Distance(transform.position, enemyController.transform.position);

            //공격사거리 적용
            if (distance <= player.AttackRange && distance < minDistance)
            {
                minDistance = distance;
                nearest = enemyController;
            }
        }

        if (nearest != null)
        {
            Vector3 direction = (nearest.transform.position - transform.position).normalized;

            //direction.y = 0f; // 높이고정

            transform.rotation = Quaternion.LookRotation(direction); // 쏘는 방향보게

            player.ShootBullet();
        }
    }    
}
