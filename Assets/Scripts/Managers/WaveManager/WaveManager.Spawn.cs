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
            // enemy.gameObject.SetActive(true);
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

        // 프리팹 기준으로 반환
        Enemy prefab = enemy.OriginalPrefab;

        if (prefab == null)
        {
            Debug.LogWarning($"{enemy.name}의 OriginalPrefab이 설정되지 않았습니다!");
            return;
        }

        if (!enemyPools.ContainsKey(prefab))
            enemyPools[prefab] = new Queue<Enemy>();

        enemyPools[prefab].Enqueue(enemy);
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

    private void SpawnEnemy(SpawnInfo spawnInfo)
    {
        // 풀 초기화 (최초 한 번만)
        if (!enemyPools.ContainsKey(spawnInfo.enemyPrefab))
            InitPool(spawnInfo.enemyPrefab);

        // 풀에서 꺼내오기
        Enemy enemy = GetEnemyFromPool(spawnInfo.enemyPrefab);
        activeEnemies.Add(enemy);

        enemy.SetOriginalPrefab(spawnInfo.enemyPrefab);

        Vector3 newPos = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            spawnPoint.position.z + Random.Range(minZPos, maxZPos)
        );

        // 자리를 변경한 후에 액티브 활성화 => 기존 자리에 있는 레이캐스트가 바로 발동되는 것을 방지
        enemy.transform.position = newPos;
        enemy.gameObject.SetActive(true);

        Debug.Log("적 생성! 현재 맵에 존재하는 적 = " + activeEnemies.Count);
    }


    // 웨이포인트 반환
    public Transform GetWaypoint(int idx) => waypoints[idx];
}
