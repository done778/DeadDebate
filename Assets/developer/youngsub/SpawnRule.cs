using UnityEngine;

public enum SpawnPattern
{
    Random, //랜덤
    Circle, //원형
    Swarm,  //군집,무리
    Line,   //일렬로
    Boss,   //보스
}

[System.Serializable]
public struct SpawnRule
{
    [Header("Base Setting")]
    //해당 패턴 시작 시간
    [SpawnRuleField("pattern", (int)SpawnPattern.Random,(int) SpawnPattern.Line,(int) SpawnPattern.Swarm, (int)SpawnPattern.Circle)]
    public float startTime;
    //해당 패턴 끝 시간
    [SpawnRuleField("pattern", (int)SpawnPattern.Random,(int) SpawnPattern.Line,(int) SpawnPattern.Swarm, (int)SpawnPattern.Circle)]
    public float endTime;
    //해당 패턴에 사용될 에너미 타입
    [SpawnRuleField("pattern", (int)SpawnPattern.Random,(int) SpawnPattern.Line,(int) SpawnPattern.Swarm, (int)SpawnPattern.Circle)]
    public ENEMY_TYPE enemyType;
    //적을 스폰할 주기
    [SpawnRuleField("pattern", (int)SpawnPattern.Random,(int) SpawnPattern.Line,(int) SpawnPattern.Swarm, (int)SpawnPattern.Circle)]
    public float spawnInterval;
    //해당 패턴에 생성될 최대 마리수
    [SpawnRuleField("pattern", (int)SpawnPattern.Random,(int) SpawnPattern.Line,(int) SpawnPattern.Swarm, (int)SpawnPattern.Circle)]
    public int maxCount;

    [Header("Pattern Type")]
    //스폰 타입
    public SpawnPattern pattern;

    #region Pattern (Circle)
    [SpawnRuleField("pattern", (int)SpawnPattern.Circle)]
    public float circleRadius;

    [SpawnRuleField("pattern", (int)SpawnPattern.Circle)]
    public int circleCount;
    #endregion

    #region Pattern (Swarm)
    [SpawnRuleField("pattern", (int)SpawnPattern.Swarm)]
    //플레이어로 부터 거리
    public float SwarmDistance;

    [SpawnRuleField("pattern", (int)SpawnPattern.Swarm)]
    public int swarmMaxCount;

    [SpawnRuleField("pattern", (int)SpawnPattern.Swarm)]
    public float dispersion;
    #endregion

    #region Pattern (Line)
    [SpawnRuleField("pattern", (int)SpawnPattern.Line)]
    public int lineCount;

    [SpawnRuleField("pattern", (int)SpawnPattern.Line)]
    //어느 간격으로?
    public float lineGap;
    
    [SpawnRuleField("pattern", (int)SpawnPattern.Line)]
    public float lineDistance;

    #endregion

    //todo : 보스는 추후 BossRule로 나누는게 좋을듯
    #region Pattern (Boss)
    [SpawnRuleField("pattern", (int)SpawnPattern.Boss)]
    public float bossSpawnTime;

    [SpawnRuleField("pattern", (int)SpawnPattern.Boss)]
    public GameObject bossPrefab;

    [SpawnRuleField("pattern", (int)SpawnPattern.Boss)]
    public float changeValue;

    [SpawnRuleField("pattern", (int)SpawnPattern.Boss)]
    //보스 경고 시간
    public float warningTime;
    #endregion
}
