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
        Debug.Log($"쿨타임 : {skillbutton.skillPrefab}");
        _animator = GetComponent<Animator>();
        skillObj = skillbutton.skillPrefab.GetComponent<SkillBase>();
        Debug.Log($"쿨타임 : {skillObj.skillCooldown}");
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
        Color imageColor = button.GetComponentInChildren<Image>().color;
        imageColor.a = 0.5f;
        button.GetComponentInChildren<Image>().color = imageColor;
    }

    public void EndCoolDown()
    {
        isActivate = true;
        button.GetComponent<Button>().interactable = true;
        Color imageColor = button.GetComponentInChildren<Image>().color;
        imageColor.a = 1f;

        button.GetComponentInChildren<Image>().color = imageColor;

        if (button.GetComponent<SkillButton>().skillCount <= 0)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void ResetTimer()
    {
        if (_animator != null)
        {
            _animator.Play("CoolTimeAnim", -1, 1f);
            _animator.speed = 1f;
        }

        button.GetComponent <Button>().interactable = true;

        Color imageColor = button .GetComponentInChildren<Image>().color;
        imageColor.a = 1f;
        button.GetComponentInChildren<Image>().color = imageColor;

        isActivate = true;
    }
}
