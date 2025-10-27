using UnityEngine;

public class PlayerFactionController : Singleton<EnemyFactionController>, IFactionController
{
    public FactionType Faction { get; }
    private Unit player;
    public virtual void Initialize(Unit unit)
    {
        Debug.Log("플레이어 선택됨!");
        Debug.Log("초기 세팅 중...");

        player = new Unit();
        player.Initialize(unit.name, unit.ID);

        Debug.Log($"{player.name}님 환영합니다!");
    }

    public virtual void StartGame()
    {
        Debug.Log("플레이어 진영으로 게임 시작!");
    }

    public virtual void Update()
    {

    }
    
    public virtual void EndGame()
    {
        
    }
}
