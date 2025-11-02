using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{

    [SerializeField] private SkillButton skillbutton;
    SkillBase skillObj;
    public Animator _animator;
    bool isActivate = true;
    GameObject button;

    private void Awake()
    {
        skillbutton = GetComponentInParent<SkillButton>();
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log($"ÄðÅ¸ÀÓ : {skillbutton.skillPrefab}");
        _animator = GetComponent<Animator>();
        skillObj = skillbutton.skillPrefab.GetComponent<SkillBase>();
        Debug.Log($"ÄðÅ¸ÀÓ : {skillObj.skillCooldown}");
        button = transform.parent.gameObject;
        
    }

    public void UseSkill()
    {
        if (isActivate == true)
        {
            _animator.speed = 1f / skillObj.skillCooldown;
            _animator.Play("CoolTimeAnim", -1, 0f);
        }
    }

    public void StartCoolDown()
    {
        isActivate = false;
        button.GetComponent<Button>().interactable = false;
    }

    public void EndCoolDown()
    {
        isActivate = true;
        button.GetComponent<Button>().interactable = true;
    }
}
