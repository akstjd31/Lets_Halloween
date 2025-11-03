using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class MonsterUnitButton : ParentButton
{
    [SerializeField] public EnemyUnit enemyPrefab;
    [SerializeField] private int unitCount;
    private TextMeshProUGUI countText;

    virtual public void Start()
    {
        countText = GetComponentInChildren<TextMeshProUGUI>();
        Color imageColor = transform.Find("UnitImage").GetComponent<Image>().color;
        imageColor.a = 0.5f;
        transform.Find("UnitImage").GetComponent<Image>().color = imageColor;

        UpdateCountUI();
    }

    private void UpdateCountUI()
    {
        if (countText != null)
            countText.text = unitCount.ToString();

        button.interactable = unitCount > 0;
        Color imageColor = transform.Find("UnitImage").GetComponent<Image>().color;
        imageColor.a = 1f;
        transform.Find("UnitImage").GetComponent<Image>().color = imageColor;

        if (unitCount <= 0)
        {
            button.interactable = false;
            imageColor.a = 0.5f;
            transform.Find("UnitImage").GetComponent<Image>().color = imageColor;
        }

        Debug.Log(unitCount);
    }
    public override void OnClickButton()
    {
        if (GameManager.Instance.GetGameState().Equals(GameState.Prepare))
            return;

        // 스킬 개수가 0이면 사용 불가
        if (unitCount <= 0)
        {
            button.interactable = false;
            return;
        }

    }

    // Update is called once per frame
    public void AddCount()
    {
        unitCount++;
        UpdateCountUI();
    }

    public void RemoveCount()
    {
        unitCount = Mathf.Max(0, unitCount - 1);
        UpdateCountUI();
    }
}
