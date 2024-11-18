using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate; // Ÿ�� ������ ��� ����
    //[SerializeField]
    //private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner; // ���� �����ϴ� �� ����Ʈ ������ �ҷ���
    //[SerializeField]
    //private int towerBuildGold = 50; // Ÿ�� �Ǽ��� �ʿ��� ��带 ����.
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;
    private int towerType; // Ÿ�� �Ӽ�

    public void ReadyToSpawnTower(int type)
    {
        towerType = type;
        if (isOnTowerButton == true) // ��ư�� ������ ������ �� �Ʒ� ������ ������� �ʵ��� ��.
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
        /*if (towerTemplate.weapon[0].cost > playerGold.CurrentGold) // Ÿ�� �Ǽ��� �ʿ��� ��尡 �����ϴٸ�
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }*/
        Tile tile = tileTransform.GetComponent<Tile>();

        if(tile.IsBuildTower == true) // ���� Ÿ�Ͽ� Ÿ���� �̹� �Ǽ��Ǿ��ִٸ�
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        isOnTowerButton = false;
        tile.IsBuildTower = true; // Ÿ�Ͽ� Ÿ���� ���ٸ� �Ǽ��� �� �Ǽ��Ǿ��ٰ� true ��ȯ
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost; // �Ǽ��� Ÿ������ŭ �÷��̾� ��忡�� ����
        Vector3 position = tileTransform.position + Vector3.back; // �Ǽ��� Ÿ���� z���� �տ����� �� ���� ���õ� �� �ְ� ��.
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        clone.GetComponent<TowerWeapon>().SetUp(this, enemySpawner, playerGold, tile);

        OnBuffAllBuffTowers();

        Destroy(followTowerClone); // Ÿ���� ��ġ�߱� ������ ������ ����.

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
