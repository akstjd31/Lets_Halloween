using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WaveManager
{
    [Serializable]
    public class Wave
    {
        public int waveNum;                         
        public List<SpawnInfo> spawnInfos = new();
    }

    [Serializable]
    public class SpawnInfo
    {
        public Enemy enemyPrefab;
        public int spawnCount;
        public float spawnInterval;
    }
}
