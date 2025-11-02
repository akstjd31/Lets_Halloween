using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : SkillBase
{
    private Transform rangeTransform;

    override public void Start()
    {
        IsReady = true;

        rangeTransform = GetComponent<Transform>();

        rangeTransform.localScale = new Vector3(_range, 1,_range);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
