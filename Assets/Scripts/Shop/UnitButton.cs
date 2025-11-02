using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : ParentButton
{
    [SerializeField] public GameObject Preview;
   
    override public void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClickButton);

        Debug.Log(button.onClick);
    }

    public override void OnClickButton()
    {

        // var skillCoolDownAnim = transform.GetComponentInChildren<SkillCoolDown>();
        // MouseTrackingManager.Instance.targetObject = Preview;
        // MouseTrackingManager.Instance.SpawnTargetSkill(skillPrefab);
        // MouseTrackingManager.Instance.SetAnim(skillCoolDownAnim);
    }
}
