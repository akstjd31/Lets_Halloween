public interface IFactionController
{
    FactionType Faction { get; }
    // 초기화
    void Initialize();

    // 게임 시작
    void StartGame();

    // 업데이트 부분
    void Update();

    // 게임 종료
    void EndGame();
}
