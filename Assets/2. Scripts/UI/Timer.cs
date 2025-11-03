using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    // Timer UI 는 오직 UI 업데이트만 담당.
    // 실제 시간 측정은 하지않음

    public int remainTime;
    public TextMeshProUGUI textTimer;

    private void Start()
    {
        remainTime = GameManager.Instance.SurviveTime;
        textTimer.text = $"Time  {remainTime / 60:00}:{remainTime % 60:00}";
        GameManager.Instance.OnTimerUpdate += UpdateTimerUI;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnTimerUpdate -= UpdateTimerUI;
    }

    private void UpdateTimerUI()
    {
        remainTime--;
        textTimer.text = $"Time : {remainTime / 60:00}:{remainTime % 60:00}";
    }
}
