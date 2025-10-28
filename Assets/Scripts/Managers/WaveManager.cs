using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
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
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform endPoint;
    [SerializeField] float minZPos;
    [SerializeField] float maxZPos;
    public event Action onWaveStarted;  // 웨이브 시작 시 이벤트
    public event Action onWaveEnded;    // 웨이브 종료 시 이벤트
    public bool isWaveRunning;          // 웨이브 진행 여부
    public int currentWaveIndex;       // 현재 진행중인 웨이브
    private int spawnedCount;           // 생성된 적 카운트
    public int deactiveCount;          // 비활성화된 적 마리 수 체크
    private float spawnTimer;           // 생성 주기 관련 타이머

    // 임시 데이터
    private void Start()
    {
        currentWaveIndex = 0;
        deactiveCount = 0;
        isWaveRunning = false;

        onWaveStarted += () => GameManager.Instance.UpdateState(GameState.Battle);
        onWaveEnded += () => GameManager.Instance.UpdateState(GameState.Prepare);
    }

    public void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
            return;

        isWaveRunning = true;
        spawnedCount = 0;

        onWaveStarted?.Invoke();
    }

    public void RunWave()
    {
        Wave wave = waves[currentWaveIndex];

        spawnTimer += Time.deltaTime;

        if (spawnedCount < wave.waveEnemyCount && spawnTimer >= wave.spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy(wave);
            spawnedCount++;
        }

        if (deactiveCount >= wave.waveEnemyCount)
        {
            EndWave();
        }
    }

    public void EndWave()
    {
        isWaveRunning = false;
        onWaveEnded?.Invoke();

        currentWaveIndex++;
        GameManager.Instance.UpdateState(GameState.Prepare);
        GameManager.Instance.InitPreparingTime();
    }

    private void SpawnEnemy(Wave wave)
    {
        Vector3 newPos = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            spawnPoint.position.z + UnityEngine.Random.Range(minZPos, maxZPos)

        );

        EnemyMover enemy = Instantiate(wave.enemies[0], newPos, Quaternion.identity).GetComponent<EnemyMover>();

        enemy.onEnemyDeactivated += () => deactiveCount++;
    }

    public Transform GetWaypoint(int index) => waypoints[index];
}
