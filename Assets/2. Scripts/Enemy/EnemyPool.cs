using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [Header("Enemy Setting")]
    public List<GameObject> prefabs = new List<GameObject>();
    public int count = 50;

    private Dictionary<ENEMY_TYPE, Queue<GameObject>> pool = new Dictionary<ENEMY_TYPE, Queue<GameObject>>();

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
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            EnemyData data = prefabs[i].GetComponent<EnemyController>().data;
            Queue<GameObject> enemyPool = new Queue<GameObject>();
            if (!pool.ContainsKey(data.enemyType))
            {
                for (int j = 0; j < count; j++)
                {
                    GameObject enemy = Instantiate(prefabs[i], transform);
                    enemy.transform.SetParent(transform);
                    enemy.SetActive(false);
                    enemyPool.Enqueue(enemy);
                }

                pool.Add(data.enemyType, enemyPool);
            }
        }
    }

    public EnemyController GetEnemy(ENEMY_TYPE type, Vector3 pos)
    {
        EnemyController enemy = null;

        if (!pool.ContainsKey(type))
        {
            Debug.LogError($"{type} 해당 타입의 풀이 없음");
        }
        else
        {
            Queue<GameObject> enemyPool = pool[type];
            if (enemyPool.Count > 0)
            {
                enemy = enemyPool.Dequeue().GetComponent<EnemyController>();
                enemy.gameObject.SetActive(true);
                enemy.transform.position = pos;
                //회전값이 필요하려나
                //enemy.transform.rotation = ???
            }
            else
            {
                Debug.LogError($"{type} 해당 타입의 적을 다 사용함");
                //todo : 다 사용하면 새로 생성할지 의논하기
            }
        }

        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        EnemyData data = enemy.GetComponent<EnemyController>().data;
        enemy.SetActive(false);
        pool[data.enemyType].Enqueue(enemy);
    }
}
