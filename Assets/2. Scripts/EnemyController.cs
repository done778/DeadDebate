using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public int maxHp;
    GameObject target;
    private int curHp;
    private int exp;
    // Start is called before the first frame update
    void Start()
    {
        exp = 1;
        curHp = maxHp;
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
                moveSpeed * Time.deltaTime
                );
            transform.LookAt(target.transform);
        }
    }
    public void Init(GameObject player)
    {
        target = player;
    }
    private void TakeDamage(int dmg)
    {
        curHp -= dmg;
        if (curHp <= 0) 
        {
            Die();
        }
    }
    private void Die()
    {
        target.GetComponent<PlayerContoller>().GetExp(exp);
        Destroy(gameObject);
    }
}
