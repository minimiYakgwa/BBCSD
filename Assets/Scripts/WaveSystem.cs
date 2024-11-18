using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves; // 현재 스테이지의 모든 웨이브 정보를 가짐.
    [SerializeField]
    private EnemySpawner enemySpawner; 
    private int currentWaveIndex = -1; // 현재 웨이브 인덱스를 가짐.
    
    // 웨이브 정보 출력을위한 Get 프로퍼티 ( 현재 웨이브, 총 웨이브 )
    public int CurrentWave => currentWaveIndex + 1; 
    public int MaxWave => waves.Length;

    public void StarWave() // 웨이브 시작
    {
        if(enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length-1) // 현재 맵에 적이 없고 웨이브가 아직 남아있을 경우
        {
            currentWaveIndex++; // 현재 웨이브 인덱스 증가
            enemySpawner.StartWave(waves[currentWaveIndex]); // 현재 웨이브를 실행하는 StartWave함수 호출
        }
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}
