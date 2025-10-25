using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY
{
    Basic, Zombie, Golem, Wolf,Count
}

[CreateAssetMenu(fileName = "CreateEnemyData", menuName = "enemy/data")]
public class EnemyData : ScriptableObject
{
    public int healthPoint;
    public int attakPower;
    public float moveSpeed;
    public float attackSpeed;
}
