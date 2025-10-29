using System.Collections.Generic;
using UnityEngine;

public partial class WaveManager
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float minZPos;
    [SerializeField] private float maxZPos;
    [SerializeField] private int poolSize;
    [SerializeField] private int spawnInfoIndex;
    private List<EnemyMover> activeEnemies = new List<EnemyMover>();

    private Queue<EnemyMover> enemyPool = new Queue<EnemyMover>();

    // 미리 생성해두기 & 구독 
    private void InitPool(EnemyMover prefab)
    {
        for (int i = 0; i < poolSize; i++)
        {
            EnemyMover enemy = Instantiate(prefab, this.transform);
            enemy.gameObject.SetActive(false);
            enemy.onEnemyDeactivated += OnEnemyDeactivated;
            enemyPool.Enqueue(enemy);
        }
    }

    // 꺼내서 갖다쓰기 else 없으면 생성
    private EnemyMover GetEnemyFromPool(EnemyMover prefab)
    {
        EnemyMover enemy;
        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
            enemy.gameObject.SetActive(true);
        }
        else
        {
            enemy = Instantiate(prefab, transform);
            enemy.onEnemyDeactivated += OnEnemyDeactivated;
        }

        return enemy;
    }

    // 생성 가능 여부 확인 및 생성
    private void TrySpawnEnemy(Wave wave)
    {
        if (spawnInfoIndex >= wave.spawnInfos.Count)
            return;
            
        var spawnInfo = wave.spawnInfos[spawnInfoIndex];

        if (runtimeData.CanSpawn(spawnInfo.spawnInterval))
        {
            SpawnEnemy(spawnInfo);
            runtimeData.OnSpawned();
            wave.spawnInfos[spawnInfoIndex].spawnCount--;
            Debug.Log("currentSpawnCount : " + wave.spawnInfos[spawnInfoIndex].spawnCount);

            // 해당 리스트에 존재하는 적 마리 수를 모두 생성한 상태이면 다음 인덱스로 넘어간다.
            if (wave.spawnInfos[spawnInfoIndex].spawnCount == 0)
                spawnInfoIndex++;
        }
    }

    // 적 생성
    private void SpawnEnemy(SpawnInfo spawnInfo)
    {
        // 처음 생성될 때만
        if (enemyPool.Count == 0)
            InitPool(spawnInfo.enemyPrefab.GetComponent<EnemyMover>());

        EnemyMover enemy = GetEnemyFromPool(spawnInfo.enemyPrefab.GetComponent<EnemyMover>());
        activeEnemies.Add(enemy);

        // 랜덤 Z 위치
        Vector3 newPos = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            spawnPoint.position.z + Random.Range(minZPos, maxZPos)
        );

        enemy.transform.position = newPos;
    }

    // 적 비활성화 (이벤트 액션) - 적이 목적지에 도달한 경우 해줘야 하는 작업
    private void OnEnemyDeactivated(EnemyMover enemy)
    {
        enemy.gameObject.SetActive(false);
        activeEnemies.Remove(enemy);
        enemyPool.Enqueue(enemy);
        runtimeData.OnEnemyDeactivated();
    }

    public Transform GetWaypoint(int idx) => waypoints[idx];
}
