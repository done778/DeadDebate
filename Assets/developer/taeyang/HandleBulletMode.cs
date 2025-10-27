using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBulletMode : MonoBehaviour
{
    private TYBulletController bulletController;
    [SerializeField] private float maxRayDistance = 100f; // 마우스 감지 거리
    [SerializeField] private LayerMask aimLayerMask; // 포인터가 인식할 레이어 (맵)

    private void Start()
    {
        bulletController = GetComponent<TYBulletController>();

        ShotBulletDirection();
    }

    //마우스 위치에 쏘는 함수
    private void ShotBulletDirection()
    {
        Camera mainCam = Camera.main;

        if (mainCam == null)
        {
            Debug.Log("메인카메라 없음");
            return;
        }

        //마우스 위치에 Ray발사
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, aimLayerMask))
        {
            //마우스가 가리키는 방향
            Vector3 direction = hit.point - transform.position;

            //땅 관통방지, XZ축으로만 일직선으로 쏘게
            direction.y = 0f;

            direction.Normalize();

            transform.forward = direction;
        }
        else
        {
            //포인터 없으면 원래대로 앞으로 발사
        }
    }
}
