using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : SkillBase
{
    private Transform rangeTransform;

    private List<Enemy> affectedEnemies = new List<Enemy>();

    override public void Start()
    {
        IsReady = true;

        rangeTransform = GetComponent<Transform>();

        rangeTransform.localScale = new Vector3(0.65f * _range, 0.65f * _range, 0.65f * _range);
    }

    public override void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !affectedEnemies.Contains(enemy))
        {
            affectedEnemies.Add(enemy);
            enemy.tag = "Shield";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && affectedEnemies.Contains(enemy))
        {
            enemy.tag = "Enemy";
            affectedEnemies.Remove(enemy);
        }
    }

    private void OnDestroy()
    {
        foreach (var enemys in affectedEnemies)
        {
            if (enemys != null)
                enemys.tag = "Enemy";
        }
    }
}
