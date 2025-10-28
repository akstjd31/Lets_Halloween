using UnityEngine;

public class PlayerFactionController : MonoBehaviour, IFactionController
{
    public FactionType Faction { get; } = FactionType.Player;
    [SerializeField] private GameObject playerPrefab;
    private Player player;

    // 초기화 부분
    public virtual void Initialize()
    {
        GameObject newPlayerPrefab = Instantiate(playerPrefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
        Debug.Log("플레이어 생성됨!");

        Debug.Log("초기 세팅 중...");
        player = newPlayerPrefab.GetComponent<Player>();
        player.Initialize(newPlayerPrefab.name, 0);
    }

    public virtual void PreparationPhase()
    {
        // 상점 활성화
        // 각종 UI 활성화
        // 배치 모드 활성화
    }

    // 업데이트 구문
    public virtual void Update()
    {
        if (player != null)
        {
            
        }
    }
    
    // 전투 단계
    public virtual void BattlePhase()
    {
        
    }
    
    // 결과 단계
    public virtual void ResultPhase()
    {
        
    }
}
