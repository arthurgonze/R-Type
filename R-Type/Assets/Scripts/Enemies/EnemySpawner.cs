﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    Coroutine spawnCoroutine;


    // Use this for initialization
    IEnumerator Spawn()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    public void SetLooping(bool toggle)
    {
        looping = toggle;
    }

    private IEnumerator SpawnAllWaves()
    {
        for(int waveIndex = 0; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return  StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWaypoints()[0].transform.position, Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null  && collision.gameObject != null && collision.gameObject.tag == "EnemieSpawnTrigger")
        {
            spawnCoroutine = StartCoroutine(Spawn());
        }
    }
}
