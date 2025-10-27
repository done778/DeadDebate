using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region field
    public EnemyData data;

    //실제 수치들
    private EnemyData currentStat;
    public EnemyData CurrentStat => currentStat;

    private GameObject target;
    #endregion

    private void Awake()
    {
        currentStat = data.GetCopy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            int damage = other.gameObject.GetComponent<BulletContoller>().power;
            TakeDamage(damage);
            Destroy(other.gameObject);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.Playing)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                currentStat.moveSpeed * Time.deltaTime
                );
            transform.LookAt(target.transform);
        }
    }

    #region method
    public void Init(GameObject player)
    {
        target = player;
    }

    public void Init(GameObject player, int changeValue)
    {
        target = player;
        currentStat.healthPoint = Mathf.RoundToInt(data.healthPoint * changeValue);
        currentStat.attakPower = Mathf.RoundToInt(data.attakPower * changeValue);
        currentStat.moveSpeed *= changeValue;
        currentStat.attackSpeed *= changeValue;
        currentStat.exp = Mathf.RoundToInt(data.exp * changeValue);
    }

    private void TakeDamage(int dmg)
    {
        currentStat.healthPoint -= dmg;
        if (currentStat.healthPoint <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        target.GetComponent<PlayerContoller>().GetExp(currentStat.exp);
        Destroy(gameObject);
    }
    #endregion
}
