using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] Transform Hp;
    [SerializeField] Camera cam;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        Quaternion Q_hp = Quaternion.LookRotation(Hp.position - cam.transform.position);

        Vector3 hpAngle = Quaternion.RotateTowards(Hp.rotation, Q_hp, 200).eulerAngles;
        Hp.rotation = Quaternion.Euler(0, hpAngle.y, 0);
    }
}
