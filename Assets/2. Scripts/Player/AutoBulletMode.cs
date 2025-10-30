using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBulletMode : MonoBehaviour, IAttackMode
{
    public void Attack(PlayerController player)
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy"); // 적들을 담은 리스트

        if (enemys.Length == 0) // 적이 없으면
            return;

        GameObject nearest = null; // 가장 가까운놈
        float minDistance = float.MaxValue; // 적 최소거리(임의 큰값)

        foreach (GameObject enemy in enemys)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            //공격사거리 적용
            if (distance <= player.AttackRange && distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        if (nearest != null)
        {
            Vector3 direction = (nearest.transform.position - transform.position).normalized;

            //direction.y = 0f; // 높이고정

            transform.rotation = Quaternion.LookRotation(direction); // 쏘는 방향보게

            player.ShootBullet(nearest.transform.position);
        }
    }    
}
