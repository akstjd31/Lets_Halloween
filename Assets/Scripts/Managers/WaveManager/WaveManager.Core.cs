using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WaveManager
{
    [SerializeField] private List<Wave> waves = new List<Wave>();
    private EnemyDeathEventHandler enemyDeathEventHandler;

    public event Action onWaveStarted;
    public event Action onWaveEnded;

    public bool IsWaveComplete =>
        runtimeData.spawnedCount >= runtimeData.totalSpawnCount &&
        activeEnemies.Count == 0;

    private int currentWaveIndex;
    private bool isWaveRunning;
    private WaveRuntimeData runtimeData;

    private void Start()
    {
        // 웨이포인트 세팅
        for (int i = 0; i < waypoint.childCount; i++)
            waypoints.Add(waypoint.GetChild(i));

        // 목적지 오브젝트의 이벤트 핸들러 참조
        if (endPoint != null)
            enemyDeathEventHandler = endPoint.GetComponent<EnemyDeathEventHandler>();

        currentWaveIndex = 0;
        runtimeData = new WaveRuntimeData();

        onWaveStarted += HandleWaveStarted;
        onWaveEnded += HandleWaveEnded;
    }

    // 웨이브 시작
    public void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
            return;

        isWaveRunning = true;
        onWaveStarted?.Invoke();
    }

    // 웨이브 진행
    public void RunWave()
    {
        if (!isWaveRunning)
            return;

        runtimeData.UpdateTimer(Time.deltaTime);

        Wave wave = waves[currentWaveIndex];
        TrySpawnEnemy(wave);

        if (IsWaveComplete)
            EndWave();
    }

    // 웨이브 종료
    private void EndWave()
    {
        isWaveRunning = false;
        onWaveEnded?.Invoke();

        GameManager.Instance.InitPreparingTime();
    }

    private void OnEnemyDeactivated(Enemy enemy)
    {
        if (enemy == null)
            return;

        EnemyMover enemyMover = enemy.GetComponent<EnemyMover>();    

        // 적 비활성화 및 초기화
        enemyMover.Initialize();
        enemy.gameObject.SetActive(false);

        activeEnemies.Remove(enemy);

        ReturnToPool(enemy);

        runtimeData.OnEnemyDeactivated();
    }

    // 현재 웨이브 번호 반환
    public int GetWaveNumber() => currentWaveIndex + 1;

    // 웨이브 진행 중인지 확인
    public bool IsWaveRunning() => isWaveRunning;

    // 웨이브 시작 시점 (이벤트 구독 등)
    private void HandleWaveStarted()
    {
        // 이벤트 구독 (중복 방지)
        if (enemyDeathEventHandler != null)
        {
            enemyDeathEventHandler.onEnemyDeactivated -= OnEnemyDeactivated;
            enemyDeathEventHandler.onEnemyDeactivated += OnEnemyDeactivated;
        }

        GameManager.Instance.UpdateState(GameState.Battle);

        Wave wave = waves[currentWaveIndex];
        runtimeData.Initialize(wave);
    }

    // 웨이브 종료 시점 (이벤트 해제, 인덱스 증가)
    private void HandleWaveEnded()
    {
        GameManager.Instance.UpdateState(GameState.Prepare);

        if (enemyDeathEventHandler != null)
            enemyDeathEventHandler.onEnemyDeactivated -= OnEnemyDeactivated;

        currentWaveIndex++;
        spawnInfoIndex = 0;
    }

    // 안전한 해제
    private void OnDisable()
    {
        onWaveStarted = null;
        onWaveEnded = null;

        if (enemyDeathEventHandler != null)
            enemyDeathEventHandler.onEnemyDeactivated -= OnEnemyDeactivated;
    }
}
