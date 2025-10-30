using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public float cooldown = 2f;
    protected float lastUseTime;
    protected Transform player;

    protected virtual void Awake()
    {
        player = transform;
    }

    public void TryUseSkill()
    {
        if (Time.time - lastUseTime < cooldown)
        {
            return;
        }
        lastUseTime = Time.time;
        UseSkill();

    }

    protected abstract void UseSkill();

}
