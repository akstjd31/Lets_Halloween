using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
    SkillBase skillObj;
    public Animator _animator;
    bool isActivate = true;
    GameObject button;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        skillObj = transform.parent.GetComponent<SkillBase>();
        button = transform.parent.GetChild(2).gameObject;
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
        button.SetActive(false);
    }

    public void EndCoolDown()
    {
        isActivate = true;
        button.SetActive(true);
    }
}
