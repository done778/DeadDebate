using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public float radius;

    private Vector3 createdPos;
    private Vector3 spawnPos;
    private GameObject curEnemy;

    WaitForSeconds spawnDelay = new WaitForSeconds(1f);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (GameManager.Instance.Playing)
        {
            createdPos = Random.insideUnitCircle * radius;
            spawnPos.x = createdPos.x + 10;
            spawnPos.y = 0;
            spawnPos.z = createdPos.y + 10;
            createdPos = GameManager.Instance.player.transform.position + spawnPos;
            curEnemy = Instantiate(Enemy, createdPos, Quaternion.identity);
            curEnemy.GetComponent<EnemyController>().Init(GameManager.Instance.player);

            yield return spawnDelay;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GameManager.Instance.player.transform.position, radius + 10);
    }
}
