using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    const int MUZZLE_INDEX = 1;
    public float moveSpeed;
    public float attackCoolTime;
    public GameObject bullet;
    private float elapsedCoolTime;
    private Vector3 muzzle;
    private int experience;
    private int expRequired;
    private int level;

    private void Awake()
    {
        GameManager.Instance.GameStart();
    }

    private void Start()
    {
        level = 1;
        expRequired = 3;
        experience = 0;
        elapsedCoolTime = 0;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Vector3 inputDirection = GetNormalizedDirection();
        Move(inputDirection);

        elapsedCoolTime -= Time.deltaTime;
        if (elapsedCoolTime < 0) 
        {
            ShootBullet();
            elapsedCoolTime = attackCoolTime;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameOver();
    }
    private Vector3 GetNormalizedDirection()
    {
        Vector3 inputDirection = Vector3.zero;
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.z = Input.GetAxisRaw("Vertical");

        return inputDirection.normalized;
    }

    private void Move(Vector3 Direction)
    {
        if (Direction == Vector3.zero) return;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(Direction),
            0.02f
        );
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

    }
    public void GetExp(int amount)
    {
        experience++;
        if ( experience >= expRequired)
        {
            LevelUp();
            experience = 0;
        }
    }

    private void LevelUp()
    {
        level++;
        Debug.Log($"레벨이 {level - 1}에서 {level}이 되었습니다.");
    }

    private void ShootBullet()
    {
        muzzle = transform.GetChild(MUZZLE_INDEX).GetComponent<Transform>().position;
        Instantiate(bullet, muzzle, transform.rotation);
    }
}
