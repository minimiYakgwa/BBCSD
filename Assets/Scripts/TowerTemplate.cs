using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public Weapon[] weapon;
    public GameObject followTowerPrefab; // �ӽ� Ÿ�� ������

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite; // Ÿ�� �� �̹���
        public float damage; // Ÿ�� �� ������
        public float slow; // ���� �ۼ�Ʈ (0.2 == 20%)
        public float buff; // ���ݷ� ������ (0.2 == +20%)
        public float rate; // Ÿ�� �� ���ݼӵ�
        public float range; // Ÿ�� �� ���ݹ���
        public int cost; // Ÿ�� �� ���� ���
        public int sell; //Ÿ�� �Ǹ� �� ȹ�� ���
    }
}
