using UnityEngine;

public class EnemyFactionController : Singleton<EnemyFactionController>, IFactionController
{
    public FactionType Faction { get; } = FactionType.Enemy;
    [SerializeField] private GameObject enemyPrefab;
    private Enemy enemy;

    // 초기화 부분
    public virtual void Initialize()
    {
        GameObject newEnemyPrefab = Instantiate(enemyPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        Debug.Log("적 생성됨!");

        Debug.Log("초기 세팅 중...");
        enemy = newEnemyPrefab.GetComponent<Enemy>();
        enemy.Initialize(newEnemyPrefab.name, 0);
    }

    public virtual void PreparationPhase()
    {
        // 게임 시작 시 바로 준비 단계로 접어들기.
        Debug.Log("적 진영으로 게임 시작!");
        Debug.Log("준비 단계");

        // 상점 활성화
        // 각종 UI 활성화
        // 배치 모드 활성화
    }

    public virtual void Update()
    {
    }

    public virtual void BattlePhase()
    {
        
    }
    
    public virtual void ResultPhase()
    {
        
    }
}
