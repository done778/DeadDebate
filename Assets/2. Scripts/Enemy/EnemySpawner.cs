using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class EnemySpawner : MonoBehaviour
{
    //플레이어 트랜스폼 주입 추후 삭제 - 기즈모 용
    public Transform trs;

    //스테이미 매니저로 옮겨야함
    public Transform bossTrs;

    public float maxRadius;
    public int exclusionRadius = 10;

    public List<GameObject> setEnemys = new List<GameObject>();
    private Dictionary<ENEMY_TYPE, GameObject> prefabs = new Dictionary<ENEMY_TYPE, GameObject>();

    private Vector3 createdPos;
    private GameObject curEnemy;

    public float timer;

    Coroutine spawnCoroutine = null;
    Coroutine bossSpawnCoroutine = null;
    WaitForSeconds spawnDelay = new WaitForSeconds(1f);

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        spawnCoroutine = StartCoroutine(EnemySpawn());
        bossSpawnCoroutine = StartCoroutine(BossSpawn());
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void Initialize()
    {
        //초기 세팅 프리팹리스트를 딕셔너리로 변경
        foreach (var enemy in setEnemys)
        {
            EnemyData data = enemy.GetComponent<EnemyController>().data;
            if (!prefabs.ContainsKey(data.enemyType))
                prefabs.Add(data.enemyType, enemy);
        }

    }

    //스테이지매니저에서 호출~
    public void DoSpawnedEnemy()
    {
        spawnCoroutine = StartCoroutine(EnemySpawn());
    }

    public void DoSpawnedBoss()
    {
        bossSpawnCoroutine = StartCoroutine(BossSpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (GameManager.Instance.Playing)
        {
            //랜덤 위치 값에서 플레이어 중심에서 위치가 정해지도록
            createdPos = GetSpawnOffset() + GameManager.Instance.player.transform.position;

            //curEnemy = Instantiate(prefabs[ENEMY_TYPE.Normal], createdPos, Quaternion.identity);
            int randomCreate = Random.Range(0, 10) + 1;

            //todo : 테스트용 리팩토링 요망
            if (randomCreate < 8) randomCreate = 0;//근접
            else randomCreate = 1;//원거리

            curEnemy = Instantiate(setEnemys[randomCreate], createdPos, Quaternion.identity);
            curEnemy.GetComponent<EnemyController>().Init(GameManager.Instance.player);

            curEnemy.transform.SetParent(transform);

            yield return spawnDelay;
        }
    }

    IEnumerator BossSpawn()
    {
        //보스 등장 전 처리할 로직들--------
        //yield return WaitUntil(위에 로직이 다 될 동안 기다려)

        //3분후 생성
        yield return new WaitForSeconds(4f);
        GameObject boss = Instantiate(prefabs[ENEMY_TYPE.Normal], bossTrs);
        boss.GetComponent<EnemyController>().Init(GameManager.Instance.player, 3, true);
        boss.transform.localScale *= 5f;

        boss.transform.SetParent(transform);
    }

    private void OnDestroy()
    {
        spawnCoroutine = null;
        bossSpawnCoroutine = null;
    }

    private Vector3 GetSpawnOffset()
    {
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
