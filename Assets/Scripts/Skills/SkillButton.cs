using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : ParentButton
{
    [SerializeField] private int skillCount;
    [SerializeField] public SkillBase skillPrefab;
    private TextMeshProUGUI countText;

    private void Start()
    {
        countText = GetComponentInChildren<TextMeshProUGUI>();

        UpdateCountUI();
    }

    private void UpdateCountUI()
    {
        if (countText != null)
            countText.text = skillCount.ToString();

        button.interactable = skillCount > 0;
        Debug.Log(skillCount);
    }

    public override void OnClickButton()
    {
        if (skillCount <= 0)
        {
            return;
        }

        base.OnClickButton(); // 부모에서 호출
        var skillCoolDownAnim = transform.GetComponentInChildren<SkillCoolDown>();
        
        MouseTrackingManager.Instance.SpawnTargetSkill(skillPrefab);
        MouseTrackingManager.Instance.SetAnim(skillCoolDownAnim);
    }

    public void AddCount()
    {
        skillCount++;
        UpdateCountUI();
    }

    public void RemoveCount()
    {
        skillCount = Mathf.Max(0, skillCount - 1);
        UpdateCountUI();
    }
}
