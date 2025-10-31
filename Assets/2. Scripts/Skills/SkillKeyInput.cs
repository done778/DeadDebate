using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillKeyInput : MonoBehaviour
{
    ElectricitySkill electricitySkill;
    FlamethrowerSkill flamethrowerSkill;

    private void Awake()
    {
        electricitySkill = GetComponent<ElectricitySkill>();
        flamethrowerSkill = GetComponent<FlamethrowerSkill>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            electricitySkill.TryUseSkill();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            flamethrowerSkill.TryUseSkill();
        }

    }
}
