using System;
using System.Collections.Generic;
using UnityEngine;

// 웨이브 관리
public partial class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private List<Wave> waves = new List<Wave>();
    public event Action onWaveStarted;
    public event Action onWaveEnded;

    public bool isWaveRunning;
    public int currentWaveIndex;
    public int deactiveCount;

    private int totalSpawnCount;
    private int spawnedCount;
    private int spawnInfoListIndex;
    private float spawnTimer;

    private void Start()
    {
        currentWaveIndex = 0;
        onWaveStarted += HandleWaveStarted;
        onWaveEnded += HandleWaveEnded;
    }

    public void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
            return;

        isWaveRunning = true;
        deactiveCount = 0;
        spawnedCount = 0;
        spawnInfoListIndex = 0;
        totalSpawnCount = 0;

        // 웨이브에 존재하는 적의 총 개수 계산
        foreach (SpawnInfo spawnInfo in waves[currentWaveIndex].spawnInfos)
            totalSpawnCount += spawnInfo.spawnCount;

        onWaveStarted?.Invoke();
    }

    public void RunWave()
    {
        if (!isWaveRunning)
            return;

        Wave wave = waves[currentWaveIndex];
        spawnTimer += Time.deltaTime;

        float spawnInterval = wave.spawnInfos[spawnInfoListIndex].spawnInterval;
        if (spawnedCount < totalSpawnCount && spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy(wave);
            spawnedCount++;
        }

        if (deactiveCount >= totalSpawnCount)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        isWaveRunning = false;
        onWaveEnded?.Invoke();

        currentWaveIndex++;
        GameManager.Instance.InitPreparingTime();
    }

    private void HandleWaveStarted() =>
        GameManager.Instance.UpdateState(GameState.Battle);

    private void HandleWaveEnded() =>
        GameManager.Instance.UpdateState(GameState.Prepare);

    private void OnDestroy()
    {
        onWaveStarted -= HandleWaveStarted;
        onWaveEnded -= HandleWaveEnded;
    }
}
