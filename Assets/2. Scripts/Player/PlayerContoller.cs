using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    const int MUZZLE_INDEX = 1;
    public float moveSpeed; // 이동속도
    public float attackCoolTime; // 공격속도
    public GameObject bullet;
    private Renderer rend;
    private Vector3 muzzle;
    private float elapsedCoolTime; // 쿨타임 경과 시간

    private int experience; // 경험치
    private int expRequired; // 필요 경험치
    private int level; // 레벨

    private int maxHp = 5; // 최대 체력
    private int currentHp = 0; // 현재 체력

    private bool isInvincibilityl; // 무적

    private AttackModeController attackModeController; // 공격모드

    //이벤트
    public event Action<int> OnLevelUp;
    public event Action<int, int> OnHpChanged;
    public event Action OnPlayerDie;

    private void Start()
    {
        attackModeController = GetComponent<AttackModeController>(); // 공격모드

        rend = transform.GetChild(0).GetComponent<Renderer>();
        GameManager.Instance.GameStart();

        level = 1;
        expRequired = 3;
        experience = 0;
        elapsedCoolTime = 0;
        
        currentHp = maxHp;
        //체력이 변할때마다 인보크해야
        OnHpChanged?.Invoke(currentHp, maxHp);
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
            attackModeController.GetCurrentMode().Attack(this); // 공격방식
            elapsedCoolTime = attackCoolTime;
        }
    }

    private void Die()
    {
        GameManager.Instance.GameOver();
        OnPlayerDie?.Invoke(); // 플레이어 사망시
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

        currentHp -= damage;
        if (currentHp < 0)
            currentHp = 0;
        OnHpChanged?.Invoke(currentHp, maxHp);  // 데미지 입을때 마다

        if (currentHp <= 0)
        {
            Die();
        }
        rend.material.color = Color.blue;
        StartCoroutine(TakeDamage());
    }

    private void Move(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Vector3 move = direction * Time.deltaTime * moveSpeed;
        transform.Translate(move, Space.World);       
    }

    public void GetExp(int amount)
    {
        experience++;
        if (experience >= expRequired)
        {
            LevelUp();
            experience = 0;
        }
    }

    private void LevelUp()
    {
        level++;
        Debug.Log($"레벨이 {level - 1}에서 {level}이 되었습니다.");
        
        OnLevelUp?.Invoke(level); // 레벨업시
    }
    
    public void ShootBullet()
    {
        muzzle = transform.GetChild(MUZZLE_INDEX).GetComponent<Transform>().position;
        Instantiate(bullet, muzzle, transform.rotation);
    }
}
