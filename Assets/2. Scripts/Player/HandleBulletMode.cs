using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBulletMode : MonoBehaviour, IAttackMode
{
    public void Attack(PlayerController player)
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 target = ray.GetPoint(distance);

            //공격사거리 적용
            float distanceToTarget = Vector3.Distance(transform.position, target);
            if (distanceToTarget > player.AttackRange)
                return;

            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0f;

            transform.rotation = Quaternion.LookRotation(direction);

            player.ShootBullet();
        }
    }   
}
