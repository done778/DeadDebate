using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpUIController : MonoBehaviour
{

    string[] stats = { "AttackPower", "AttackSpeed", "HealthPoint", "MoveSpeed" };
    string[] selectedStats = new string[3];
    Button[] childButton = null;

    private void OnEnable()
    {
        Debug.Log("OnEnable 실행");
        // 활성화가 되면 우선 스탯 중 랜덤 3개를 가져온다.
        RandomSelectThree();
        Debug.Log("랜덤 3개 선택 완료");

        // 자식 버튼 가져오기.
        childButton = transform.GetComponentsInChildren<Button>();

        Debug.Log($"텍스트 컴포넌트 가져오기 완료 : 자식 수 {childButton.Length}");

        TextMeshProUGUI child;

        for (int i = 0; i < childButton.Length; i++)
        {
            child = childButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Debug.Log(selectedStats[i]);
            Debug.Log(childButton[i].name);
            child.text = selectedStats[i];
        }
        // 클릭하면 선택한 스탯의 정보를 담아서 Invoke
        // 저 이벤트를 게임 매니저랑 UI 매니저가 게임을 다시 재개하는 메서드를 구독시켜둔다.
    }

    private void RandomSelectThree()
    {
        int index = 0;
        
        while (index < selectedStats.Length)
        {
            bool alreadyExist = false;
            int selected = Random.Range(0, stats.Length); // 0, 4 니까 0 ~ 3
            for (int i = 0; i < index; i++) {
                if (selectedStats[index] == stats[selected])
                {
                    alreadyExist = true;
                    break;
                }
            }
            if (!alreadyExist) { 
                selectedStats[index] = stats[selected];
                index++;
            }
        }
    }
}
