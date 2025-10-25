using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimeManager>();
            }
            return instance;
        }
    }
    public float Timer { get; set; }
    public bool IsStop { get; set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsStop)
        {
            Timer += Time.deltaTime;
        }
    }

    public void Run() => IsStop = false;
    public void Stop() => IsStop = true;

    public void ReStart()
    {
        IsStop = false;
        Timer = 0f;
    }
}
