using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class PlayerStatButton : MonoBehaviour
{
    public event Action<string> OnButtonClicked;
    private string buttonType;
    private Button button;


    public void StatsButtonClick(int index)
    {
        button = transform.GetChild(index).GetComponent<Button>();
        buttonType = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        OnButtonClicked?.Invoke(buttonType); 
        Debug.Log($"{buttonType}을 매개변수로 이벤트 발생함.");
    }
}

