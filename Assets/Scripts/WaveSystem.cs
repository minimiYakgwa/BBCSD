using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves; // ���� ���������� ��� ���̺� ������ ����.
    [SerializeField]
    private EnemySpawner enemySpawner; 
    private int currentWaveIndex = -1; // ���� ���̺� �ε����� ����.
    
    // ���̺� ���� ��������� Get ������Ƽ ( ���� ���̺�, �� ���̺� )
    public int CurrentWave => currentWaveIndex + 1; 
    public int MaxWave => waves.Length;

    public void StarWave() // ���̺� ����
    {
        if(enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length-1) // ���� �ʿ� ���� ���� ���̺갡 ���� �������� ���
        {
            currentWaveIndex++; // ���� ���̺� �ε��� ����
            enemySpawner.StartWave(waves[currentWaveIndex]); // ���� ���̺긦 �����ϴ� StartWave�Լ� ȣ��
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
