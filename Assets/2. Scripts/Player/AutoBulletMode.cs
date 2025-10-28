using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBulletMode : MonoBehaviour, IAttackMode
{
    public void Attack(PlayerContoller player)
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy"); // 적들을 담은 리스트

        if (enemys.Length == 0) // 적이 없으면
            return;

        GameObject nearest = null; // 가장 가까운놈
        float minDistance = float.MaxValue; // 적 최소거리(임의 큰값)

        foreach (GameObject enemy in enemys)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        if (nearest != null)
        {
            Vector3 direction = (nearest.transform.position - transform.position).normalized;
            direction.y = 0f;

            transform.rotation = Quaternion.LookRotation(direction); // 쏘는 방향보게

            player.ShootBullet();
        }
    }

    //private TYBulletController bulletController; // 불렛컨트롤러 참조
    //private Transform target; // 적 위치
    //[SerializeField] private float searchRadius = 50f; // 공격반응 반경
    //
    //private void Start()
    //{
    //    bulletController = GetComponent<TYBulletController>();
    //
    //    FindNearEnemy();
    //    
    //    if (target != null) // 적이 있으면 그방향으로
    //    {
    //        Vector3 direction = target.position - transform.position; // 적방향
    //
    //        direction.y = 0f;
    //
    //        direction.Normalize();
    //
    //        transform.forward = direction;
    //    }
    //    //적이 없으면 원래대로 앞으로 발사
    //}
    //
    ////가까운적 찾는 함수
    //private void FindNearEnemy()
    //{
    //    GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy"); // 적들을 담은 리스트
    //    float minDistance = float.MaxValue; // 적과의 최소거리를 우선 임의로 큰값
    //    Transform nearest = null; // 가장 가까운놈
    //
    //    foreach (GameObject enemy in enemys)
    //    {
    //        float distance = Vector3.Distance(transform.position, enemy.transform.position);
    //
    //        //더 가까운놈이 있는지
    //        if (distance < minDistance && distance <= searchRadius)
    //        {
    //            minDistance = distance;
    //            nearest = enemy.transform;
    //        }
    //    }
    //
    //    target = nearest;
    //}   
}
