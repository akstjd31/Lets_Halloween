using System.Collections.Generic;
using UnityEngine;

public partial class WaveManager
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform waypoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float minZPos;
    [SerializeField] private float maxZPos;
    [SerializeField] private int poolSize;
    [SerializeField] private int spawnInfoIndex;

    [Header("Runtime")]
    [SerializeField] private List<Enemy> activeEnemies = new List<Enemy>();
    [SerializeField] private List<Transform> waypoints;

    // 프리팹별 오브젝트 풀
    private Dictionary<Enemy, Queue<Enemy>> enemyPools = new();

    // 풀 초기화 (처음 한 번만)
    private void InitPool(Enemy prefab)
    {
        if (!enemyPools.ContainsKey(prefab))
            enemyPools[prefab] = new Queue<Enemy>();

        for (int i = 0; i < poolSize; i++)
        {
            Enemy enemy = Instantiate(prefab, transform);
            enemy.gameObject.SetActive(false);
            enemyPools[prefab].Enqueue(enemy);
        }
    }

    // 풀에서 꺼내기 (없으면 새로 생성)
    private Enemy GetEnemyFromPool(Enemy prefab)
    {
        // 해당 프리팹용 풀이 없으면 새로 생성
        if (!enemyPools.TryGetValue(prefab, out var pool))
        {
            pool = new Queue<Enemy>();
            enemyPools[prefab] = pool;
        }

        Enemy enemy;
        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
            enemy.gameObject.SetActive(true);
        }
        else
        {
            enemy = Instantiate(prefab, transform);
        }

        return enemy;
    }

    // 적을 풀로 반환
    private void ReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        if (!enemyPools.ContainsKey(enemy))
            enemyPools[enemy] = new Queue<Enemy>();

        enemyPools[enemy].Enqueue(enemy);
    }

    // 적 생성 시도
    private void TrySpawnEnemy(Wave wave)
    {
        if (spawnInfoIndex >= wave.spawnInfos.Count)
            return;

        var spawnInfo = wave.spawnInfos[spawnInfoIndex];

        if (runtimeData.CanSpawn(spawnInfo.spawnInterval))
        {
            SpawnEnemy(spawnInfo);
            runtimeData.OnSpawned();
            spawnInfo.spawnCount--;

            // 남은 스폰 수 0이면 다음 스폰정보로
            if (spawnInfo.spawnCount <= 0)
                spawnInfoIndex++;
        }
    }

    // 🔹 적 실제 생성 로직
    private void SpawnEnemy(SpawnInfo spawnInfo)
    {
        Enemy prefab = spawnInfo.enemyPrefab;

        // 해당 프리팹의 풀이 비어 있으면 초기화
        if (!enemyPools.ContainsKey(prefab) || enemyPools[prefab].Count == 0)
            InitPool(prefab);

        Enemy enemy = GetEnemyFromPool(prefab);
        activeEnemies.Add(enemy);

        // 랜덤 Z 위치
        Vector3 newPos = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            spawnPoint.position.z + Random.Range(minZPos, maxZPos)
        );

        enemy.transform.position = newPos;
    }

    // 🔹 웨이포인트 반환
    public Transform GetWaypoint(int idx) => waypoints[idx];
}
