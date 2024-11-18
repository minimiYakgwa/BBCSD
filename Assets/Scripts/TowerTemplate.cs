using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public Weapon[] weapon;
    public GameObject followTowerPrefab; // 임시 타워 프리펩

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite; // 타워 별 이미지
        public float damage; // 타워 별 데미지
        public float slow; // 감속 퍼센트 (0.2 == 20%)
        public float buff; // 공격력 증가율 (0.2 == +20%)
        public float rate; // 타워 별 공격속도
        public float range; // 타워 별 공격범위
        public int cost; // 타워 별 구매 비용
        public int sell; //타워 판매 시 획득 골드
    }
}
