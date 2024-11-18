using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab; // 적 프리펩
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // 적 체력을 나타내는 Slider UI 프리펩
    //[SerializeField]
    //private float spawnTime; // 적 생성 주기
    [SerializeField]
    private GameObject towerSpawner;
    [SerializeField]
    private PlayerGold playerGold;

    [SerializeField]
    private Transform canvasTransform; // UI를 표현하는 Canvas 오브젝트의 Transform
    [SerializeField]
    private Transform[] wayPoints; // 이동 지점
    [SerializeField]
    private PlayerHP playerHP; // 플레이어 체력
    private Wave currentWave; // 현재 웨이브 정보
    private int currentEnemyCount; // 현재 웨이브에 남아있는 적의 개수. 초기엔 max로 설정되고 적이 죽을때마다 -1하게 됨.
    private List<EnemyMovement> enemyList; // 현재 맵에 존재하는 적 정보

    public List<EnemyMovement> EnemyList => enemyList; // inspector에서 저장한 EnemyList를 enemyList에 저장.

    // 현재 웨이브의 남아있는 적과 최대 적의 개수를 담고 있음
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        enemyList = new List<EnemyMovement>();
        
        //StartCoroutine("SpawnEnemy"); // 적 생성 함수 호출

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
        while(spawnEnemyCount < currentWave.maxEnemyCount) // 현재 웨이브에서 생성되어야 할 적의 개수만큼 생성함.
        {
            // 적 오브젝트 생성
            //GameObject clone = Instantiate(enemyPrefab);

            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            EnemyMovement enemy = clone.GetComponent<EnemyMovement>();

            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy); // 생성된 적의 정보를 enemyList에 저장

            SpawnEnemyHPSlider(clone); // 적 체력을 표현해주는 UI 생성.

            spawnEnemyCount++;

            // 스폰 시간동안 대기함.
            //yield return new WaitForSeconds(spawnTime);
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, EnemyMovement enemy, int gold)
    {
        if (type == EnemyDestroyType.Arrive) // 아직 살아있다면
        {
            playerHP.TakeDamage(1); // 체력 감소 함수 실행.
        }
        else if (type == EnemyDestroyType.kill) // 적이 플레이어에 의해 죽었을 때
        {
            playerGold.CurrentGold += gold; // 플레이어의 골드 증가
        }
        currentEnemyCount--; // 적이 죽으면 현재 웨이브의 생존 적 개수가 감소(UI 표시용)
        if (currentEnemyCount == 0) towerSpawner.SetActive(true);
        // 리스트에서 죽은 적에 대한 정보 삭제
        enemyList.Remove(enemy);
        // 죽은 적 오브젝트 삭제
        Destroy(enemy.gameObject);
    }

    public void SpawnEnemyHPSlider(GameObject enemy)
    {
        // 적의 체력을 표시하는 slider 생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // 생성된 slider를 canvas의 자식으로 설정(UI는 canvas의 자식 오브젝트로 설정해야 보임)
        sliderClone.transform.SetParent(canvasTransform);
        // slider의 크기를 1로 설정.
        sliderClone.transform.localScale = Vector3.one;
        // slider가 쫓아다닐 대상을 본인으로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // 슬라이더가 본인의 체력을 표시하도록 설정.
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }

}
