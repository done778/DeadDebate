using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UIInstance { get; private set; }

    // 로비 씬 관련 변수
    private GameObject charSelectPanel;

    // 스테이지 씬 관련 변수
    private GameObject pausePanel;
    private GameObject stageClearPanel;
    private GameObject gameoverPanel;
    private GameObject levelUpPanel;
    private PlayerController curPlayer;

    bool isLoading;

    void Awake()
    {
        if (UIInstance != null && UIInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            UIInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading) return;
        isLoading = true;
        SceneManager.LoadScene(sceneName);
        isLoading = false;
    }

    public void EnterLobby()
    {
        charSelectPanel?.SetActive(false);
    }

    public void GameStart()
    {
        curPlayer = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        pausePanel = GameObject.Find("PauseUI");
        stageClearPanel = GameObject.Find("StageClearUI");
        gameoverPanel = GameObject.Find("GameOverUI");
        levelUpPanel = GameObject.Find("LevelUpUI");

        pausePanel.SetActive(false);
        gameoverPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        stageClearPanel.SetActive(false);

        curPlayer.OnPlayerDie += () => SetGameoverPanel(true);
        curPlayer.OnLevelUp += (int temp) => SetSelectPanel(true);
    }

    public void SetGameoverPanel(bool isOpen)
    {
        gameoverPanel.SetActive(isOpen);
    }
    public void SetStageClearPanel(bool isOpen)
    {
        stageClearPanel.SetActive(isOpen);
    }
    public void SetPausePanel(bool isOpen)
    {
        pausePanel.SetActive(isOpen);
    }
    public void SetSelectPanel(bool isOpen)
    {
        levelUpPanel.SetActive(isOpen);
    }
    public void SetCharSelectPanel(bool isOpen)
    {
        charSelectPanel.SetActive(isOpen);
    }

    // 스테이지에서 퇴장 시 구독들 해지하고 나가야 함.
    public void ExitStage(string goScene)
    {
        curPlayer.OnPlayerDie += () => SetGameoverPanel(true);
        curPlayer.OnLevelUp += (int temp) => SetSelectPanel(true);
        LoadScene(goScene);
    }

    public void RegistCharSelectPanel(GameObject controller)
    {
        charSelectPanel = controller;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Stage"))
        {
            GameStart();
        }
        if (scene.name == "Lobby")
        {
            EnterLobby();
        }
    }
}
