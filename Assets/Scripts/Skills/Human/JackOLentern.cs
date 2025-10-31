using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackOLentern : SkillBase
{
    [SerializeField] private float enemySpeed;
    [SerializeField] private float power;

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
            obj.gameObject.transform.position = Vector3.MoveTowards(obj.gameObject.transform.position, target.position, enemySpeed * Time.deltaTime * power);

            yield return null;
        }
    }

}
