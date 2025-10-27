using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="createEnemyData", menuName ="data/enemy")]
public class EnemyData : ScriptableObject
{
    public float healthPoint;
    public float attakPower;
    public float moveSpeed;
    public float attackSpeed;
    public int exp;
}
