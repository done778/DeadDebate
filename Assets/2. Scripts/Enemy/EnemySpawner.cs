using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class EnemySpawner : MonoBehaviour
{
    //스테이미 매니저로 옮겨야함
    public Transform bossTrs;

    [Header("Spawn Settings")]
    [SerializeField] private Transform player;

    //스폰 최대 반경
    [SerializeField] private float spawnRadius = 20f;
    //스폰 제외 반경
    [SerializeField] private float exclusionRadius = 10f;
    //스폰 주기
    [SerializeField] private WaitForSeconds spawnDelay = new WaitForSeconds(1f);
    //스폰 주기 당 마리수
    [SerializeField] private int spawnPerWave = 1;
    //스폰 위치
    private Vector3 createdPos;
    //현재 생성된 적 마리수
    [SerializeField] private int currentEnemyCount = 0;


    Coroutine spawnCoroutine = null;
    Coroutine bossSpawnCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        spawnCoroutine = StartCoroutine(EnemySpawn());
        bossSpawnCoroutine = StartCoroutine(BossSpawn());
    }

    //스테이지매니저에서 호출~
    public void DoSpawnedEnemy()
    {
        for (int i = 0; i < spawnPerWave; i++)
        {
            //랜덤 위치 값에서 플레이어 중심에서 위치가 정해지도록
            createdPos = GetSpawnOffset() + GameManager.Instance.player.transform.position;

            //curEnemy = Instantiate(prefabs[ENEMY_TYPE.Normal], createdPos, Quaternion.identity);
            int randomCreate = Random.Range(0, 10) + 1;

            //todo : 테스트용 리팩토링 요망
            if (randomCreate < 8) randomCreate = 0;//근접
            else randomCreate = 1;//원거리

            EnemyController controller = EnemyPool.Instance.GetEnemy((ENEMY_TYPE)randomCreate, createdPos);
            if (controller != null)
            {
                controller.Init(GameManager.Instance.player);
                controller.OnDeath += HandleEnemyDeath;
                currentEnemyCount++;
            }

        }
    }

    public void DoSpawnedBoss()
    {
        //보스 등장 전 처리할 로직들--------

        EnemyController boss = EnemyPool.Instance.GetEnemy(ENEMY_TYPE.Warrior, bossTrs.position);

        boss.Init(GameManager.Instance.player, 3, true);
        boss.transform.localScale *= 5f;
        boss.OnDeath += HandleEnemyDeath;

        boss.transform.SetParent(transform);
    }

    IEnumerator EnemySpawn()
    {
        while (GameManager.Instance.Playing)
        {
            DoSpawnedEnemy();

            yield return spawnDelay;
        }
    }

    IEnumerator BossSpawn()
    {
        //보스 등장 전 처리할 로직들--------
        //yield return WaitUntil(위에 로직이 다 될 동안 기다려)

        //3분후 생성
        yield return new WaitForSeconds(400f);
        EnemyController boss = EnemyPool.Instance.GetEnemy(ENEMY_TYPE.Warrior, bossTrs.position);

        boss.Init(GameManager.Instance.player, 3, true);
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
        float distance = Random.Range(exclusionRadius, spawnRadius);

        float angle = Random.Range(0, 360f) * Mathf.Deg2Rad;

        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;
        return new Vector3(x, 0, z);
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(player.position, spawnRadius);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(player.position, exclusionRadius);
    //}
}
