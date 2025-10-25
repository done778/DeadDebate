using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int createCount;
    [SerializeField] private int radius;
    public List<GameObject> enemys = new List<GameObject>();
    Coroutine enemyCoroutine;

    private void Start()
    {
        enemyCoroutine = StartCoroutine(CreateEnemyCycle(5f));
    }

    private void CreateEnemy(ENEMY type){
        for (int i = 0; i < createCount; i++)
        {
            Vector3 rndPos = Random.insideUnitCircle * radius;
            rndPos = GameManager.Instance.player.transform.position + new Vector3(rndPos.x+5, 0, rndPos.y+5);
            GameObject enemy = Instantiate(enemys[(int)type], rndPos, Quaternion.identity);
        }
    }

    IEnumerator CreateEnemyCycle(float cycle)
    {
        while (true)
        {
            CreateEnemy(ENEMY.Basic);

            yield return new WaitForSeconds(cycle);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GameManager.Instance.player.transform.position, radius);
    }
}
