using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoESkill : SkillBase
{
    public float damage = 20f;
    public float radius = 4f;
    public Vector3 offset;
    protected override void UseSkill()
    {

        Vector3 center = player.position - offset;
        var hits = Physics.OverlapSphere(center, radius, ~0, QueryTriggerInteraction.Collide);

        foreach (var hit in hits)
        {
            if (hit.gameObject == player.gameObject) continue;
            Debug.Log("hitSkill");
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
