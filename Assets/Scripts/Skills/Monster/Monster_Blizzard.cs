using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Monster_Blizzard : SkillBase
{
    [SerializeField] private float power;
    private Transform rangeTransform;

    // 범위 안에 들어와 있는 적들
    private readonly HashSet<PlayerUnit> _unitsInRange = new HashSet<PlayerUnit>();

    // 적들의 공격속도를 기억
    private readonly Dictionary<PlayerUnit, float> _originalAttackDelays = new Dictionary<PlayerUnit, float>();


    override public void Start()
    {
        IsReady = true;

        collider = GetComponent<SphereCollider>();

        GameObject rangeChild = transform.GetChild(0).Find("Freeze circle").gameObject;


        Debug.Log("자식" + rangeChild);

        rangeTransform = rangeChild.GetComponent<Transform>();

        rangeTransform.localScale = new Vector3(0.25f * _range, transform.position.y, 0.25f * _range);

        if (collider != null)
        {
            collider.radius = _range;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerUnit")
        {
            if (!endSkill)
            {
                PlayerUnit _unit = other.GetComponent<PlayerUnit>();

                // 이미 슬로우 중이라면 넣지 않음
                if (!_unitsInRange.Contains(_unit))
                {
                    Debug.Log("슬로우중");
                    _unitsInRange.Add(_unit);

                    // 원래 속도 저장
                    if (!_originalAttackDelays.ContainsKey(_unit))
                    {
                        Debug.Log("속도 저장");
                        _originalAttackDelays.Add(_unit, _unit.AttackDelay);
                    }
                    // 속도 감소
                    Debug.Log("속도 감소");
                    _unit.AttackSlow(power);
                    Debug.Log("현재 공격 속도" + _unit.AttackDelay);
                }
            }
            Debug.Log("범위내에 적 들어옴");
        }
    }

    private void OnDestroy()
    {
        foreach (var u in _unitsInRange)
        {
            if (u != null)
            {
                if (_originalAttackDelays.TryGetValue(u, out float originalAttackDelay))
                {
                    u.ResetAttackDelay(originalAttackDelay);

                    _originalAttackDelays.Remove(u);
                }
            }
        }

        _unitsInRange.Clear();
    }

}
