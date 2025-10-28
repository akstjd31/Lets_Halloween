using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackOLentern : SkillBase
{
    [SerializeField] private float enemySpeed;
    
    void Awake()
    {
        _name = "잭오랜턴";
        _cooldown = 10f;
        IsReady = true;
        range = 5;
        _duration = 3;
        endSkill = false;
    }

    override public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!endSkill)
            {
                StartCoroutine(MoveToTarget(transform, other));
            }
            
            Debug.Log("범위내에 적 들어옴");
        }
    }

    IEnumerator MoveToTarget(Transform target, Collider obj)
    {
        while (true)
        {
            obj.gameObject.transform.position = Vector3.MoveTowards(obj.gameObject.transform.position, target.position, enemySpeed * Time.deltaTime);

            yield return null;
        }
    }

}
