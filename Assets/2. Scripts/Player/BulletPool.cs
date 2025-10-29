using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 20; // 일단 20발

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 신상 풀 생성
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = CreateNewBullet();
            bulletPool.Enqueue(bullet);
        }

    }

    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);

        return bullet;
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = bulletPool.Count > 0 ? bulletPool.Dequeue() : CreateNewBullet();

        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
