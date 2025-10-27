using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject player;
    private Camera mainCamera;
    private Vector3 cameraPos;
    public readonly float surviveTime = 180f;
    private float playingTime;

    public event Action OnGameStart; // 게임 시작 이벤트
    public event Action OnGameOver; // 게임 종료 이벤트
    public event Action OnStageClear; // 스테이지 클리어 성공 이벤트
    public event Action OnStageFailed; // 스테이지 클리어 실패 이벤트

    // 한결님 UI 업데이트 메서드를 여기에 구독하세요.
    public event Action OnTimerUpdate; // 1초마다 타이머 이벤트
    public event Action OnKillCount; // 적 처치 시 이벤트
    public event Action OnPlayerHpChange; // 플레이어 HP 변동 이벤트

    private Coroutine timerUpdate;

    public bool Playing { get; private set; }

    WaitForSeconds playTimer = new WaitForSeconds(1f);

    // 싱글톤
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        // 카메라 초기화
        mainCamera = Camera.main;
        mainCamera.transform.rotation = Quaternion.Euler(80f, 0f, 0f);
    }

    void Update()
    {
        if (Playing == true)
        {
            //카메라가 플레이어를 따라 가도록 함. 20과 -4는 플레이어와의 거리 상수
            cameraPos.x = player.transform.position.x;
            cameraPos.y = player.transform.position.y + 20;
            cameraPos.z = player.transform.position.z - 4;
            mainCamera.transform.position = cameraPos;

            // 생존 시간 측정. UI에 업데이트
            playingTime += Time.deltaTime;
            if (playingTime >= surviveTime)
            {
                GameOver();
            }
        }
    }
    public void GameStart()
    {
        playingTime = 0f;
        Playing = true;
        timerUpdate = StartCoroutine(PlayingTimer());
    }

    public void GameOver()
    {
        bool isClear = true;
        Playing = false;
        StopCoroutine(timerUpdate);
        Time.timeScale = 0f;
        if (isClear)
        {
            OnStageClear?.Invoke();
        }
        else
        {
            OnStageFailed?.Invoke();
        }
    }

    // 플레이어가 살아있는 동안 플레이타임 측정 코루틴
    IEnumerator PlayingTimer()
    {
        while (Playing)
        {
            OnTimerUpdate?.Invoke();
            yield return playTimer;
        }
    }
}
