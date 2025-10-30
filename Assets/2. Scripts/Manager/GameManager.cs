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
    private PlayerController curPlayer;
    private TimeManager timeManager;
    private PrefabManager prefabManager;

    private float playingTime;

    public Action OnGameStart; // 게임 시작 이벤트
    public Action<bool> OnGameOver; // 게임 종료 이벤트
    public event Action OnStageClear; // 스테이지 클리어 성공 이벤트
    public event Action OnStageFailed; // 스테이지 클리어 실패 이벤트

    // 한결님 UI 업데이트 메서드를 여기에 구독하세요.
    public Action OnTimerUpdate; // 1초마다 타이머 이벤트
    public Action OnKillCount; // 적 처치 시 이벤트 (적 관리 쪽으로)
    // public event Action OnPlayerHpChange; // 플레이어 HP 변동 이벤트 (이거 플레이어 쪽으로)

    private Coroutine timerUpdate;

    public bool Playing { get; private set; }
    public int SurviveTime { get; private set; }
    public int SelectedPlayerType { get; private set; }

    

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
        SurviveTime = 180;
        SelectedPlayerType = 0;
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
        curPlayer = player.GetComponent<PlayerController>();

        // 플레이어가 죽으면 클리어 실패 메서드를 실행한다.
        curPlayer.OnPlayerDie += StageFailed;

        // 플레이어가 레벨 업하면 게임을 멈추라고 한다. (매개변수는 안씀)
        curPlayer.OnLevelUp += (int temp) => PauseGame();

        // 레벨 업 시 스탯 선택지 버튼을 클릭하면 게임을 재개한다.
        UIManager.UIInstance.CloseUIPanel += PlayGame;

        GameObject.Find("HpBar").GetComponent<HpBar>().Init(player);

        timeManager.Init();
        timeManager.timeOver += StageClear;

        PlayGame();
    }

    // 게임 종료 메서드.
    private void GameOver(bool isClear)
    {
        Playing = false;
        // 구독한 이벤트 모두 해제하고 일시정지
        curPlayer.OnPlayerDie -= StageFailed;
        curPlayer.OnLevelUp -= (int temp) => PauseGame();
        UIManager.UIInstance.CloseUIPanel -= PlayGame;
        timeManager.timeOver -= StageClear;
        PauseGame();
    }
    // 스테이지 클리어
    public void StageClear()
    {
        GameOver(true);
    }

    // 스테이지 실패
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

    // 매니저 쪽에서 이 메서드를 통해 자신을 등록하도록 함.
    // 기존의 게임매니저가 모든 오브젝트 순회하며 찾는 것보다 훨씬 효율적.
    public void RegistTimeManager(TimeManager manager)
    {
        timeManager = manager;
    }
    public void RegistPrefabManager(PrefabManager manager)
    {
        prefabManager = manager;
    }

    // 다른 매니저에서 프리팹을 가져갈 수 있게 함. (딕셔너리)
    public GameObject GetPrefab(string name)
    {
        GameObject prefab = prefabManager.GetPrefabFromKey(name);
        if (prefab == null) 
        {
            Debug.Log($"{name} 프리팹을 찾지 못했습니다.");
        }
        return prefab;
    }

    public void SetPlayerCharacter(int index)
    {
        SelectedPlayerType = index;
    }

    // Stage 씬 진입을 감지함
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage")
        {
            GameStart();
        }
    }
}
