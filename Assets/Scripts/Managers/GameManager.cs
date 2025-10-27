using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerFactionController playerFactionController;
    [SerializeField] private EnemyFactionController enemyFactionController;
    private IFactionController activeFaction;

    private void Update()
    {
        activeFaction?.Update();
    }

    // 게임 시작 시 진영 선택에 따른 컨트롤러 제어권 관리
    private void StartGame(FactionType factionType)
    {
        // 선택한 진영에 따른 컨트롤러 부여
        activeFaction = factionType == FactionType.Player ? playerFactionController : enemyFactionController;

        // 초기화 및 게임 시작
        activeFaction?.Initialize();
        activeFaction?.StartGame();
    }

    // 선택한 진영 (버튼 선택)
    public void SelectFaction(int buttonIdx)
    {
        FactionType selectedFaction = (FactionType)buttonIdx;
        StartGame(selectedFaction);
    }
}
