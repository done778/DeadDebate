using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject player;
    private PlayerContoller curPlayer;

    public readonly float surviveTime = 180f;
    private float playingTime;

    public Action OnGameStart; // 게임 시작 이벤트
    public Action<bool> OnGameOver; // 게임 종료 이벤트
    public event Action OnStageClear; // 스테이지 클리어 성공 이벤트
    public event Action OnStageFailed; // 스테이지 클리어 실패 이벤트

    // 한결님 UI 업데이트 메서드를 여기에 구독하세요.
    public event Action OnTimerUpdate; // 1초마다 타이머 이벤트
    public event Action OnKillCount; // 적 처치 시 이벤트 (적 관리 쪽으로)
    // public event Action OnPlayerHpChange; // 플레이어 HP 변동 이벤트 (이거 플레이어 쪽으로)

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
        SceneManager.sceneLoaded += OnSceneLoad;
        OnGameStart = GameStart;
        OnGameOver = GameOver;
        Playing = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // Stage 씬에 들어가면 실행될 메서드. 게임 시작을 위한 초기화
    public void GameStart()
    {
        playingTime = 0f;
        Playing = true;

        // 플레이어를 태그로 찾습니다.
        player = GameObject.FindWithTag("Player");
        curPlayer = player.GetComponent<PlayerContoller>();

        // 플레이어가 죽으면 클리어 실패 메서드를 실행한다.
        curPlayer.OnPlayerDie += StageFailed;

        // 플레이어가 레벨 업하면 게임을 멈추라고 한다. (매개변수는 안씀)
        curPlayer.OnLevelUp += (int temp) => PauseGame();

        // 레벨 업 시 스탯 선택지 버튼을 클릭하면 게임을 재개한다.
        UIManager.UIInstance.CloseUIPanel += PlayGame;

        // 1초마다 OnTimerUpdate 인보크 발생시키는 코루틴 시작
        timerUpdate = StartCoroutine(PlayingTimer());

        PlayGame();
    }

    // 게임 종료 메서드.
    private void GameOver(bool isClear)
    {
        Playing = false;
        // 코루틴 멈추고, 구독한 이벤트 모두 해제하고 일시정지
        StopCoroutine(timerUpdate);
        curPlayer.OnPlayerDie -= StageFailed;
        curPlayer.OnLevelUp -= (int temp) => PauseGame();
        UIManager.UIInstance.CloseUIPanel -= PlayGame;
        PauseGame();
    }
    // 게임 일시 정지
    public void StageClear()
    {
        GameOver(true);
    }

    // 게임 재개
    public void StageFailed()
    {
        GameOver(false);
    }

    // 게임 일시 정지
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // 게임 재개
    public void PlayGame()
    {
        Time.timeScale = 1f;
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

    // Stage 씬 진입을 감지함
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StageYH")
        {
            GameStart();
        }
    }
}
