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
    private IFactionController activeFaction;
    [SerializeField] private GameState currentState;
    [SerializeField] private float PreparingTime = 10f;   // 임시로 설정
    private bool onGameStart;

    // 준비 단계에서 남은 시간
    [SerializeField] private float elapsedTime;

    private void Start()
    {
        activeFaction = null;
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
                Preparing();
                break;
            case GameState.Battle:
                break;
            case GameState.Result:
                break;
        }
    }

    public void UpdateState(GameState gameState) => this.currentState = gameState;

    // 준비
    private void Preparing()
    {
        elapsedTime -= Time.deltaTime;

        // 준비 시간 끝나면 전투로 넘어감
        if (elapsedTime <= 0f)
        {
            UpdateState(GameState.Battle);
        }
    }

    // 게임 시작 시 진영 선택에 따른 컨트롤러 제어권 관리
    private void StartGame(FactionType factionType)
    {
        // 상태 변경
        onGameStart = true;

        // 선택한 진영에 따른 컨트롤러 부여
        activeFaction = factionType == FactionType.Player ? playerFactionController : enemyFactionController;

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

        elapsedTime = PreparingTime;

        // 해당 진영 선택 후 초기화, 준비 단계로 변경
        activeFaction?.Initialize();
        activeFaction?.StartGame();
        UpdateState(GameState.Prepare);
    }

    // 선택한 진영 (버튼 선택) - 임시
    public void SelectFaction(int buttonIdx)
    {
        FactionType selectedFaction = (FactionType)buttonIdx;
        StartGame(selectedFaction);
    }
}
