using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Camera cm;
    public bool Playing { get; private set; }
    public GameObject player;
    public readonly float surviveTime = 600f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        cm = Camera.main;
    }
    void Update()
    {
        if (Playing == true)
            cm.transform.position = new Vector3(player.transform.position.x, cm.transform.position.y, player.transform.position.z - 18);
    }

    public void GameStart()
    {
        Playing = true;
    }
    public void GameOver()
    {
        Playing = false;
    }
}
