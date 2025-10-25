using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timer;
    float surviveTimer = 0f;
    private void Start()
    {
        surviveTimer = GameManager.Instance.surviveTime;
    }

    // Update is called once per frame
    void Update()
    {
        float t = surviveTimer - TimeManager.Instance.Timer;
        //Debug.Log($"{t} : {t/60:0}");
        timer.text = $"{(int)t / 60} : {t % 60:0}";
    }
}
