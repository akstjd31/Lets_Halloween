public interface IFactionController
{
    FactionType FactionType { get; }
    // 초기화
    void Initialize();

    // 준비 단계
    void PreparationPhase();

    // 업데이트 부분
    void Update();

    // 전투 단계
    void BattlePhase();

    // 결과 단계
    void ResultPhase();
}
