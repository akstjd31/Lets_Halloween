using UnityEngine;

// 스폰 관리
public partial class WaveManager
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float minZPos;
    [SerializeField] private float maxZPos;

    private void SpawnEnemy(Wave wave)
    {
        var spawnInfo = wave.spawnInfos[spawnInfoListIndex];
        Vector3 newPos = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            spawnPoint.position.z + UnityEngine.Random.Range(minZPos, maxZPos)
        );

        EnemyMover enemy = Instantiate(spawnInfo.enemyPrefab, newPos, Quaternion.identity)
            .GetComponent<EnemyMover>();

        enemy.onEnemyDeactivated += OnEnemyDeactivated;
    }

    private void OnEnemyDeactivated(EnemyMover enemy)
    {
        enemy.onEnemyDeactivated -= OnEnemyDeactivated;
        deactiveCount++;
    }

    public Transform GetWaypoint(int index) => waypoints[index];
}
