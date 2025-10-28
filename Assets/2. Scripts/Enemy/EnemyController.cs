using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region field
    public EnemyData data;
    public GameObject bullet;
    public Transform muzzle;

    //실제 수치들
    private EnemyData currentStat;
    public EnemyData CurrentStat => currentStat;

    private bool isBoss = false;
    private bool isAttack = true;
    private WaitForSeconds attackDelay;

    private PlayerContoller target;
    #endregion

    private void Awake()
    {
        currentStat = data.GetCopy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            int damage = other.gameObject.GetComponent<BulletController>().damage;
            TakeDamage(damage);
            Destroy(other.gameObject);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.Playing)
        {
            transform.LookAt(target.transform);

            if (!IsTagetInRange() && isAttack)
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
    public void Init(GameObject player, bool isBoss = false)
    {
        target = player.GetComponent<PlayerContoller>();
        attackDelay = new WaitForSeconds(currentStat.attackSpeed);

        currentStat = data.GetCopy();
        this.isBoss = isBoss;
    }

    public void Init(GameObject player, int changeValue, bool isBoss = false)
    {
        target = player.GetComponent<PlayerContoller>();

        currentStat.healthPoint = Mathf.RoundToInt(data.healthPoint * changeValue);
        currentStat.attakPower = Mathf.RoundToInt(data.attakPower * changeValue);
        currentStat.moveSpeed *= changeValue;
        currentStat.attackSpeed *= changeValue;
        currentStat.exp = Mathf.RoundToInt(data.exp * changeValue);
        attackDelay = new WaitForSeconds(currentStat.attackSpeed);

        this.isBoss = isBoss;
    }

    private void TakeDamage(int dmg)
    {
        currentStat.healthPoint -= dmg;
        if (currentStat.healthPoint <= 0)
        {
            Die();
        }
    }


    #region Attack Type
    private void Attack()
    {
        if (!isAttack) return;

        switch (currentStat.attackType)
        {
            case ATTAK_TYPE.None:
                break;
            case ATTAK_TYPE.Melee:
                MeleeAttack();
                break;
            case ATTAK_TYPE.Ranged:
                RangedAttack();
                break;
            case ATTAK_TYPE.Hybrid:
                //HybridAttack();
                break;
            default:
                break;
        }
    }

    private void MeleeAttack()
    {
        StartCoroutine(AttackDelay(true));
    }
    private void RangedAttack()
    {
        SpawnProjectile();

        StartCoroutine(AttackDelay(false));
    }

    private void SpawnProjectile()
    {
        GameObject newBullet = Instantiate(bullet, muzzle);
        newBullet.transform.SetParent(null);
    }

    IEnumerator AttackDelay(bool isMelee)
    {
        isAttack = false;

        if (isMelee)
        {
            target.TakeDamage(currentStat.attakPower);
        }

        yield return attackDelay;
        isAttack = true;
    }

    private bool IsTagetInRange()
    {
        float distance = (target.transform.position - transform.position).sqrMagnitude;

        return distance <= currentStat.range * currentStat.range;
    }
    #endregion


    private void Die()
    {
        target.GetComponent<PlayerContoller>().GetExp(currentStat.exp);
        Destroy(gameObject);
    }
    #endregion
}
