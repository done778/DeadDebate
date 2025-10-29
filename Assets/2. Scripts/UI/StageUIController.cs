using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUIController : MonoBehaviour
{
    public void OnLobbyButtonClick()
    {
        UIManager.UIInstance.LoadScene("Lobby");
    }

    public void OnRetryButtonClick()
    {
        // 재도전은 현재 씬 이름을 가져와 다시 로드함.
        UIManager.UIInstance.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 클리어 시 다음 스테이지로 바로 이동하는 버튼.
    // 아직 여러 스테이지가 구현되지 않아 로비로 가도록 함.
    public void OnNextButtonClick()
    {
        UIManager.UIInstance.LoadScene("Lobby");
    }
}
