using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonTank : SkillButton
{
    [SerializeField] private PlayerUnit_Projectile playerUnitObj;

    public override void OnClickButton()
    {
        if (skillCount <= 0)
        {
            
            return;
        }

        MouseTrackingManager.Instance.SetActiveButton(this);

        var skillCoolDownAnim = transform.GetComponentInChildren<SkillCoolDown>();
        MouseTrackingManager.Instance.targetObject = PreviewObj;
        MouseTrackingManager.Instance.SpawnTargetUnitP(playerUnitObj);
        MouseTrackingManager.Instance.SetAnim(skillCoolDownAnim);
    }
}
