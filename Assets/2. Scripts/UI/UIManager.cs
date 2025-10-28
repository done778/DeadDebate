using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager UIInstance { get; private set; }
    private GameObject gameoverPanel;
    private GameObject levelUpPanel;
    private PlayerContoller curPlayer;
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

    // Stage 씬 진입 시 UI 매니저 초기화
    public void GameStart()
    {
        curPlayer = GameObject.FindWithTag("Player").GetComponent<PlayerContoller>();
        gameoverPanel = GameObject.Find("GameOverUI");
        levelUpPanel = GameObject.Find("LevelUpUI");
        gameoverPanel.SetActive(false);
        levelUpPanel.SetActive(false);

        curPlayer.OnPlayerDie += OpenGameoverPanel;
        curPlayer.OnLevelUp += (int temp) => OpenSelectPanel();

    }

    public void OpenGameoverPanel()
    {
        gameoverPanel.SetActive(true);
    }

    public void OpenSelectPanel()
    {
        levelUpPanel.SetActive(true);
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
