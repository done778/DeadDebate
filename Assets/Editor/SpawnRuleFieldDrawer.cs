using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(SpawnRuleFieldAttribute))]
public class SpawnRuleFieldDrawer : PropertyDrawer
{
    //필드 높이 계산
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //조건이 맞지 않으면 높이를 0으로
        if (!CheckCondition(property)) return 0f;

        //조건이 맞으면 기본 높이로
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    //필드를 실제로 보여줌
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (CheckCondition(property))
        {
            //조건이 맞으면 필드를 그려줌    
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
    //조건 체크하기
    private bool CheckCondition(SerializedProperty property)
    {
        SpawnRuleFieldAttribute attribute = (SpawnRuleFieldAttribute)this.attribute;

        //조건이 되는 변수 찾기 SpawnPattern 찾기
        string path = property.propertyPath.Replace(property.name, attribute.conditionalSourceField);
        SerializedProperty sourceProperty = property.serializedObject.FindProperty(path);

        if(sourceProperty != null){
            //sourceProperty가 Enum 타입이므로 enum 값 비교
            int currentEnumValue = sourceProperty.enumValueIndex;
            return attribute.enumValues.Contains(currentEnumValue);
        }

        //변수를 찾지 못했다면 안전하게 true반환하여 보이게함
        return true;
    }
}
