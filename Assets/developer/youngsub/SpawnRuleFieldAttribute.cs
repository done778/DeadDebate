using UnityEngine;

public class SpawnRuleFieldAttribute : PropertyAttribute
{
    //조건이 되는 변수의 이름
    //ex)SpawnRule 내의 spawnPattern
    public string conditionalSourceField;

    //이 필드가 보여야 할때의 SpawnPattern 값
    //spawnpattern이 enum이니까 int로
    public int[] enumValues;

    public SpawnRuleFieldAttribute(string conditionalSourceField, params int[] enumValues)
    {
        this.conditionalSourceField = conditionalSourceField;
        this.enumValues = enumValues;
    }
}
