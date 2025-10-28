using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager UIInstance { get; private set; }
    [SerializeField] private GameObject gameoverPanel;
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
    }
   
    public void LoadScene(string sceneName)
    {
        if (isLoading) return;
        isLoading = true;
        SceneManager.LoadScene(sceneName);
        isLoading = false;
    }
    
    public void OpenGameoverPanel()
    {
        gameoverPanel.SetActive(true);
    }
}
