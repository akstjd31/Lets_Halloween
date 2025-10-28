using System;
using System.Collections.Generic;

public partial class WaveManager
{
    [Serializable]
    public class Wave
    {
        public int waveNum;
        public List<SpawnInfo> spawnInfos = new();
    }

    [Serializable]
    public class SpawnInfo
    {
        public Enemy enemyPrefab;
        public int spawnCount;
        public float spawnInterval;
    }

    // 웨이브 당 총 생성 개수, 현재까지 스폰된 수, 비활성화된 오브젝트 수 등 확인 및 시간 관련
    [System.Serializable]
    public class WaveRuntimeData
    {
        public int totalSpawnCount { get; private set; }
        public int spawnedCount { get; private set; }
        public int deactiveCount { get; private set; }

        private int spawnInfoListIndex;
        private float spawnTimer;
        private float currentInterval;

        public bool IsWaveComplete => deactiveCount >= totalSpawnCount;

        public void Initialize(Wave wave)
        {
            spawnedCount = 0;
            deactiveCount = 0;
            spawnInfoListIndex = 0;
            spawnTimer = 0f;
            totalSpawnCount = 0;

            foreach (SpawnInfo info in wave.spawnInfos)
                totalSpawnCount += info.spawnCount;
        }

        public void UpdateTimer(float deltaTime)
        {
            spawnTimer += deltaTime;
        }

        // 스폰 주기에 따른 생성 가능 여부 확인
        public bool CanSpawn(float interval)
        {
            if (spawnedCount >= totalSpawnCount)
                return false;

            if (spawnTimer >= interval)
            {
                spawnTimer = 0f;
                return true;
            }

            return false;
        }

        public void OnSpawned() => spawnedCount++;
        public void OnEnemyDeactivated() => deactiveCount++;
    }
}
