using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFactionController : MonoBehaviour, IFactionController
{
    public FactionType FactionType { get; } = FactionType.Player;
    [SerializeField] private GameObject playerPrefab;
    private Player player;

    // 초기화 부분
    public virtual void Initialize()
    {
        // 선택한 진영과 일치한다면
        if (GameManager.Instance.gameOptionData.factionType.Equals(FactionType))
        {
            GameObject newPlayerPrefab = Instantiate(playerPrefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
            Debug.Log("플레이어 생성됨!");

            Debug.Log("초기 세팅 중...");
            player = newPlayerPrefab.GetComponent<Player>();
            player.Initialize(newPlayerPrefab.name, 0);
        }

        // 선택한 진영이 아닌 경우
        else
        {
            
        }
    }

    // 준비 페이즈
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

    /// <summary> 전투 페이즈
    /// 플레이어를 선택한 경우,
    /// 슬롯 활성화
    /// 
    /// </summary>
    public virtual void BattlePhase()
    {
        // 웨이브 시작
        if (!WaveManager.Instance.IsWaveRunning())
            WaveManager.Instance.StartWave();


        if (WaveManager.Instance.IsWaveRunning())
            WaveManager.Instance.RunWave();
    }

    // 결과 페이즈
    public virtual void ResultPhase()
    {
        // 플레이어가 죽었는가?
        if (player.IsDead)
        {
            GameManager.Instance.isGameOver = true;
        }
        else
        {
        }
    }

    public virtual Unit GetUnit() => player;
}
