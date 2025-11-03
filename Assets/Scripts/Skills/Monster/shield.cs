using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class shield : SkillBase
{
    private Transform rangeTransform;

    private readonly HashSet<Enemy> enemyInRange = new HashSet<Enemy>();
    private readonly Dictionary<Enemy, string> _originalTag = new Dictionary<Enemy, string>();
   
    override public void Start()
    {
        IsReady = true;

        rangeTransform = GetComponent<Transform>();

        rangeTransform.localScale = new Vector3(0.65f * _range, 0.65f * _range, 0.65f * _range);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyUnit")
        {
            if (!endSkill)
            {
                Enemy _enemy = other.GetComponent<Enemy>();

                // 이미 슬로우 중이라면 넣지 않음
                if (!enemyInRange.Contains(_enemy))
                {
                    Debug.Log("슬로우중");
                    enemyInRange.Add(_enemy);

                    if (!_originalTag.ContainsKey(_enemy))
                    {
                        Debug.Log("속도 저장");
                        _originalTag.Add(_enemy, _enemy.tag);
                    }
                    _enemy.tag = "Shield";
                }
            }
            Debug.Log("범위내에 적 들어옴");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Shield")
        {
            Enemy _enemy = other.GetComponent<Enemy>();

            if (enemyInRange.Contains(_enemy))
            {
                ResetTag(_enemy); 

                enemyInRange.Remove(_enemy);
            }
        }
    }

    private void ResetTag(Enemy _enemy)
    {
        if (_originalTag.TryGetValue(_enemy, out string originalTag))
        {
            _enemy.tag = originalTag;

            _originalTag.Remove(_enemy);
        }
    }

    private void OnDestroy()
    {
        foreach (Enemy e in enemyInRange)
        {
            if (e != null)
            {
                ResetTag(e);
            }
        }

        enemyInRange.Clear();
    }
}
