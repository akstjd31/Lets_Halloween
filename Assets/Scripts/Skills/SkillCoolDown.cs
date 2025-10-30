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

    GameObject button;


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
            _animator.SetFloat("Cooldown", skillObj.skillCooldown / 100f);
            Debug.Log(_animator.GetFloat("Cooldown"));
            _animator.SetTrigger("UseSkill");
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
