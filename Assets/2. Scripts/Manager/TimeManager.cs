using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // 플레이 타임 측정을 담당한다.
    private float remainTime;
    public event Action timeOver;

    WaitForSeconds playTimer = new WaitForSeconds(1f);

    Coroutine TimerUpdate;

    private void Awake()
    {
        GameManager.Instance.RegistTimeManager(this);
    }

    private void OnDestroy()
    {
        StopCoroutine(TimerUpdate);
    }

    private void Update()
    {
        remainTime -= Time.deltaTime;
        if (remainTime <= 0)
        {
            // 시간이 다 됨을 알림.
            timeOver?.Invoke();
        }
    }
    public void Init()
    {
        remainTime = GameManager.Instance.SurviveTime;
        TimerUpdate = StartCoroutine(PlayingTimer());
    }

    // 플레이어가 살아있는 동안 플레이타임 측정 코루틴
    IEnumerator PlayingTimer()
    {
        while (GameManager.Instance.Playing)
        {
            GameManager.Instance.OnTimerUpdate?.Invoke();
            yield return playTimer;
        }
    }
}
