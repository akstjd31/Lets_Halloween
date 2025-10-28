using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Main, Prepare, Battle, Result
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerFactionController playerFactionController;
    [SerializeField] private EnemyFactionController enemyFactionController;
    [SerializeField] private GameState currentState;
    public FactionType selectedFactionType;    // 선택한 진영 타입 (플레이어의 주체가 누군지?)
    [SerializeField] private float preparingTime = 10f;   // 임시로 설정
    private IFactionController activeFaction;
    private bool isFirstWave;                             // 첫 번째 웨이브인가?

    // 준비 단계에서 남은 시간
    [SerializeField] private float elapsedTime;

    private void Start()
    {
        activeFaction = null;
        isFirstWave = true;
        elapsedTime = preparingTime;
        
        UpdateState(GameState.Main);
    }

    private void Update()
    {
        //activeFaction?.Update();

        // 각 상태에 따른 수행 부분
        switch (currentState)
        {
            case GameState.Main:
                break;
            case GameState.Prepare:
                activeFaction?.PreparationPhase();
                StartPreparing();
                break;
            case GameState.Battle:
                activeFaction?.BattlePhase();
                break;
            case GameState.Result:
                activeFaction?.ResultPhase();
                break;
        }
    }

    // 상태 업데이트
    public void UpdateState(GameState gameState) => this.currentState = gameState;

    // 현재 상태 비교 후 bool 반환
    public bool CompareState(GameState gameState) => this.currentState == gameState ? true : false;

    // 준비 완료 버튼 이벤트
    public void OnClickReadyButton()
    {
        if (currentState.Equals(GameState.Prepare))
        {
            isFirstWave = false;
            UpdateState(GameState.Battle);
        }
    }

    public void InitPreparingTime()
    {
        elapsedTime = preparingTime;
    }
    
    // 준비시간 게산
    private void StartPreparing()
    {
        if (!isFirstWave)
        {
            elapsedTime -= Time.deltaTime;

            // 준비 시간 끝나면 전투로 넘어감
            if (elapsedTime <= 0f)
            {
                UpdateState(GameState.Battle);
            }
        }
    }

    // 게임 시작 시 진영 선택에 따른 컨트롤러 제어권 관리
    private void StartGame()
    {
        // 선택한 진영에 따른 컨트롤러 부여
        activeFaction = selectedFactionType.Equals(FactionType.Player) ? playerFactionController : enemyFactionController;

        // 씬 넘어가기
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("InGame");
    }

    // 다음 씬에서 초기화하기 위함
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "InGame")
            return;

        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 해당 진영 선택 후 초기화, 준비 단계로 변경
        activeFaction?.Initialize();
        UpdateState(GameState.Prepare);
    }

    // 선택한 진영 (버튼 선택) - 임시
    public void SelectFaction(int buttonIdx)
    {
        selectedFactionType = (FactionType)buttonIdx;
        StartGame();
    }
}
