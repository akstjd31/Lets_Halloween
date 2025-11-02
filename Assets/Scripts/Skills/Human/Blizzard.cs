using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : SkillBase
{
    private Transform rangeTransform;
    
    override public void Start()
    {
        IsReady = true;

        collider = GetComponent<SphereCollider>();

        GameObject rangeChild = transform.Find("Freeze circle").gameObject;

        Debug.Log("ÀÚ½Ä"+rangeChild);

        rangeTransform = rangeChild.GetComponent<Transform>();

        rangeTransform.localScale = new Vector3(0.25f * _range, transform.position.y, 0.25f * _range);

        if (collider != null)
        {
            collider.radius = _range;
        }
    }
}
