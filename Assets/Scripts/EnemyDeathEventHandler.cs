using System;
using UnityEngine;

public class EnemyDeathEventHandler : MonoBehaviour
{
    public event Action<EnemyMover> onEnemyDeactivated;
    private Unit unit;

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }
    
    private void OnDestroy()
    {
        onEnemyDeactivated = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMover enemy = other.GetComponent<EnemyMover>();
            onEnemyDeactivated?.Invoke(enemy);
            unit.TakeDamage(1);
        }
    }
}
