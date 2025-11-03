using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 10f; // 이동속도
    public float attackCoolTime = 0.3f; // 공격속도
    public int attackPower = 1; // 공격력
    [SerializeField] private int maxHp = 5; // 최대 체력
    private int currentHp = 0; // 현재 체력
    [SerializeField] private float attackRange = 20f; // 공격사거리
    public float AttackRange => attackRange;


    //const int MUZZLE_INDEX = 1;
    //private Vector3 muzzle;
    [SerializeField] private Transform muzzle; // 총구

    public GameObject bullet;
    private Renderer rend;
    private Color originalColor; // 기본색깔

    private float elapsedCoolTime; // 쿨타임 경과 시간

    private int experience; // 경험치
    private int expRequired; // 필요 경험치
    private int level; // 레벨    
    private float baseAttackCoolTime; // 기본 공격 속도
    private float increaseAttackSpeed; // 증가된 공격 속도

    private bool isInvincibilityl; // 무적

    private AttackModeController attackModeController; // 공격모드
    //private PlayerAnimatorController playerAnimatorController; // 모션

    private Vector3 inputDirection;
    public Vector3 InputDirection{ get{ return inputDirection; } }

    //이벤트
    public event Action<int> OnLevelUp;
    public event Action<int, int> OnHpChanged;
    public event Action OnPlayerDie;

    //이동 가능 영역
    private Vector3 minWorldBounds;
    private Vector3 maxWorldBounds;
    Vector3 limitPos;

    private void Start()
    {
        attackModeController = GetComponent<AttackModeController>(); // 공격모드        
        //playerAnimatorController = GetComponent<PlayerAnimatorController>(); // 모션
        rend = GetComponentInChildren<Renderer>();

        // 플레이어 이동 가능 영역 불러오기
        GameObject plane = GameObject.Find("PlayableArea");
        if (plane != null)
        {
            Bounds movableArea = plane.GetComponent<MeshRenderer>().bounds;
            minWorldBounds = movableArea.center - movableArea.extents;
            maxWorldBounds = movableArea.center + movableArea.extents;
        }

        if (rend != null)
        {
            originalColor = rend.material.color; // 기본색깔
        }    

        level = 1;
        expRequired = 3;
        experience = 0;
        elapsedCoolTime = 0;
        baseAttackCoolTime = attackCoolTime;
        increaseAttackSpeed = 0f;

        currentHp = maxHp;
        //체력이 변할때마다 인보크해야
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    private void OnTriggerEnter(Collider other)
    {
        //적 몸박시
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }

        //적 총알과 충돌시
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(1);
            ObjectManager.Instance.ReturnBullet(other.gameObject);
        }
    }

    void Update()
    {
        inputDirection = GetNormalizedDirection();
        Move(inputDirection);

        //playerAnimatorController.UpdateMovementAnimation(inputDirection);

        elapsedCoolTime -= Time.deltaTime;
        if (elapsedCoolTime < 0)
        {
            attackModeController.GetCurrentMode().Attack(this); // 공격방식
            //playerAnimatorController.PlayRunAttack(); // 공격모션
            elapsedCoolTime = attackCoolTime;
        }
    }

    private void Die()
    {
        OnPlayerDie?.Invoke(); // 플레이어 사망시
        //playerAnimatorController.PlayDeath(); // 죽는모션
    }

    private Vector3 GetNormalizedDirection()
    {
        Vector3 inputDirection = Vector3.zero;
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.z = Input.GetAxisRaw("Vertical");

        return inputDirection.normalized;
    }

    IEnumerator TakeDamageColorEffect()
    {
        isInvincibilityl = true;
        if (rend != null)
        {
            rend.material.color = Color.blue;
        }
        yield return new WaitForSeconds(0.3f);
        if (rend != null)
        {
            rend.material.color = originalColor;
        }
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
        
        StartCoroutine(TakeDamageColorEffect());
        Debug.Log($"현재체력: {currentHp}");
    }

    private void Move(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Vector3 move = direction * Time.deltaTime * moveSpeed;
        transform.Translate(move, Space.World);
        limitPos = transform.position;
        limitPos.x = Mathf.Clamp(transform.position.x, minWorldBounds.x, maxWorldBounds.x);
        limitPos.z = Mathf.Clamp(transform.position.z, minWorldBounds.z, maxWorldBounds.z);
        transform.position = limitPos;
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

        expRequired = Mathf.FloorToInt(3 * Mathf.Pow(level, 1.5f));

        OnLevelUp?.Invoke(level); // 레벨업시
    }

    //스탯 증가
    public void IncreaseStat(string statType)
    {
        switch (statType)
        {
            case "AttackPower":
                attackPower += 1;
                Debug.Log($"공격력 증가!! 현재 공격력: {attackPower}");
                break;
            case "AttackSpeed":
                increaseAttackSpeed += 0.1f;
                attackCoolTime = baseAttackCoolTime / (1 + increaseAttackSpeed);
                Debug.Log($"공격속도 증가!! 현재 공격속도: {(1 + increaseAttackSpeed) * 100}%");
                break;
            case "HealthPoint":
                maxHp += 1;
                currentHp += 1;
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

    public void ShootBullet()
    {
        if (muzzle == null)
        {
            Debug.LogWarning("Muzzle이 할당되지 않았습니다!");
            return;
        }

        GameObject bullet = ObjectManager.Instance.GetPlayerBullet();
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;
        //BulletPool.Instance.GetBullet(muzzle.position, transform.rotation);
    }
}
