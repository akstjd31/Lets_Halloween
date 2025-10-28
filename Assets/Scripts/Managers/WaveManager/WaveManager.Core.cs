using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private List<Wave> waves = new List<Wave>();

    public event Action onWaveStarted;
    public event Action onWaveEnded;

    private int currentWaveIndex;
    private bool isWaveRunning;
    private WaveRuntimeData runtimeData;

    private void Start()
    {
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

        Wave wave = waves[currentWaveIndex];
        runtimeData.Initialize(wave);

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

        if (runtimeData.IsWaveComplete)
            EndWave();
    }

    // 웨이브 종료
    private void EndWave()
    {
        isWaveRunning = false;
        onWaveEnded?.Invoke();
        currentWaveIndex++;

        GameManager.Instance.InitPreparingTime();
    }

    public bool IsWaveRunning() => isWaveRunning;
    private void HandleWaveStarted() =>
        GameManager.Instance.UpdateState(GameState.Battle);

    private void HandleWaveEnded() =>
        GameManager.Instance.UpdateState(GameState.Prepare);

    // 구독 해제
    private void OnDestroy()
    {
        onWaveStarted -= HandleWaveStarted;
        onWaveEnded -= HandleWaveEnded;
    }
}
