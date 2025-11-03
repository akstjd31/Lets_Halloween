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
    }

    // 웨이브 시작
    public void StartWave()
    {
        // if (currentWaveIndex >= waves.Count)
        //     return;

        isWaveRunning = true;
        onWaveStarted?.Invoke();
    }

    // 웨이브 진행
    public void RunWave()
    {
        if (!isWaveRunning)
            return;

        if (GameManager.Instance.gameOptionData.factionType.Equals(FactionType.Player))
        {
            runtimeData.UpdateTimer(Time.deltaTime);

            Wave wave = waves[currentWaveIndex];
            TrySpawnEnemy(wave);

            if (IsWaveComplete)
                EndWave();
        }
        else
        {
            
        }
    }

    // 웨이브 종료
    private void EndWave()
    {
        isWaveRunning = false;
        GameManager.Instance.InitPreparingTime();

        onWaveEnded?.Invoke();
    }

    public void OnEnemyDeactivated(Enemy enemy)
    {
        if (enemy == null)
            return;

        // 적 비활성화 및 초기화
        enemy.GetComponent<EnemyUnit>().SetWayPoint(0);
        enemy.gameObject.SetActive(false);

        ReturnToPool(enemy);
        activeEnemies.Remove(enemy);

        Debug.Log("적 회수! 현재 맵에 존재하는 적 = " + activeEnemies.Count);
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
            enemyDeathEventHandler.onEnemyDeactivated += OnEnemyDeactivated;
        }

        GameManager.Instance.UpdateState(GameState.Battle);

        if (GameManager.Instance.gameOptionData.factionType.Equals(FactionType.Player))
        {
            Wave wave = waves[currentWaveIndex];
            runtimeData.Initialize(wave);
        }
    }

    // 웨이브 종료 시점 (이벤트 해제, 인덱스 증가)
    private void HandleWaveEnded()
    {
        GameManager.Instance.UpdateState(GameState.Prepare);

        if (enemyDeathEventHandler != null)
            enemyDeathEventHandler.onEnemyDeactivated -= OnEnemyDeactivated;

        // 보상 지급
        Unit unit = GameManager.Instance.gameOptionData.unit;
        var getGameObject = GameObject.FindWithTag("SkillButton_Tank");
        var coolTime = getGameObject.GetComponentInChildren<SkillCoolDown>();
        coolTime.ResetTimer();

        if (unit is Player player)
        {
            Debug.Log(waves[currentWaveIndex].reward + " 받음!");
            player?.ReceiveReward(waves[currentWaveIndex].reward);
        }


        // 세팅
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
