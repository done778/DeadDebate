using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    const int MUZZLE_INDEX = 1;
    public float moveSpeed;
    public float attackCoolTime;
    public GameObject bullet;
    private Renderer rend;
    private Vector3 muzzle;
    private float elapsedCoolTime;
    private int experience;
    private int expRequired;
    private int level;
    private int hp;
    private bool isInvincibilityl;


    private void Start()
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();
        GameManager.Instance.GameStart();

        level = 1;
        hp = 3;
        expRequired = 3;
        experience = 0;
        elapsedCoolTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
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

    private void Die()
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
    IEnumerator TakeDamage()
    {
        isInvincibilityl = true;
        yield return new WaitForSeconds(0.2f);
        rend.material.color = Color.yellow;
        yield return new WaitForSeconds(0.3f);
        isInvincibilityl = false;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincibilityl) return;

        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
        rend.material.color = Color.blue;
        StartCoroutine(TakeDamage());
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
        Debug.Log($"������ {level - 1}���� {level}�� �Ǿ����ϴ�.");
    }

    private void ShootBullet()
    {
        muzzle = transform.GetChild(MUZZLE_INDEX).GetComponent<Transform>().position;
        Instantiate(bullet, muzzle, transform.rotation);
    }
}
