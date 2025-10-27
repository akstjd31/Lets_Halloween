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
    private GameState currentState;
    private bool onGameStart;

    private void Start()
    {
        currentState = GameState.Main;
    }

    private void Update()
    {
        // activeFaction?.Update();

        // 각 상태에 따른 수행 부분
        switch (currentState)
        {
            case GameState.Prepare:
                break;
            case GameState.Battle:
                break;
            case GameState.Result:
                break;
        }
    }

    public void UpdateState(GameState gameState) => this.currentState = gameState;

    // 게임 시작 시 진영 선택에 따른 컨트롤러 제어권 관리
    private void StartGame(FactionType factionType)
    {
        // 상태 변경
        onGameStart = true;

        // 선택한 진영에 따른 컨트롤러 부여
        activeFaction = factionType == FactionType.Player ? playerFactionController : enemyFactionController;

        // 씬 넘어가기
        SceneManager.LoadScene("InGame");

        // 초기화 및 게임 시작
        Unit unit = null;
        switch (factionType)
        {
            case FactionType.Player:
                unit = GameObject.Find("Player").GetComponent<Unit>();
                break;
            case FactionType.Enemy:
                unit = GameObject.Find("Enemy").GetComponent<Unit>();
                break;
        }
        activeFaction?.Initialize(unit);
        activeFaction?.StartGame();
    }

    // 선택한 진영 (버튼 선택)
    public void SelectFaction(int buttonIdx)
    {
        FactionType selectedFaction = (FactionType)buttonIdx;
        StartGame(selectedFaction);
    }
}
