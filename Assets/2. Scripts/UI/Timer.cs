using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    public float limitTime;
    public TextMeshProUGUI textTimer;

    private void Update()
    {
        limitTime -= Time.deltaTime;
        if (limitTime < 0) limitTime = 0;
      
        
        int minute = Mathf.FloorToInt(limitTime / 60);
        int second = Mathf.FloorToInt(limitTime % 60);
        textTimer.text = $"Time : {minute:00}:{second:00}";
    }
}
