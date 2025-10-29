using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : Unit, IFaction
{
    // 상수
    // 변수
    public int maxLife = 3;
    private int currentLife;

    // 프로퍼티
    public FactionType Faction => FactionType.Player;
    public int CurrentLife => currentLife;
    // 생성자

    // 메소드
    public override void Initialize(string name, int id)
    {
        base.Initialize(name, id);
        currentLife = maxLife;
    }

    public override void TakeDamage(int damage)
    {
        currentLife -= damage;
        currentLife = Mathf.Max(currentLife, 0);
        if (currentLife <= 0)
            OnDead();
    }

    public override void OnDead()
    {
        Debug.Log("게임 오버!");
    }
}
