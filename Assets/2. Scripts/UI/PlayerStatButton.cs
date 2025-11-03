using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatButton : MonoBehaviour
{
    //public event Action<string> OnButtonClicked;
    private string buttonType;
    private Button button;

    public void StatsButtonClick(int index)
    {
        button = transform.GetChild(index).GetComponent<Button>();
        buttonType = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        GameManager.Instance.playerStatIncrease(buttonType);
        GameManager.Instance.PlayGame();
        UIManager.UIInstance.SetSelectPanel(false);
    }
}

