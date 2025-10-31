using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDownController : MonoBehaviour
{
    [SerializeField] private Image[] coolDownImages;

    SkillKeyInput skill;

    void Start()
    {
        skill = GameObject.FindWithTag("Player").GetComponent<SkillKeyInput>();
        skill.skillCoolTimeUI += CoolTimeUIUpdate;
    }

    private void OnDestroy()
    {
        skill.skillCoolTimeUI -= CoolTimeUIUpdate;
    }

    void CoolTimeUIUpdate()
    {
        coolDownImages[0].fillAmount = skill.CoolTimeRate_R();
        coolDownImages[1].fillAmount = skill.CoolTimeRate_T();
    }
}
