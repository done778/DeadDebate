using UnityEngine;

public class FlamethrowerSkill : SkillBase
{
    public int damage = 30;
    public float range = 15f;
    public Vector3 offset = new Vector3(0, 0.5f, 0);
    public LayerMask hitMask = ~0;
    private GameObject effectPrefab;
    public float effectLifeTime = 0.5f;
    protected override void UseSkill()
    {
        Vector3 origin = player.position + offset;
        Vector3 dir = player.forward;

        effectPrefab = GameManager.Instance.GetPrefab("Flamethrower");
        if (effectPrefab != null)
        {
            GameObject fx = Instantiate(effectPrefab, origin, Quaternion.LookRotation(dir));
            Destroy(fx, effectLifeTime);
        }

        if (Physics.Raycast(origin, dir, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Collide))
        {
            if (hit.transform.root == player) return;
            var enemy = hit.collider.GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("miss");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Transform refTransform;
        if (Application.isPlaying)
        {
            refTransform = player;
        }
        else
        {
            refTransform = transform;
        }

        Vector3 origin = refTransform.position + offset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + refTransform.forward * range);
        Gizmos.DrawSphere(origin + refTransform.forward * range, 0.15f);

    }
}
