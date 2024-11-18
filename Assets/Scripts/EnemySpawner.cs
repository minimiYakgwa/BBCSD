using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab; // �� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // �� ü���� ��Ÿ���� Slider UI ������
    //[SerializeField]
    //private float spawnTime; // �� ���� �ֱ�
    [SerializeField]
    private GameObject towerSpawner;
    [SerializeField]
    private PlayerGold playerGold;

    [SerializeField]
    private Transform canvasTransform; // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    [SerializeField]
    private Transform[] wayPoints; // �̵� ����
    [SerializeField]
    private PlayerHP playerHP; // �÷��̾� ü��
    private Wave currentWave; // ���� ���̺� ����
    private int currentEnemyCount; // ���� ���̺꿡 �����ִ� ���� ����. �ʱ⿣ max�� �����ǰ� ���� ���������� -1�ϰ� ��.
    private List<EnemyMovement> enemyList; // ���� �ʿ� �����ϴ� �� ����

    public List<EnemyMovement> EnemyList => enemyList; // inspector���� ������ EnemyList�� enemyList�� ����.

    // ���� ���̺��� �����ִ� ���� �ִ� ���� ������ ��� ����
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        enemyList = new List<EnemyMovement>();
        
        //StartCoroutine("SpawnEnemy"); // �� ���� �Լ� ȣ��

    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;
        towerSpawner.SetActive(false);

        // while (true)
        while(spawnEnemyCount < currentWave.maxEnemyCount) // ���� ���̺꿡�� �����Ǿ�� �� ���� ������ŭ ������.
        {
            // �� ������Ʈ ����
            //GameObject clone = Instantiate(enemyPrefab);

            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            EnemyMovement enemy = clone.GetComponent<EnemyMovement>();

            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy); // ������ ���� ������ enemyList�� ����

            SpawnEnemyHPSlider(clone); // �� ü���� ǥ�����ִ� UI ����.

            spawnEnemyCount++;

            // ���� �ð����� �����.
            //yield return new WaitForSeconds(spawnTime);
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, EnemyMovement enemy, int gold)
    {
        if (type == EnemyDestroyType.Arrive) // ���� ����ִٸ�
        {
            playerHP.TakeDamage(1); // ü�� ���� �Լ� ����.
        }
        else if (type == EnemyDestroyType.kill) // ���� �÷��̾ ���� �׾��� ��
        {
            playerGold.CurrentGold += gold; // �÷��̾��� ��� ����
        }
        currentEnemyCount--; // ���� ������ ���� ���̺��� ���� �� ������ ����(UI ǥ�ÿ�)
        if (currentEnemyCount == 0) towerSpawner.SetActive(true);
        // ����Ʈ���� ���� ���� ���� ���� ����
        enemyList.Remove(enemy);
        // ���� �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }

    public void SpawnEnemyHPSlider(GameObject enemy)
    {
        // ���� ü���� ǥ���ϴ� slider ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // ������ slider�� canvas�� �ڽ����� ����(UI�� canvas�� �ڽ� ������Ʈ�� �����ؾ� ����)
        sliderClone.transform.SetParent(canvasTransform);
        // slider�� ũ�⸦ 1�� ����.
        sliderClone.transform.localScale = Vector3.one;
        // slider�� �Ѿƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // �����̴��� ������ ü���� ǥ���ϵ��� ����.
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }

}
