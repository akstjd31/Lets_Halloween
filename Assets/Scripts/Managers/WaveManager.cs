// using System;
// using System.Collections.Generic;
// using UnityEngine;

// public class WaveManager : Singleton<WaveManager>
// {
//     [Serializable]
//     public class Wave
//     {
//         public int waveNum;                         // 웨이브 번호
//         public List<SpawnInfo> spawnInfos = new();
//     }

//     [Serializable]
//     public class SpawnInfo
//     {
//         public Enemy enemyPrefab;
//         public int spawnCount;
//         public float spawnInterval;
//     }

//     [SerializeField] List<Wave> waves = new List<Wave>();
//     [SerializeField] private Transform spawnPoint;
//     [SerializeField] private Transform[] waypoints;
//     [SerializeField] private Transform endPoint;
//     [SerializeField] float minZPos;
//     [SerializeField] float maxZPos;
//     public event Action onWaveStarted;  // 웨이브 시작 시 이벤트
//     public event Action onWaveEnded;    // 웨이브 종료 시 이벤트
//     public bool isWaveRunning;          // 웨이브 진행 여부
//     public int currentWaveIndex;       // 현재 진행중인 웨이브
//     public int deactiveCount;          // 비활성화된 적 마리 수 체크
//     private int totalSpawnCount;       // 웨이브별 생성되어야 하는 적의 총 마리 수
//     private int spawnedCount;          // 생성된 적 카운트
//     private int spawnInfoListIndex;    // 스폰정보가 담긴 리스트 인덱스
//     private float spawnTimer;          // 생성 주기 관련 타이머

//     // private Queue<EnemyMover> enemyPool = new Queue<EnemyMover>();
//     // [SerializeField] private int poolSize;

//     // 임시 데이터
//     private void Start()
//     {
//         currentWaveIndex = 0;
        
//         onWaveStarted += HandleWaveStarted;
//         onWaveEnded += HandleWaveEnded;
//     }

//     public void StartWave()
//     {
//         if (currentWaveIndex >= waves.Count)
//             return;

//         isWaveRunning = true;
        
//         deactiveCount = 0;
//         spawnedCount = 0;
//         spawnInfoListIndex = 0;
        
//         // 현재 웨이브에 존재하는 몹들의 총 개수를 계산
//         foreach (SpawnInfo spawnInfo in waves[currentWaveIndex].spawnInfos)
//             totalSpawnCount += spawnInfo.spawnCount;

//         onWaveStarted?.Invoke();
//     }

//     public void RunWave()
//     {
//         if (!isWaveRunning)
//             return;

//         Wave wave = waves[currentWaveIndex];
//         spawnTimer += Time.deltaTime;

//         // 이번 웨이브에 생성되어야 하는 총 몹의 개수가 0이면서 
//         float spawnInverval = waves[currentWaveIndex].spawnInfos[spawnInfoListIndex].spawnInterval;
//         if (spawnedCount < totalSpawnCount && spawnTimer >= spawnInverval)
//         {
//             spawnTimer = 0f;
//             SpawnEnemy(wave);
//             spawnedCount++;
//         }

//         // 이 부분 수정해야 됨. (웨이브 종료 시점??)
//         if (deactiveCount >= totalSpawnCount)
//         {
//             EndWave();
//         }
//     }

//     public void EndWave()
//     {
//         isWaveRunning = false;
//         onWaveEnded?.Invoke();

//         currentWaveIndex++;
//         GameManager.Instance.InitPreparingTime();
//     }

//     private void InitPool()
//     {
//         if (waves.Count == 0)
//             return;

//         //EnemyMover prefab = waves[currentWaveIndex].enm
//     }

//     // 웨이브 시작 핸들러
//     private void HandleWaveStarted()
//     {
//         GameManager.Instance.UpdateState(GameState.Battle);
//     }

//     // 웨이브 종료 핸들러
//     private void HandleWaveEnded()
//     {
//         GameManager.Instance.UpdateState(GameState.Prepare);
//     }

//     // 파괴될 때 구독 해제
//     private void OnDestroy()
//     {
//         onWaveStarted -= HandleWaveStarted;
//         onWaveEnded -= HandleWaveEnded;
//     }

//     private void SpawnEnemy(Wave wave)
//     {
//         Vector3 newPos = new Vector3(
//             spawnPoint.position.x,
//             spawnPoint.position.y,
//             spawnPoint.position.z + UnityEngine.Random.Range(minZPos, maxZPos)

//         );

//         EnemyMover enemy = Instantiate(waves[currentWaveIndex].spawnInfos[spawnInfoListIndex].enemyPrefab, newPos, Quaternion.identity).GetComponent<EnemyMover>();

//         enemy.onEnemyDeactivated += OnEnemyDeactivated;
//     }

//     private void OnEnemyDeactivated(EnemyMover enemy)
//     {
//         enemy.onEnemyDeactivated -= OnEnemyDeactivated;
//         //enemyPool.Return(enemy);
//         deactiveCount++;
//     }

//     public Transform GetWaypoint(int index) => waypoints[index];
// }
