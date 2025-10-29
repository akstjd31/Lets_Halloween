using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private Transform endpoint;
    private EnemyDeathEventHandler enemyDeathEventHandler;

    public event Action onWaveStarted;
    public event Action onWaveEnded;
    public bool IsWaveComplete => 
        runtimeData.spawnedCount >= runtimeData.totalSpawnCount
        && activeEnemies.Count == 0;

    private int currentWaveIndex;
    private bool isWaveRunning;
    private WaveRuntimeData runtimeData;

    private void Start()
    {
        if (endpoint != null)
            enemyDeathEventHandler = endpoint.GetComponent<EnemyDeathEventHandler>();

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

    // 웨이브 진행 중
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

    // 적 비활성화 (이벤트 액션) - 적이 목적지에 도달한 경우 해줘야 하는 작업
    private void OnEnemyDeactivated(EnemyMover enemy)
    {
        // 초기화를 한 번 해준 다음에 비활성화
        enemy.Initialize();

        enemy.gameObject.SetActive(false);
        activeEnemies.Remove(enemy);
        enemyPool.Enqueue(enemy);
        runtimeData.OnEnemyDeactivated();
    }

    // 현 웨이브
    public int GetWaveNumber() => currentWaveIndex + 1;

    // 웨이브가 진행중인지?
    public bool IsWaveRunning() => isWaveRunning;

    private void HandleWaveStarted()
    {
        enemyDeathEventHandler.onEnemyDeactivated += OnEnemyDeactivated;

        GameManager.Instance.UpdateState(GameState.Battle);
        Wave wave = waves[currentWaveIndex];
        runtimeData.Initialize(wave);
    }
        
    private void HandleWaveEnded()
    {
        GameManager.Instance.UpdateState(GameState.Prepare);       
        currentWaveIndex++;
        spawnInfoIndex = 0;
    }
        
    private void OnDestroy()
    {
        // 이벤트 해제
        onWaveStarted -= HandleWaveStarted;
        onWaveEnded -= HandleWaveEnded;

        if (enemyDeathEventHandler != null)
            enemyDeathEventHandler.onEnemyDeactivated -= OnEnemyDeactivated;

        // 싱글톤 해제
        if (Instance == this)
            Instance = null;
    }
}
