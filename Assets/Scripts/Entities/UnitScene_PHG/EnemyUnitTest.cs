using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitTest : MonoBehaviour
{
    [SerializeField] private int enemyHp = 10;


    private void Update()
    {
        EnemyHp();
    }

    void EnemyHp()
    {
        if(enemyHp <=0)
        {
            Destroy(gameObject);
        }
    }

    void TakeDamage()
    {
        enemyHp--;
    }


}
