using UnityEngine;

public class EnemyFactionController : Singleton<EnemyFactionController>, IFactionController
{
    public FactionType Faction { get; }
    private Unit enemy;
    public virtual void Initialize(Unit unit)
    {
        Debug.Log("적 선택됨!");
        Debug.Log("초기 세팅 중...");

        enemy = new Unit();
        enemy.Initialize(unit.name, unit.ID);

        Debug.Log($"{enemy.name}님 환영합니다!");
    }

    public virtual void StartGame()
    {
        // 게임 시작 시 바로 준비 단계로 접어들기.
        Debug.Log("적 진영으로 게임 시작!");
        GameManager.Instance.UpdateState(GameState.Prepare);
    }

    public virtual void Update()
    {
        Debug.Log("업데이트...");
    }
    
    public virtual void EndGame()
    {
        
    }
}
