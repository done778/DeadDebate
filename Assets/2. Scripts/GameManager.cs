using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool Playing { get; private set; }
    private Camera mainCamera;
    public GameObject player;
    public readonly float surviveTime = 180f;
    private float playingTime;
    private Vector3 cameraPos;

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
        // ī�޶� �ʱ�ȭ
        mainCamera = Camera.main;
        mainCamera.transform.rotation = Quaternion.Euler(80f, 0f, 0f);
    }

    void Update()
    {
        if (Playing == true)
        {
            //ī�޶� �÷��̾ ���� ������ ��. 20�� -4�� �÷��̾���� �Ÿ� ���
            cameraPos.x = player.transform.position.x;
            cameraPos.y = player.transform.position.y + 20;
            cameraPos.z = player.transform.position.z - 4;
            mainCamera.transform.position = cameraPos;

            // ���� �ð� ����. UI�� ������Ʈ
            playingTime += Time.deltaTime;
            if (playingTime >= surviveTime)
            {
                StageClear();
            }
        }
    }

    public void GameStart()
    {
        playingTime = 0f;
        Playing = true;
    }
    public void GameOver()
    {
        Playing = false;
    public void StageClear()
    {

    }
    }
}
