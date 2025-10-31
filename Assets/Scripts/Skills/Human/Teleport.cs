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

    void Update()
    {
        
    }
}
