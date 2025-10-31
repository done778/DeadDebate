using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region field
    public EnemyData data;
    public GameObject bullet;
    public Transform muzzle;

    private Animator anim;
    //인스펙터에서 직접 참조
    [SerializeField] private Collider col;

    //실제 수치들
    private EnemyData currentStat;
    public EnemyData CurrentStat => currentStat;

    private bool isBoss = false;
    private bool isAttack = true;
    private bool isDie = false;
    private float attackTime;
    public bool IsDie
    {
        get { return isDie; }
        private set
        {
            isDie = value;
        }
    }

    private WaitForSeconds attackDelay;
    private Coroutine attackCoroutine;

    private IAttackBehaviour attackBehaviour;
    private PlayerController target;

    public event Action OnDeath;
    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();

        currentStat = data.GetCopy();
    }
    private void OnEnable()
    {
        isDie = false;
        isAttack = true;
        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDie) return;

        if (other.gameObject.CompareTag("Bullet"))
        {
            int damage = GameManager.Instance.CurPlayer.attackPower;
            TakeDamage(damage);
            GameManager.Instance.IndicateDamage(transform.position, damage);
            ObjectManager.Instance.ReturnBullet(other.gameObject);

            //총알 피격 이펙트
            //Vector3 hitPoint = other.ClosestPoint(transform.position);
            //ParticleManager.Instance.PlayEffect("EnemyHitEffect", hitPoint);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.Playing && !isDie)
        {
            if (isAttack)
            {
                transform.LookAt(target.transform);
            }

            if (!IsTagetInRange())
            {

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.transform.position,
                    currentStat.moveSpeed * Time.deltaTime
                    );
            }

            if (IsTagetInRange())
            {
                Attack();
            }

        }

    }

    #region method
    public void Init(GameObject player, int changeValue = 1, bool isBoss = false)
    {

        currentStat = data.GetCopy();
        if (changeValue != 1)
        {
            currentStat.healthPoint = Mathf.RoundToInt(data.healthPoint * changeValue);
            currentStat.attakPower = Mathf.RoundToInt(data.attakPower * changeValue);
            currentStat.moveSpeed *= changeValue;
            currentStat.attackSpeed *= changeValue;
            currentStat.exp = Mathf.RoundToInt(data.exp * changeValue);
        }

        target = player.GetComponent<PlayerController>();
        AddAttakType();


        attackDelay = new WaitForSeconds(currentStat.attackSpeed);
        this.isBoss = isBoss;
    }

    private void TakeDamage(int dmg)
    {
        if (isDie) return;

        currentStat.healthPoint -= dmg;

        if (currentStat.healthPoint <= 0)
        {
            Die();
        }
    }

    private void AddAttakType()
    {
        if (GetComponent<IAttackBehaviour>() == null)
        {
            switch (currentStat.attackType)
            {
                case ATTAK_TYPE.None:
                    break;
                case ATTAK_TYPE.Melee:
                    transform.AddComponent<MeleeAttack>();
                    break;
                case ATTAK_TYPE.Ranged:
                    transform.AddComponent<RangedAttack>();
                    break;
                case ATTAK_TYPE.Hybrid:
                    break;
                default:
                    break;
            }
        }
        attackBehaviour = GetComponent<IAttackBehaviour>();
    }

    #region Attack Type
    private void Attack()
    {
        if (!isAttack) return;

        isAttack = false;
        anim.SetTrigger("Attack");

        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        //todo : EnemyData에 공격시점 추가할지말지
        yield return new WaitForSeconds(0.3f);
        attackBehaviour.Execute(transform, target, currentStat.attakPower, bullet, muzzle);
        //yield return attackDelay;
        yield return new WaitForSeconds(currentStat.attackSpeed - 0.3f);


        isAttack = true;
    }

    IEnumerator RetrunDelay(float timer)
    {
        yield return new WaitForSeconds(timer);

        OnDeath = null;
        attackCoroutine = null;

        EnemyPool.Instance.ReturnEnemy(gameObject);
    }

    private bool IsTagetInRange()
    {
        if (target == null) return false;

        float distance = (target.transform.position - transform.position).sqrMagnitude;

        return distance <= currentStat.range * currentStat.range;
    }
    #endregion


    private void Die()
    {
        isDie = true;

        OnDeath?.Invoke();
        anim.SetTrigger("Die");
        col.enabled = false;

        target.GetExp(currentStat.exp);
        StartCoroutine(RetrunDelay(4f));
    }
    #endregion
}
