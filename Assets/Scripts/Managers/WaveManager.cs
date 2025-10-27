using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Serializable]
    public class Wave
    {
        public int waveNum;            // 웨이브 번호
        public int waveEnemyCount;     // 웨이브 당 생성될 적 수
        public float spawnInterval;    // 스폰 주기
        public List<Enemy> enemies;    // 해당 웨이브에 소환될 적 리스트
    }

    [SerializeField] List<Wave> waves = new List<Wave>();
    public event Action onWaveStarted;  // 웨이브 시작 시 이벤트
    public event Action onWaveEnded;    // 웨이브 종료 시 이벤트
    private int currentWaveNum;         // 현재 (진행한, 진행중인) 웨이브
    private int spawnedCount;           // 생성된 적 카운트
    private float spawnTimer;
    private bool isWaveRunning = false;

    // 임시 데이터
    private void Start()
    {
        currentWaveNum = 1;
    }

    private void StartWave()
    {
        if (currentWaveNum >= waves.Count)
            return;

        isWaveRunning = true;
        spawnedCount = 0;

        onWaveStarted?.Invoke();

        Debug.Log($"Wave {currentWaveNum} 시작!");
    }

    private void RunWave()
    {
        Wave wave = waves[currentWaveNum];

        
    }
}
