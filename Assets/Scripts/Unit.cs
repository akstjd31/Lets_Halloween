using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IEntity
{
    public string Name { get; set; }
    public int ID { get; set; }
    public int HP { get; set; }
    public int Money { get; set; }


    public virtual void Initialize(string name, int id)
    {
        Name = name;
        ID = id;
        Money = 0;
    }

    public void SetMoney(int money) => Money = money;


    public virtual void TakeDamage(int damage)
    {
        HP -= damage;
        HP = Mathf.Max(HP, 0);
        if (HP <= 0)
            OnDead();
    }

    public void ReceiveReward(int reward) => Money += reward;
    
    public virtual void OnDead()
    {
        Debug.Log($"{Name} 사망!");
    }
}
