using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    List<Wave> waves = new List<Wave>();
    int currentWaveIndex;
    public event Action onWaveStarted;
    public event Action onWaveEnded;

    private void Start()
    {
        Wave wave = new Wave();

        wave.waveNum = 1;
        wave.waveEnemyCount = 10;
        wave.spawnInterval = 1f;
        wave.enemies = null;

        currentWaveIndex = wave.waveNum;
    }
}
