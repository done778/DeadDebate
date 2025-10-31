using UnityEngine;

public class CameraRigController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // 플레이어

    [Tooltip("카메라 기준 오프셋 (위, 뒤)")]
    public Vector3 offset = new Vector3(0, 0f, -15f);
    public float followSpeed = 10f; // 따라가는 속도
    public float rotateSpeed = 150f; // 회전 속도

    //y축 제한
    private Vector2 mouseYLimit = new Vector2(-2f, 60f);

    private float mouseX;   // 마우스 좌우
    private float mouseY; // 마우스 상하

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraRigController 없음!");
            return;
        }

        // 초기화
        Vector3 angles = transform.eulerAngles;
        mouseX = angles.y;
        mouseY = angles.x;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        HandleCameraRotation();

        // 회전 적용
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

        // 회전된 방향을 기준으로 카메라 위치 계산 (항상 target을 중심으로 회전)
        Vector3 offsetPosition = rotation * new Vector3(0, offset.y, offset.z);
        Vector3 desiredPosition = target.position + offsetPosition;

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);

        // 항상 플레이어를 바라보도록 설정 (중심 고정)
        transform.LookAt(target.position + Vector3.up * 2f); // 약간 위를 바라보게
    }

    private void HandleCameraRotation()
    {
        mouseX += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, mouseYLimit.x, mouseYLimit.y);
    }


    public Quaternion GetCameraRotation()
    {
        // 플레이어 이동 방향 계산용 Y축 회전만 반환
        return Quaternion.Euler(0, mouseX, 0);
    }
}
