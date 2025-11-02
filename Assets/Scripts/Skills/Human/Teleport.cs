using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : SkillBase
{
    private Transform rangeTransform;

    override public void Start()
    {

        IsReady = true;

        rangeTransform = GetComponent<Transform>();

        rangeTransform.localScale = new Vector3(_range, _range, _range);
    }

    override public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!endSkill)
            {
                
                EnemyUnit _enemy = other.GetComponent<EnemyUnit>();
                _enemy.SetWayPoint(0);
                _enemy.Teleport();
            }
            Debug.Log("범위내에 적 들어옴");
        }
    }
}
