using System.Collections.Generic;
using UnityEngine.PlayerLoop;

public class Wave
{
    public int waveNum;            // 웨이브 번호
    public int waveEnemyCount;     // 웨이브 당 생성될 적 수
    public float spawnInterval;    // 스폰 주기
    public List<Enemy> enemies;    // 해당 웨이브에 소환될 적 리스트
}
