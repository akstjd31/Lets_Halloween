using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Blizzard : SkillBase
{
    [SerializeField] private float power;
    private Transform rangeTransform;

    // 범위 안에 들어와 있는 적들
    private readonly HashSet<EnemyUnit> _enemyInRange = new HashSet<EnemyUnit>();

    // 적들의 속도를 기억
    private readonly Dictionary<EnemyUnit, float> _originalSpeeds = new Dictionary<EnemyUnit, float>();

    override public void Start()
    {
        IsReady = true;

        collider = GetComponent<SphereCollider>();

        GameObject rangeChild = transform.GetChild(0).Find("Freeze circle").gameObject;


        Debug.Log("자식"+rangeChild);

        rangeTransform = rangeChild.GetComponent<Transform>();

        rangeTransform.localScale = new Vector3(0.25f * _range, transform.position.y, 0.25f *_range);

        if (collider != null)
        {
            collider.radius = _range;
        }
    }

    override public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!endSkill)
            {
                EnemyUnit _enemy = other.GetComponent<EnemyUnit>();

                // 이미 슬로우 중이라면 넣지 않음
                if (!_enemyInRange.Contains(_enemy))
                {
                    Debug.Log("슬로우중");
                    _enemyInRange.Add(_enemy);

                    // 원래 속도 저장
                    if (!_originalSpeeds.ContainsKey(_enemy))
                    {
                        Debug.Log("속도 저장");
                        _originalSpeeds.Add(_enemy, _enemy.MoveSpeed);
                    }
                    // 속도 감소
                    Debug.Log("속도 감소");
                    _enemy.MoveSpeed *= 1 - (power / 100f);
                    Debug.Log("현재 속도"+_enemy.MoveSpeed);
                }            
            }
            Debug.Log("범위내에 적 들어옴");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyUnit _enemy = other.GetComponent<EnemyUnit>();

            if (_enemyInRange.Contains(_enemy))
            {
                ResetSpeed(_enemy);

                _enemyInRange.Remove(_enemy);
            }
        }
    }

    private void ResetSpeed(EnemyUnit _enemy)
    {
        if (_originalSpeeds.TryGetValue(_enemy, out float originalSpeed))
        {
            _enemy.MoveSpeed = originalSpeed;

            _originalSpeeds.Remove(_enemy);
        }
    }

    private void OnDestroy()
    {
        foreach (EnemyUnit e in _enemyInRange)
        {
            if (e != null)
            {
                ResetSpeed(e);
            }
        }

        _enemyInRange.Clear();
    }
}
