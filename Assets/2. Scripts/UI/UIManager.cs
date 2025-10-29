using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UIInstance { get; private set; }

    // 로비 씬 관련 변수
    private GameObject characterSelectPanel;
    private Button characterButton;
    private GameObject charSelectPanel;

    // 스테이지 씬 관련 변수
    private GameObject gameoverPanel;
    private GameObject levelUpPanel;
    private PlayerController curPlayer;
    private PlayerStatButton onClickDetected;
    private Button lobbyButton;


    bool isLoading;

    public event Action CloseUIPanel;

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
        //characterButton = GameObject.Find("CharacterButton")?.GetComponent<Button>();
        //characterButton.onClick.AddListener(OpenCharacterSelectPanel);
        //onClickDetected.OnButtonClicked += (string statType) =>
        //{
        //    curPlayer.IncreaseStat(statType);
        //    CloseSelectPanel();
        //};
        charSelectPanel.SetActive(false);
    }

    public void GameStart()
    {
        curPlayer = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        gameoverPanel = GameObject.Find("GameOverUI");
        levelUpPanel = GameObject.Find("LevelUpUI");
        onClickDetected = GameObject.Find("OnClickEvent").GetComponent<PlayerStatButton>();
        lobbyButton = gameoverPanel.transform.GetChild(0).GetComponent<Button>();
        lobbyButton.onClick.AddListener(GoToLobby);

        gameoverPanel.SetActive(false);
        levelUpPanel.SetActive(false);

        curPlayer.OnPlayerDie += OpenGameoverPanel;
        curPlayer.OnLevelUp += (int temp) => OpenSelectPanel();
        onClickDetected.OnButtonClicked += (string temp) => CloseSelectPanel();
    }

    public void OpenGameoverPanel()
    {
        gameoverPanel.SetActive(true);
    }

    public void CloseGameoverPanel()
    {
        gameoverPanel.SetActive(true);
    }

    public void OpenSelectPanel()
    {
        levelUpPanel.SetActive(true);
    }

    public void CloseSelectPanel()
    {
        CloseUIPanel?.Invoke();
        levelUpPanel.SetActive(false);
    }

    public void OpenCharacterSelectPanel()
    {
        characterSelectPanel.SetActive(true);
    }

    public void CloseCharacterSelectPanel()
    {
        characterSelectPanel.SetActive(false);
    }
    public void GoToLobby()
    {
        curPlayer.OnPlayerDie -= OpenGameoverPanel;
        curPlayer.OnLevelUp -= (int temp) => OpenSelectPanel();
        onClickDetected.OnButtonClicked -= (string temp) => CloseSelectPanel();
        LoadScene("Lobby");
    }


    public void RegistCharSelectPanel(GameObject controller)
    {
        charSelectPanel = controller;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage")
        {
            GameStart();
        }
        if (scene.name == "Lobby")
        {
            EnterLobby();
        }
    }
}
