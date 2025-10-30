using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    public void OnBackButtonClick()
    {
        UIManager.UIInstance.LoadScene("Title");
    }

    public void OnStageButtonClick(int index)
    {
        // Stage 추가되면 여기도 변경
        switch(index)
        {
            case 0:
                UIManager.UIInstance.LoadScene("Stage");
                break;
            case 1:
                UIManager.UIInstance.LoadScene("Stage");
                break;
            case 2:
                UIManager.UIInstance.LoadScene("Stage");
                break;
            case 3:
                UIManager.UIInstance.LoadScene("Stage");
                break;
            default:
                Debug.Log("범위를 벗어난 값이 들어옴.");
                UIManager.UIInstance.LoadScene("Title");
                break;
        }
    }

    public void OnSelectButtonClick()
    {
        UIManager.UIInstance.SetCharSelectPanel(true);
    }
}
