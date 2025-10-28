using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{
    [SerializeField] TMP_Text KillText;
    private int killCount = 0;

    private void Start()
    {
        KillText.text = "kill : 0";

        if (GameManager.Instance != null )
        {
            GameManager.Instance.OnKillCount += UpdateKillText;
        }
    }

    private void OnDestroy()
    {
        if ( GameManager.Instance != null )  //중복방지
        {
            GameManager.Instance.OnKillCount -= UpdateKillText;
        }
    }

    void UpdateKillText()
    {
        killCount++;  // 이벤트 발생 시마다 카운트 증가 + 텍스트 갱신
        KillText.text = $"Kill : {killCount}";
    }

}
