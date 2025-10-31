using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // 
    [Header("Stage Configuration")]
    public StageData currentStageData;
    public EnemySpawner spawner;

    [Header("Spawn Settings")]
    public Transform playerTransform;
    public float spawnRadius = 20f;

    private float gameTime = 0f;
    private Dictionary<ENEMY_TYPE, float> ruleTimers = new Dictionary<ENEMY_TYPE, float>();
    private bool isBossSpawn = false;

    private void Start()
    {
        GameManager.Instance.SurviveTime = currentStageData.surviveTime;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (currentStageData == null)
        {
            Debug.LogError("스테이지 데이터가 없음");
            return;
        }

        foreach (var rule in currentStageData.spawnRules)
        {
            if (rule.pattern == SpawnPattern.Boss)
            {
                if (isBossSpawn) continue;

                if (gameTime >= rule.bossSpawnTime)
                {
                    isBossSpawn = true;
                    SpawnEnemy(rule);
                }
            }
            else
            {
                // 규칙의 시작 시간과 종료 시간 사이에 있는지 확인
                if (gameTime >= rule.startTime && gameTime < rule.endTime)
                {

                    ProcessSpawnRule(rule);
                }
            }
        }
    }

    private void ProcessSpawnRule(SpawnRule rule)
    {
        //각 적 프리팹별로 타이머를 관리하여 스폰 간격을 체크
        if (!ruleTimers.ContainsKey(rule.enemyType))
        {
            ruleTimers[rule.enemyType] = 0f;
        }

        ruleTimers[rule.enemyType] += Time.deltaTime;

        if (ruleTimers[rule.enemyType] >= rule.spawnInterval)
        {
            //필드 내 최대 개체 수 확인
            int currentEnemyCount = EnemyPool.Instance.GetEnemies(rule.enemyType);
            if (currentEnemyCount < rule.maxCount)
            {
                SpawnEnemy(rule);
            }

            // 스폰했으니 타이머 초기화
            ruleTimers[rule.enemyType] = 0f;
        }


    }


    private void SpawnEnemy(SpawnRule rule)
    {
        switch (rule.pattern)
        {
            case SpawnPattern.Random:
                spawner.SpawnRandom(rule.enemyType, rule.spawnInterval);
                break;
            case SpawnPattern.Circle:
                spawner.SpawnInCircle(rule.enemyType, rule.circleRadius, rule.circleCount);
                break;
            case SpawnPattern.Swarm:
                spawner.SpawnSwarm(rule.enemyType, rule.SwarmDistance, rule.swarmMaxCount, rule.dispersion);
                break;
            case SpawnPattern.Line:
                spawner.SpawnLine(rule.enemyType, rule.lineCount, rule.lineGap, rule.lineDistance);
                break;
            case SpawnPattern.Boss:
                spawner.SpawnBoss(rule.bossPrefab, rule.changeValue, rule.warningTime);
                break;
        }
    }
}

