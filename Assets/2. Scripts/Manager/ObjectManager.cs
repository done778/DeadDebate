using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    [SerializeField] private int playerBulletCount = 20; // 일단 20발
    [SerializeField] private int enemyBulletCount = 50; // 화면에 보여질 총 개수임(마리당 아님!)

    private Dictionary<string, Queue<GameObject>> bulletPool = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitailizePool(GameManager.Instance.GetPrefab("Bullet"), playerBulletCount);
        InitailizePool(GameManager.Instance.GetPrefab("EnemyBullet"), enemyBulletCount);
    }

    private void InitailizePool(GameObject prefab, int count)
    {
        Queue<GameObject> pool = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(prefab,transform);
            go.SetActive(false);
            go.transform.SetParent(transform);
            pool.Enqueue(go);
        }

        if (!bulletPool.ContainsKey(prefab.name))
        {
            bulletPool.Add(prefab.name, pool);
        }
    }

    #region bullet(player, enemy)
    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(GameManager.Instance.GetPrefab("Bullet"), transform);
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);

        return bullet;
    }

    //public GameObject GetPlayerBullet(Vector3 position, Quaternion rotation)
    //{
    //    GameObject bullet = playerBulletPool.Count > 0 ? playerBulletPool.Dequeue() : CreateNewBullet();

    //    bullet.transform.position = position;
    //    bullet.transform.rotation = rotation;
    //    bullet.SetActive(true);
    //    bullet.transform.SetParent(null);

    //    return bullet;
    //}

    //플레이어 총알 위치 안맞음 플레이어 머즐위치에서 나가게
    public GameObject GetPlayerBullet()
    {
        Queue<GameObject> pool = bulletPool["Bullet"];
        GameObject bullet = GetBullet(pool);

        //총알이 더 이상 없다면
        if (bullet == null)
        {
            pool.Enqueue(CreateNewBullet());
            bullet = pool.Dequeue();
        }

        return bullet;
    }

    public GameObject GetEnemyBullet()
    {
        return GetBullet(bulletPool["EnemyBullet"]);
    }

    private GameObject GetBullet(Queue<GameObject> pool)
    {
        GameObject bullet = null;

        if (pool.Count == 0)
        {
            // Debug.LogError($"풀에 오브젝트가 없음");
        }
        else
        {
            bullet = pool.Dequeue();
            bullet.SetActive(true);
            bullet.transform.SetParent(null);
        }

        return bullet;

    }

    public void ReturnBullet(GameObject bullet)
    {
        string name = bullet.name.Replace("(Clone)", "");
        Queue<GameObject> pool = new Queue<GameObject>();
        if (!bulletPool.TryGetValue(name, out pool))
        {
            // Debug.LogError($"{name}타입의 풀이 없습니다");
        }
        else
        {
            bullet.SetActive(false);
            bullet.transform.SetParent(transform);

            pool.Enqueue(bullet);
        }
    }
    #endregion
}
