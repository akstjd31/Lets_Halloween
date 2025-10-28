using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCoolDown : MonoBehaviour
{
    SkillBase skillObj;
    public Animator _animator;
    bool isActivate = true;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        skillObj = transform.parent.GetComponent<SkillBase>();
    }

    public void UseSkill()
    {
        if (isActivate == true)
        {
            _animator.SetFloat("Cooldown", skillObj._cooldown / 100f);
            Debug.Log(_animator.GetFloat("Cooldown"));
            _animator.SetTrigger("UseSkill");
        }
    }

    public void StartCoolDown()
    {
        isActivate = false;
    }

    public void EndCoolDown()
    {
        isActivate = true;
    }
}
