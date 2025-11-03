using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonTank : SkillButton
{
    [SerializeField] private PlayerUnit_Projectile playerUnitObj;

    private SkillCoolDown _cooldownAnim;

    override public void Start()
    {
        

        _cooldownAnim = transform.GetComponentInChildren<SkillCoolDown>();
        base.Start();
    }
    public override void OnClickButton()
    {
        if (skillCount <= 0)
        {
            button.interactable = false;
            return;
        }

        MouseTrackingManager.Instance.SetActiveButton(this);
        MouseTrackingManager.Instance.targetObject = PreviewObj;
        MouseTrackingManager.Instance.SpawnTargetUnitP(playerUnitObj);
        MouseTrackingManager.Instance.SetAnim(_cooldownAnim);
    }

    public void ResetCooldownAndEnable()
    {
        if (_cooldownAnim != null)
        {
            _cooldownAnim.ResetTimer();
        }

        button.interactable = true;
    }
}
