using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Camera mainCamera;
    public GameObject player;
    public readonly float surviveTime = 180f;
    private float playingTime;
    private Vector3 cameraPos;

    public bool Playing { get; private set; }

    // 싱글톤
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
        // 카메라 초기화
        mainCamera = Camera.main;
        mainCamera.transform.rotation = Quaternion.Euler(80f, 0f, 0f);
    }

    void Update()
    {
        if (Playing == true)
        {
            //카메라가 플레이어를 따라 가도록 함. 20과 -4는 플레이어와의 거리 상수
            cameraPos.x = player.transform.position.x;
            cameraPos.y = player.transform.position.y + 20;
            cameraPos.z = player.transform.position.z - 4;
            mainCamera.transform.position = cameraPos;

            // 생존 시간 측정. UI에 업데이트
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

    public void GameOver(bool isClear)
    {
        Playing = false;
        Time.timeScale = 0f;
        if (isClear)
        {
            StageClear();
        }
        else
        {
            StageFailed();
        }
    }

    // 스테이지 도전 성공
    // 승리 관련 UI 띄우고 로비로 돌아가기
    public void StageClear()
    {

    }

    // 스테이지 도전 실패
    // 패배 관련 UI 띄우고 로비로 돌아가기
    public void StageFailed()
    {

    }
}
