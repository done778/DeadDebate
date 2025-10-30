using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSkill : SkillBase
{
    public float damage = 30f;
    public float range = 15f;
    public Vector3 offset = new Vector3(0, 0.5f, 0);
    public LayerMask hitMask = ~0;

    protected override void UseSkill()
    {
        Vector3 origin = player.position + offset;
        Vector3 forwar = player.forward;

        if (Physics.Raycast(origin, forwar, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Collide))
        {
            if (hit.transform.root == player) return;

            Debug.Log($"{hit.collider.name}");
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
