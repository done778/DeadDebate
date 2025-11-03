using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Camera mainCamera;
    private Vector3 cameraPos;

    void Start()
    {
        // 카메라 초기화
        mainCamera = Camera.main;
        mainCamera.transform.rotation = Quaternion.Euler(80f, 0f, 0f);

        // 플레이어 찾아서 참조
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        if (GameManager.Instance.Playing == true)
        {
            //카메라가 플레이어를 따라 가도록 함. 20과 -4는 플레이어와의 거리 상수
            cameraPos.x = player.transform.position.x;
            cameraPos.y = player.transform.position.y + 20;
            cameraPos.z = player.transform.position.z - 4;
            mainCamera.transform.position = cameraPos;
        }
    }
}
