using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class EnemySpawner : MonoBehaviour
{
   

    [Header("수치 변경을 위한 변수")]
    public Transform trs;
    public float maxRadius;
    public int exclusionRadius = 10;
    
    public List<GameObject> setEnemys = new List<GameObject>();
    private Dictionary<ENEMY_TYPE,GameObject> prefabs = new Dictionary<ENEMY_TYPE, GameObject>();

    private Vector3 createdPos;
    private Vector3 spawnPos;
    public GameObject curEnemy;

    Coroutine spawnCoroutine = null;
    WaitForSeconds spawnDelay = new WaitForSeconds(1f);

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        spawnCoroutine = StartCoroutine(EnemySpawn());
    }

    private void Initialize()
    {
        //초기 세팅 프리팹리스트를 딕셔너리로 변경
        foreach(var enemy in setEnemys){
            EnemyData data = enemy.GetComponent<EnemyController>().data;
            prefabs.Add(data.enemyType, enemy);
        }
    }

    IEnumerator EnemySpawn()
    {
        while (GameManager.Instance.Playing)
        {
            //랜덤 위치 값에서 플레이어 중심에서 위치가 정해지도록
            createdPos = GetSpawnOffset() + GameManager.Instance.player.transform.position;

            curEnemy = Instantiate(prefabs[ENEMY_TYPE.Normal], createdPos, Quaternion.identity);
            curEnemy.GetComponent<EnemyController>().Init(GameManager.Instance.player);

            yield return spawnDelay;
        }
    }

    private Vector3 GetSpawnOffset(){
        float distance = Random.Range(exclusionRadius, maxRadius);

        float angle = Random.Range(0, 360f) * Mathf.Deg2Rad;

        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;
        return new Vector3(x, 0, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(trs.position, maxRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trs.position, exclusionRadius);
    }
}
