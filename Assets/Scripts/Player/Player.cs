using System.Collections.Generic;
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
    public override void Initialize(string name, int id) => base.Initialize(name, id);

    public void TakeDamage(int amount)
    {
        currentLife -= amount;
        currentLife = Mathf.Max(currentLife, 0);
        if (currentLife <= 0)
            OnPlayerDead();
    }

    private void OnPlayerDead()
    {
        Debug.Log("게임 오버!");
    }
}
