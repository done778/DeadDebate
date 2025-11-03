using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public float cooldown = 2f;
    [HideInInspector] public Transform player;

    public void TryUseSkill()
    {
        UseSkill();
    }
    protected abstract void UseSkill();
}
