using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBulletMode : MonoBehaviour, IAttackMode
{
    public void Attack(PlayerController player)
    {
        //마우스포인터 기준으로 발사방향
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!plane.Raycast(ray, out float distance))
            return;

        Vector3 target = ray.GetPoint(distance);
        Vector3 direction = (target - transform.position).normalized;

        //direction.y = 0f; // 높이고정

        transform.rotation = Quaternion.LookRotation(direction); // 마우스방향 보기

        //공격사거리 안에 적이 있는지
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        bool isEnemyInRange = false;

        foreach (GameObject enemy in enemys)
        {
            //죽은적 감지 안되게
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController == null || enemyController.IsDie)
                continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= player.AttackRange)
            {
                isEnemyInRange = true;
                break;
            }
        }

        //사거리 내에 적이있으면 마우스방향으로 발사
        if (isEnemyInRange)
        {
            player.ShootBullet();
        }        
    }   
}
