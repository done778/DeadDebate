using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ATTAK_TYPE { 
    None, //몸박만
    Melee, //근접
    Ranged,//원거리
    Hybrid,//하이브리드
}
public enum ENEMY_TYPE
{
    Warrior,Archer
}


[CreateAssetMenu(fileName = "createEnemyData", menuName = "data/enemy")]
public class EnemyData : ScriptableObject
{
    public int healthPoint;
    public int attakPower;
    public int exp;

    public float moveSpeed;
    public float attackSpeed;
    public float range;

    public ENEMY_TYPE enemyType;
    public ATTAK_TYPE attackType;

   public EnemyData GetCopy(){
        EnemyData copy = Instantiate(this);
        return copy;
   }
}
