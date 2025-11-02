using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 흐름 FSM
public enum GameState
{
    Main, Prepare, Battle, Result
}

// 난이도
public enum Difficulty
{
    Afternoon, Midnight, Hell
}

// 사용자가 선택한 데이터 정보가 담길 구조체
public struct GameOptionData
{
    public Unit unit;
    public FactionType factionType;
    public Difficulty difficulty;
}

public class GameManager : Singleton<GameManager>
{
    public IFactionController selectedFactionController;
    [SerializeField] private PlayerFactionController playerFactionController;
    [SerializeField] private EnemyFactionController enemyFactionController;
    [SerializeField] private GameState currentState;
    [SerializeField] private float preparingTime;   // 임시로 설정

    public GameOptionData gameOptionData;
    private bool isFirstWave;                       // 첫 번째 웨이브인가?
    public bool isGameOver;
    public bool isGameClear;
    private bool hasProcessed = false;


    // 준비 단계에서 남은 시간
    public float elapsedTime;

    private void Start()
    {
        isGameOver = false;
        isGameClear = false;
        isFirstWave = true;
        elapsedTime = preparingTime;

        UpdateState(GameState.Main);
    }

    private void Update()
    {
        // 게임 오버 상태
        if (isGameOver && !hasProcessed)
        {
            hasProcessed = true;
            SceneManager.LoadScene("Defeat");
        }

        if (isGameClear && !hasProcessed)
        {
            hasProcessed = true;
            SceneManager.LoadScene("Victory");
        }

        // 각 상태에 따른 수행 부분
        switch (currentState)
        {
            case GameState.Main:
                break;
            case GameState.Prepare:
                selectedFactionController?.PreparationPhase();
                CalPreparingTime();
                break;
            case GameState.Battle:
                selectedFactionController?.BattlePhase();
                break;
            case GameState.Result:
                selectedFactionController?.ResultPhase();
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

            if (gameOptionData.factionType.Equals(FactionType.Enemy))
                (selectedFactionController as EnemyFactionController).SetUnitActive(true);
        }
    }

    public void InitPreparingTime() => elapsedTime = preparingTime;

    // 준비시간 게산
    private void CalPreparingTime()
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
        UpdateState(GameState.Prepare);
        
        // 선택한 진영에 따른 컨트롤러 부여
        selectedFactionController = gameOptionData.factionType.Equals(FactionType.Player) ? playerFactionController : enemyFactionController;

        // 씬 넘어가기
        SceneManager.sceneLoaded += OnSceneLoaded;

        string loadSceneName = "";
        if (gameOptionData.factionType.Equals(FactionType.Player))
            loadSceneName = "Deffence";
        else
            loadSceneName = "Offence";

        SceneManager.LoadScene(loadSceneName);
    }

    // 다음 씬에서 초기화하기 위함
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 해당 진영 선택 후 초기화, 유닛 설정, 준비 단계로 변경
        selectedFactionController?.Initialize();
        gameOptionData.unit = selectedFactionController?.GetUnit();

        // endpoint 유닛 세팅
        if (gameOptionData.unit != null)
            FindFirstObjectByType<EnemyDeathEventHandler>().SetUnit(gameOptionData.unit);
    }

    // 선택한 진영 (버튼 선택)
    public void OnSelectFactionButtonClicked(int buttonIdx)
    {
        gameOptionData.factionType = (FactionType)buttonIdx;
    }

    // 난이도 선택
    public void OnSelectDifficultyButtonClicked(int buttonIdx)
    {
        gameOptionData.difficulty = (Difficulty)buttonIdx;
    }

    // 결정 버튼 누름
    public void OnConfirmButtonClicked()
    {
        StartGame();
    }
}
