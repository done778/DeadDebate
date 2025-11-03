using UnityEngine;


public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f; // 총알속도
    [SerializeField] private float bulletLifeTime = 2f; // 총알유지시간

    private float bulletTimer;

    private void OnEnable()
    {
        bulletTimer = 0f;
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * bulletSpeed * Vector3.forward);

        bulletTimer += Time.deltaTime;
        if (bulletTimer >= bulletLifeTime)
        {
            ObjectManager.Instance.ReturnBullet(gameObject);
        }
    }
}
