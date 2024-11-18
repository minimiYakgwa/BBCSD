using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate; // 타워 정보를 담고 있음
    //[SerializeField]
    //private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner; // 현재 존재하는 적 리스트 정보를 불러옴
    //[SerializeField]
    //private int towerBuildGold = 50; // 타워 건설에 필요한 골드를 저장.
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;
    private int towerType; // 타워 속성

    public void ReadyToSpawnTower(int type)
    {
        towerType = type;
        if (isOnTowerButton == true) // 버튼을 여러번 눌렀을 때 아래 내용이 실행되지 않도록 함.
        {
            return;
        }
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        isOnTowerButton = true;

        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);

        StartCoroutine("OnTowerCancleSystem");

    }

    public void SpawnTower(Transform tileTransform)
    {
        if (isOnTowerButton == false)
        {
            return;
        }
        /*if (towerTemplate.weapon[0].cost > playerGold.CurrentGold) // 타원 건설에 필요한 골드가 부족하다면
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }*/
        Tile tile = tileTransform.GetComponent<Tile>();

        if(tile.IsBuildTower == true) // 현재 타일에 타워가 이미 건설되어있다면
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        isOnTowerButton = false;
        tile.IsBuildTower = true; // 타일에 타워가 없다면 건설한 후 건설되었다고 true 반환
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost; // 건설한 타워값만큼 플레이어 골드에서 차감
        Vector3 position = tileTransform.position + Vector3.back; // 건설한 타워가 z축이 앞에오게 해 먼저 선택될 수 있게 함.
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        clone.GetComponent<TowerWeapon>().SetUp(this, enemySpawner, playerGold, tile);

        OnBuffAllBuffTowers();

        Destroy(followTowerClone); // 타워를 배치했기 때문에 프리펩 삭제.

        StopCoroutine("OnTowerCancleSystem");
    }

    private IEnumerator OnTowerCancleSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }

            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");


        for (int i = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();
            Debug.Log(i);
            Debug.Log(weapon.WeaponType);
            if (weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
