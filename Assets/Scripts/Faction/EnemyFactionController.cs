using UnityEngine;

public class EnemyFactionController : MonoBehaviour, IFactionController
{
    public FactionType FactionType { get; } = FactionType.Enemy;
    [SerializeField] private GameObject enemyPrefab;
    private MyEnemyUnit enemy;

    // 초기화 부분
    public virtual void Initialize()
    {
        // 선택한 진영과 일치한다면
        if (GameManager.Instance.gameOptionData.factionType.Equals(FactionType))
        {
            GameObject newEnemyPrefab = Instantiate(enemyPrefab, new Vector3(35f, 0f, -15f), Quaternion.identity);
            Debug.Log("적 생성됨!");

            Debug.Log("초기 세팅 중...");
            enemy = newEnemyPrefab.GetComponent<MyEnemyUnit>();
            enemy.Initialize(newEnemyPrefab.name, 0);
            enemy.SetMoney(200);
            SetUnitActive(false);
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

    public virtual void Update()
    {
        if (enemy != null)
        {

        }
    }

    // 전투 페이즈
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

    }

    public void SetUnitActive(bool active) => enemy.gameObject.SetActive(active);

    public virtual Unit GetUnit() => enemy;
}
