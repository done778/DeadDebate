using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage; // 공격력
    [SerializeField] private float bulletSpeed = 10f; // 총알속도
    [SerializeField] private float bulletLifeTime = 2f; // 총알유지시간

    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * bulletSpeed * Vector3.forward);

        timer += Time.deltaTime;
        if (timer >= bulletLifeTime)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
}
