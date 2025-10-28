using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatButton : MonoBehaviour
{
   
    public event Action<string> OnButtonClicked;

    [SerializeField] private string buttonType;  

    public void StatsButtonClick()
    {
        OnButtonClicked?.Invoke(buttonType); 
        Debug.Log("클릭 이벤트 발생!");
    }
}

