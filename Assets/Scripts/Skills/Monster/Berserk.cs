using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : SkillBase
{
    [SerializeField] private float power;
    [SerializeField] private float effectDuration;
    private Transform rangeTransform;

    // 범위 안에 들어와 있는 적들
    private readonly HashSet<EnemyUnit> _enemyInRange = new HashSet<EnemyUnit>();

    // 적들의 속도를 기억
    private readonly Dictionary<EnemyUnit, float> _originalSpeeds = new Dictionary<EnemyUnit, float>();

    override public void Start()
    {
        IsReady = true;

        rangeTransform = GetComponent<Transform>();

        rangeTransform.localScale = new Vector3(_range, 1, _range);
    }

    override public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!endSkill)
            {
                var test = other.GetComponent<EnemyUnit>();
                test.BerserkBuff(power, effectDuration);
            }
            Debug.Log("범위내에 적 들어옴");
        }
    }
}
