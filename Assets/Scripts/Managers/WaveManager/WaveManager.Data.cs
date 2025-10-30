using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class WaveManager
{
    // 웨이브 클래스
    [Serializable]
    public class Wave
    {
        public int waveNum;                         // 웨이브 번호
        public List<SpawnInfo> spawnInfos = new();  // 스폰될 적의 정보 리스트
        public int reward;                          // 보상
    }

    // 적 정보 클래스
    [Serializable]
    public class SpawnInfo
    {
        public Enemy enemyPrefab;   // 프리팹
        public int spawnCount;      // 생성할 적의 카운트
        public float spawnInterval; // 생성 주기
    }

    // 웨이브 당 총 생성 개수, 현재까지 스폰된 수, 비활성화된 오브젝트 수 등 확인 및 시간 관련
    [System.Serializable]
    public class WaveRuntimeData
    {
        public int totalSpawnCount { get; private set; }    // 웨이브별 총 생성 카운트
        public int spawnedCount { get; private set; }       // 현재까지 생성된 적 카운트

        private float spawnTimer;

        // 초기화
        public void Initialize(Wave wave)
        {
            spawnedCount = 0;
            spawnTimer = 0f;
            totalSpawnCount = 0;

            // 웨이브별 스폰될 적의 수 합산
            foreach (SpawnInfo info in wave.spawnInfos)
                totalSpawnCount += info.spawnCount;
        }

        // 시간 계산
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
    }
}
