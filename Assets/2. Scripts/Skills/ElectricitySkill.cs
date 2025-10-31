using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricitySkill : SkillBase
{
    public int damage = 20;
    public float radius = 4f;
    public Vector3 offset;
    public float effectLifeTime = 2f;
    private GameObject effectPrefab;
    protected override void UseSkill()
    {

        Vector3 center = player.position + offset;
        var hits = Physics.OverlapSphere(center, radius, ~0, QueryTriggerInteraction.Collide);

        effectPrefab = GameManager.Instance?.GetPrefab("Electricity");
        if (effectPrefab != null)
        {
            var fx = Instantiate(effectPrefab, center, Quaternion.identity);
            Destroy(fx, effectLifeTime); // 하드코딩 제거
        }

        foreach (var hit in hits)
        {
            if (hit.gameObject == player.gameObject) continue;
            Debug.Log(hit.name);
            var enemy = hit.GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

            }

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

        Vector3 center = refTransform.position + offset;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, radius);
    }

}
