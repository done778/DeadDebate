using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour, IMovement, IAttack
{
    public EnemyData data;
    private Rigidbody rb;
    private float rotateSpeed = 10f;
    private float range = 1.5f;
    private float radius = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        //Initialize();
    }


    private void Update()
    {
        Move(GameManager.Instance.player.transform.position);
        LookRotate(GameManager.Instance.player.transform.position);

        float dis = Vector3.Distance(GameManager.Instance.player.transform.position, transform.position);
        if (dis <= range)
        {
            Attack();
        }
    }

    public void Attack()
    {
        GameManager.Instance.player.GetComponent<PlayerController>()?.TakeDamage(data.attakPower);
    }

    public void LookRotate(Vector3 dir)
    {
        Vector3 direction = dir - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    public void Move(Vector3 dir)
    {
        transform.position = Vector3.MoveTowards(transform.position, dir, data.moveSpeed * Time.deltaTime);
    }

}
