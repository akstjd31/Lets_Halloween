using UnityEngine;

public class PlayerFactionController : Singleton<PlayerFactionController>, IFactionController
{
    public FactionType Faction { get; } = FactionType.Player;
    [SerializeField] private GameObject playerPrefab;
    private Player player;

    // 초기화 부분
    public virtual void Initialize()
    {
        GameObject newPlayerPrefab = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        Debug.Log("플레이어 생성됨!");

        Debug.Log("초기 세팅 중...");
        player = newPlayerPrefab.GetComponent<Player>();
        player.Initialize(newPlayerPrefab.name, 0);

        Debug.Log($"{player.name}님 환영합니다!");
    }

    public virtual void StartGame()
    {
        Debug.Log("플레이어 진영으로 게임 시작!");
        Debug.Log("준비 단계");

        // 상점 활성화
        // 각종 UI 활성화
        // 배치 모드 활성화

    }

    // public virtual void Update()
    // {
    // }
    
    // 끝나는 시점에 해야될 것 작성
    public virtual void EndGame()
    {
        
    }
}
