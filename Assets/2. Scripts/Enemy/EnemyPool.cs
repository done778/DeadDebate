using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [Header("Enemy Setting")]
    public List<GameObject> prefabs = new List<GameObject>();
    //타입 당 미리 생성될 적의 마리수 300마리 정도 넉넉하게 잡기
    public int count = 300;

    private Dictionary<ENEMY_TYPE, Queue<GameObject>> pool = new Dictionary<ENEMY_TYPE, Queue<GameObject>>();
    //현재까지 생성된 적들 리스트
    private List<EnemyController> enemies = new List<EnemyController>();
    public GameObject Boss{ get; private set; }

    //적이 죽었을 때의 이벤트
    public event Action OnDeath;

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

    //현재까지 생성된 에너미 정보들
    public List<EnemyController> GetEnemies()
    {
        return enemies;
    }

    public int GetEnemies(ENEMY_TYPE type)
    {
        return pool[type].Where(x => x.activeInHierarchy).Count();
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

                enemies.Add(enemy);
            }
            else
            {
                Debug.LogError($"{type} 해당 타입의 적을 다 사용함");
                //todo : 다 사용하면 새로 생성할지 의논하기
            }
        }

        return enemy;
    }

    public void OnDeathAction()
    {
        OnDeath?.Invoke();
    }

    public void ReturnEnemy(GameObject enemy)
    {
        EnemyController controller = enemy.GetComponent<EnemyController>();

        enemy.SetActive(false);
        enemies.Remove(controller);
        pool[controller.data.enemyType].Enqueue(enemy);
    }

    public void AllReturn()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy && !enemy.isBoss)
            {
                enemy.gameObject.SetActive(false);
                pool[enemy.data.enemyType].Enqueue(enemy.gameObject);
            }
        }

        enemies.Clear();
    }

    public void SetBoss(EnemyController boss){
        enemies.Add(boss);
    }
}
