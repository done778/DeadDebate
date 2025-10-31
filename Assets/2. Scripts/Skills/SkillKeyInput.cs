using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillKeyInput : MonoBehaviour
{
    [SerializeField] private SkillBase skill_R;
    [SerializeField] private SkillBase skill_T;
    private float coolTime_R;
    private float elapsedTime_R;
    private float coolTime_T;
    private float elapsedTime_T;
    public event Action skillCoolTimeUI;
    //ElectricitySkill electricitySkill;
    //FlamethrowerSkill flamethrowerSkill;

    private void Awake()
    {
        coolTime_R = skill_R.cooldown;
        elapsedTime_R = 0;
        coolTime_T = skill_T.cooldown;
        elapsedTime_T = 0;
        skill_R.player = transform;
        skill_T.player = transform;
        //electricitySkill = GetComponent<ElectricitySkill>();
        //flamethrowerSkill = GetComponent<FlamethrowerSkill>();
    }

    void Update()
    {
        elapsedTime_R -= Time.deltaTime;
        elapsedTime_T -= Time.deltaTime;
        skillCoolTimeUI?.Invoke();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (elapsedTime_R < 0)
            {
                skill_R.TryUseSkill();
                elapsedTime_R = skill_R.cooldown;
            }
            //electricitySkill.TryUseSkill();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (elapsedTime_T < 0)
            {
                skill_T.TryUseSkill();
                elapsedTime_T = skill_T.cooldown;
            }
            //flamethrowerSkill.TryUseSkill();
        }
    }

    public float CoolTimeRate_R()
    {
        return elapsedTime_R / coolTime_R;
    }
    public float CoolTimeRate_T()
    {
        return elapsedTime_T / coolTime_T;
    }
}
