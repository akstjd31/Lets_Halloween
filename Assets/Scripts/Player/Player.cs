using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : Unit, IFaction
{
    // 상수
    // 변수
    public int maxLife = 100;
    private int currentLife;
    private int money;
    private bool isDead;

    // 프로퍼티
    public FactionType Faction => FactionType.Player;
    public int CurrentLife => currentLife;
    public int Money => money;
    public bool IsDead => isDead;
    // 생성자

    // 메소드
    public override void Initialize(string name, int id)
    {
        base.Initialize(name, id);
        currentLife = maxLife;
        money = 1000;
        isDead = false;
    }

    public override void TakeDamage(int damage)
    {
        currentLife -= damage;
        currentLife = Mathf.Max(currentLife, 0);
        if (currentLife <= 0)
            OnDead();
    }

    public void ReceiveReward(int reward) => money += reward;
    

    public override void OnDead()
    {
        //Debug.Log("게임 오버!");
        isDead = true;
        GameManager.Instance.UpdateState(GameState.Result);
    }
}
