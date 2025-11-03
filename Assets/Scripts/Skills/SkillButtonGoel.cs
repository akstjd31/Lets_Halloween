using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class SkillButtonGoel : SkillButton
{
    [SerializeField] private TextMeshProUGUI countText;

    private MyEnemyUnit myEnemyUnit;

    public override void OnClickButton()
    {
        if (GameManager.Instance.GetGameState().Equals(GameState.Prepare))
            return;

        // 스킬 개수가 0이면 사용 불가
        if (skillCount <= 0)
        {
            button.interactable = false;
            return;
        }

        myEnemyUnit = GameObject.Find("PlayerEnemy(Clone)").GetComponent<MyEnemyUnit>();
        var skillCoolDownAnim = transform.GetComponentInChildren<SkillCoolDown>();
        MouseTrackingManager.Instance.activeSkill = true;
        myEnemyUnit.SpawnGoal();
        MouseTrackingManager.Instance.SetAnim(skillCoolDownAnim);

    }
}
