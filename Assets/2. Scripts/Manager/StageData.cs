using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewStageData",menuName ="data/stage")]
public class StageData : ScriptableObject
{
    public string stageName = "Stage 1 : Volcano";
    public int surviveTime = 180;
    public List<SpawnRule> spawnRules = new List<SpawnRule>();
}
