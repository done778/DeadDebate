using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject playGround;
    public GameObject player;
    public GameObject Enemy;

    private Vector3 minPlayBounds;
    private Vector3 maxPlayBounds;
    private Vector3 createdPos;
    private GameObject curEnemy;

    WaitForSeconds spawnDelay = new WaitForSeconds(1f);

    // Start is called before the first frame update
    void Start()
    {
        Bounds groundBounds = playGround.GetComponent<MeshRenderer>().bounds;
        minPlayBounds = groundBounds.center - groundBounds.extents;
        maxPlayBounds = groundBounds.center + groundBounds.extents;
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while(GameManager.Instance.Playing)
        {
            createdPos.x = Random.Range(minPlayBounds.x, maxPlayBounds.x);
            createdPos.z = Random.Range(minPlayBounds.z, maxPlayBounds.z);
            curEnemy = Instantiate(Enemy, createdPos, transform.rotation);
            curEnemy.GetComponent<EnemyController>().Init(player);

            yield return spawnDelay;
        }
    }
}
