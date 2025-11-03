using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : ParentButton
{
    [SerializeField] public int skillCount;
    [SerializeField] public SkillBase skillPrefab;
    private TextMeshProUGUI countText;
    [SerializeField] public GameObject PreviewObj;

    virtual public void Start()
    {
        countText = GetComponentInChildren<TextMeshProUGUI>();
        Color imageColor = transform.Find("SkillImage").GetComponent<Image>().color;
        imageColor.a = 0.5f;
        transform.Find("SkillImage").GetComponent<Image>().color = imageColor;

        UpdateCountUI();
    }

    private void UpdateCountUI()
    {
        if (countText != null)
            countText.text = skillCount.ToString();

        button.interactable = skillCount > 0;
        Color imageColor = transform.Find("SkillImage").GetComponent<Image>().color;
        imageColor.a = 1f;
        transform.Find("SkillImage").GetComponent<Image>().color = imageColor;

        if (skillCount <= 0 )
        {
            button.interactable = false;
            imageColor.a = 0.5f;
            transform.Find("SkillImage").GetComponent<Image>().color = imageColor;
        }

        Debug.Log(skillCount);
    }

    public override void OnClickButton()
    {
        if (skillCount <= 0)
        {
            button.interactable = false;
            return;
        }

        base.OnClickButton(); // �θ𿡼� ȣ��
        var skillCoolDownAnim = transform.GetComponentInChildren<SkillCoolDown>();
        MouseTrackingManager.Instance.targetObject = PreviewObj;
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
