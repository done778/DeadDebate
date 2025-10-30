using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const int MUZZLE_INDEX = 1;

    //플레이어 스탯
    public float moveSpeed = 10f; // 이동속도
    public float attackCoolTime = 0.3f; // 공격속도
    public float attackPower = 1f; // 공격력
    [SerializeField] private int maxHp = 5; // 최대 체력
    private int currentHp = 0; // 현재 체력
    [SerializeField] private float attackRange = 10f; // 공격사거리
    public float AttackRange => attackRange;


    public GameObject bullet;
    private Renderer rend;
    private Vector3 muzzle;
    private float elapsedCoolTime; // 쿨타임 경과 시간

    private int experience; // 경험치
    private int expRequired; // 필요 경험치
    private int level; // 레벨    

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
        Debug.Log($"현재체력: {currentHp}");
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

    //스탯 증가
    public void IncreaseStat(string statType)
    {
        switch (statType)
        {
            case "AttackPower":
                attackPower += 1f;
                Debug.Log($"공격력 증가!! 현재 공격력: {attackPower}");
                break;
            case "AttackSpeed":
                attackCoolTime = Mathf.Max(0.1f, attackCoolTime - 0.1f);
                Debug.Log($"공격속도 증가!! 현재 공격속도: {attackCoolTime}");
                break;
            case "HealthPoint":
                maxHp += 1;
                OnHpChanged?.Invoke(currentHp, maxHp);
                Debug.Log($"최대체력 증가!! 현재 최대체력: {maxHp}");
                break;
            case "MoveSpeed":
                moveSpeed += 0.5f;
                Debug.Log($"이동속도 증가!! 현재 이동속도: {moveSpeed}");
                break;

            default:
                Debug.Log($"알 수 없는 스탯: {statType}");
                break;
        }
    }
    
    public void ShootBullet(Vector3 targetPosition)
    {
        //이게 총구니까
        muzzle = transform.GetChild(MUZZLE_INDEX).position;
        //총구와 적방향을 계산하고
        Vector3 direction = (targetPosition - muzzle).normalized;
        //총구를 회전시키다면
        Quaternion bulletRotation = Quaternion.LookRotation(direction);

        GameObject bulletObject = BulletPool.Instance.GetBullet(muzzle, bulletRotation);
    }
}
