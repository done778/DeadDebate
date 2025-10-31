using System.Collections;
using UnityEngine;

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

    public event System.Action OnBossWarningStart;
    public event System.Action OnBossWarningEnd;

    #region lagacy

    //Coroutine spawnCoroutine = null;
    //Coroutine bossSpawnCoroutine = null;

    // Start is called before the first frame update
    //void Start()
    //{
    //    spawnCoroutine = StartCoroutine(EnemySpawn());
    //    bossSpawnCoroutine = StartCoroutine(BossSpawn());
    //}

    //private void OnDestroy()
    //{
    //    spawnCoroutine = null;
    //    bossSpawnCoroutine = null;
    //}

    //스테이지매니저에서 호출~
    //public void DoSpawnedEnemy()
    //{
    //    for (int i = 0; i < spawnPerWave; i++)
    //    {
    //        //랜덤 위치 값에서 플레이어 중심에서 위치가 정해지도록
    //        createdPos = GetSpawnOffset() + GameManager.Instance.player.transform.position;

    //        //curEnemy = Instantiate(prefabs[ENEMY_TYPE.Normal], createdPos, Quaternion.identity);
    //        int randomCreate = Random.Range(0, 10) + 1;

    //        //todo : 테스트용 리팩토링 요망
    //        if (randomCreate < 8) randomCreate = 0;//근접
    //        else randomCreate = 1;//원거리

    //        EnemyController controller = EnemyPool.Instance.GetEnemy((ENEMY_TYPE)randomCreate, createdPos);
    //        if (controller != null)
    //        {
    //            controller.Init(GameManager.Instance.player);
    //            controller.OnDeath += HandleEnemyDeath;
    //            currentEnemyCount++;
    //        }

    //    }
    //}

    //public void DoSpawnedBoss()
    //{
    //    //보스 등장 전 처리할 로직들--------

    //    EnemyController boss = EnemyPool.Instance.GetEnemy(ENEMY_TYPE.Warrior, bossTrs.position);

    //    boss.Init(GameManager.Instance.player, 3, true);
    //    boss.transform.localScale *= 5f;
    //    boss.OnDeath += HandleEnemyDeath;

    //    boss.transform.SetParent(transform);
    //}

    //IEnumerator EnemySpawn()
    //{
    //    while (GameManager.Instance.Playing)
    //    {
    //        DoSpawnedEnemy();

    //        yield return spawnDelay;
    //    }
    //}

    //IEnumerator BossSpawn()
    //{
    //    //보스 등장 전 처리할 로직들--------
    //    //yield return WaitUntil(위에 로직이 다 될 동안 기다려)

    //    //3분후 생성
    //    yield return new WaitForSeconds(400f);
    //    EnemyController boss = EnemyPool.Instance.GetEnemy(ENEMY_TYPE.Warrior, bossTrs.position);

    //    boss.Init(GameManager.Instance.player, 3, true);
    //    boss.transform.localScale *= 5f;

    //    boss.transform.SetParent(transform);
    //}
    #endregion

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

    #region SpawnMethod

    public void SpawnInCircle(ENEMY_TYPE type, float radius, int count)
    {
        Transform playerTransform = GameManager.Instance.player.transform;
        if (playerTransform == null)
        {
            Debug.LogError("플레이어 Transform을 찾을 수 없습니다.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            float angle = i * 360f / count;

            float radian = angle * Mathf.Deg2Rad;

            float x = radius * Mathf.Cos(radian);
            float z = radius * Mathf.Sin(radian);

            Vector3 spawnOffset = new Vector3(x, 0f, z);

            Vector3 spawnPosition = playerTransform.position + spawnOffset;

            EnemyController controller = EnemyPool.Instance.GetEnemy(type, spawnPosition);
            EnemyInit(controller);
        }
    }

    public void SpawnRandom(ENEMY_TYPE type, float spawnInterval)
    {
        Transform playerTransform = GameManager.Instance.player.transform;
        if (playerTransform == null)
        {
            Debug.LogError("플레이어 Transform을 찾을 수 없습니다.");
            return;
        }

        for (int i = 0; i < spawnPerWave; i++)
        {
            createdPos = GetSpawnOffset() + playerTransform.position;

            EnemyController controller = EnemyPool.Instance.GetEnemy(type, createdPos);
            EnemyInit(controller);
        }
    }

    public void SpawnSwarm(ENEMY_TYPE type, float distance, int swarmMaxCount, float dispersion)
    {
        Transform playerTransform = GameManager.Instance.player.transform;
        if (playerTransform == null)
        {
            Debug.LogError("플레이어 Transform을 찾을 수 없습니다.");
            return;
        }

        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0f;
        randomDirection.Normalize();

        Vector3 swarmCenter = playerTransform.position + randomDirection * distance;

        swarmCenter.y = 0f;

        for (int i = 0; i < swarmMaxCount; i++)
        {
            Vector3 offset = Random.insideUnitSphere * dispersion;
            offset.y = 0f;

            EnemyController controller = EnemyPool.Instance.GetEnemy(type, swarmCenter + offset);
            EnemyInit(controller);
        }
    }

    //플레이어의 앞뒤좌우중 일렬로 정거리 간격으로 생성
    public void SpawnLine(ENEMY_TYPE type, int count, float gap, float distance)
    {
        Transform playerTransform = GameManager.Instance.player.transform;
        PlayerController player = GameManager.Instance.CurPlayer;
        if (playerTransform == null)
        {
            Debug.LogError("플레이어 Transform을 찾을 수 없습니다.");
            return;
        }

        Vector3 spawnDirection = player.InputDirection != Vector3.zero ? player.InputDirection : playerTransform.forward;

        spawnDirection.Normalize();

        Vector3 lineCenter = playerTransform.position + spawnDirection * distance;
        lineCenter.y = 0f;


        //캐릭터의 방향의 앞 쪽에 생성
        Vector3 lineVector = Vector3.Cross(spawnDirection, Vector3.up);


        float totalLength = (count - 1) * gap;
        //첫번째 적 기준으로
        float startOffset = -totalLength / 2f;

        for (int i = 0; i < count; i++)
        {
            // 현재 적의 라인 위치
            float currentOffset = startOffset + i * gap;

            // 최종 스폰 위치 = 라인 중심 + (라인 축 방향 * 오프셋)
            Vector3 spawnPosition = lineCenter + lineVector * currentOffset;
            spawnPosition.y = 0f;

            EnemyController controller = EnemyPool.Instance.GetEnemy(type, spawnPosition);
            EnemyInit(controller);
        }
    }

    public void SpawnBoss(GameObject prefab, float changeValue, float warningTime)
    {
        StartCoroutine(BossSpawnSequence(prefab, changeValue, warningTime));
    }

    private IEnumerator BossSpawnSequence(GameObject bossPrefab, float changeValue, float warningTime)
    {
        Transform playerTransform = GameManager.Instance.player.transform;
        PlayerController player = GameManager.Instance.CurPlayer;
        if (playerTransform == null)
        {
            Debug.LogError("플레이어 Transform을 찾을 수 없습니다.");
            yield break;
        }
        //경고 UI 표시 시작
        OnBossWarningStart?.Invoke(); 

        yield return new WaitForSeconds(warningTime);

        //경고 UI 표시 끝
        OnBossWarningEnd?.Invoke(); 


        Vector3 spawnPosition = playerTransform.position + Random.insideUnitSphere * 40f;
        spawnPosition.y = 0f;

        //todo : 추후 보스컨트롤러 작성
        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        boss.transform.localScale *= 20f;
        EnemyController controller = boss.GetComponent<EnemyController>();
        EnemyBossInit(controller,changeValue);
    }

    private void EnemyInit(EnemyController controller)
    {
        if (controller != null)
        {
            controller.Init(GameManager.Instance.player);
            controller.OnDeath += HandleEnemyDeath;
            currentEnemyCount++;
        }
    }

    private void EnemyBossInit(EnemyController controller, float changeValue)
    {
        if (controller != null)
        {
            controller.Init(GameManager.Instance.player, (int)changeValue, true);
            controller.OnDeath += HandleEnemyDeath;
            currentEnemyCount++;
        }
    }
    #endregion
}
