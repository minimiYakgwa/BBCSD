using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {  Cannon = 0, Laser, slow, Buff}
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser,} // AttackToTarget X

public class TowerWeapon : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate towerTemplate;
    [SerializeField]
    private Transform spawnPoint; // 발사체 스폰 지점
    [SerializeField]
    private WeaponType weaponType;
    [Header("Cannon")]
    [SerializeField]
    private GameObject projectilePrefab; // 발사체 프리펩

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer; // 레이저로 사용되는 선
    [SerializeField]
    private Transform hitEffect; // 타격 효과
    [SerializeField]
    private LayerMask targetLayer; // 광선에 부딪히는 레이어 설정
    /*[SerializeField]
    private float attackRate = 0.5f; // 공격 속도
    [SerializeField]
    private float attackRange = 2.0f; // 공격 범위
    [SerializeField]
    private int attackDamage = 1; // 공격력*/
    private int level = 0; // 타워의 레벨
    private WeaponState weaponState = WeaponState.SearchTarget; // 타워 무기의 상태
    private Transform attackTarget = null; // 공격 대상
    private SpriteRenderer spriteRenderer;
    private TowerSpawner towerSpawner;
    private EnemySpawner enemySpawner; // 게임에 존재한 적 정보 획득
    private PlayerGold playerGold;
    private Tile ownerTile;

    private float addedDamage;
    private int buffLevel;
    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public int UpgradeCost => Level < MaxLevel ? towerTemplate.weapon[level + 1].cost : 0;
    public int SellCost => towerTemplate.weapon[level].sell;

    public WeaponType WeaponType => weaponType;

    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int BuffLevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }

    public float Slow => towerTemplate.weapon[level].slow;
    public float Buff => towerTemplate.weapon[level].buff;
    //public float Damage => attackDamage;
     //public float Rate => attackRate;
     //public float Range => attackRange;

    public void SetUp(TowerSpawner towerSpawner, EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.enemySpawner = enemySpawner;
        this.towerSpawner = towerSpawner;
        ChangeState(WeaponState.SearchTarget); // 최초 상태를 ChangeState로 설정함.
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        if (weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser) ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        //이전에 진행중인 상태 종료
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        // 새로운 상태로 진행
        StartCoroutine(weaponState.ToString());
    }

    public void Update()
    {
        if(attackTarget != null) // 타겟이 널이 아니면 타겟을 바라보도록 RotateToTarget 실행
        {
            RotateToTarget();
        }
    }

    void RotateToTarget() // 타워가 타겟을 바라보도록 하는 함수
    {
        float dx = attackTarget.position.x - transform.position.x; // x 변화량 
        float dy = attackTarget.position.y - transform.position.y; // y 변화량

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; // Mathf.Atan2를 통해 각도를 구하고, Mathf.Rad2Deg를 통해 degree로 바꿈
        transform.rotation = Quaternion.Euler(0, 0, degree); // 오일러 각도?를 기준으로 z축 회전시켜줌.
    }


    private IEnumerator SearchTarget()
    {
        while(true)
        {
            /*float closestDistSqr = Mathf.Infinity; // 제일 가까이 있는 적을 찾기 위해 최소 거리를 최대한 설정.

            for(int i = 0; i<enemySpawner.EnemyList.Count; ++i) // EnemySpawner에 있는 EneyList의 모든 적을 한번씩 검사
            {
                // 각각의 적과의 거리를 distance에 저장.
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                // 적과의 거리가 공격범위보다 작고 거리가 현재까지 검사한 적보다 가까우면
                //if(distance <= attackRange && distance <= closestDistSqr)
                if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform; // 현재 가장 가까운 적의 정보를 attackTarget에 저장
                }
            }*/
            attackTarget = FindClosestAttackTarget();

            if(attackTarget != null) // 검사가 끝나고 공격할 적이 남아있다면 
            {
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
                // 가장 가까운 적을 공격함.
                
            }

            yield return null;
        }
    }



    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            // 공격할 적이 없다면 
           /* if(attackTarget == null)
            {
                // 상태 변경
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            // 적이 공격 범위를 벗어났다면
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            //if( distance > attackRange)
            if (distance > towerTemplate.weapon[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }*/
           if (IsPossibleToAttackTarget() == false)
           {
                ChangeState(WeaponState.SearchTarget);
                break;
           }
            // attackRate 시간만큼 대기함
            //yield return new WaitForSeconds(attackRate);
            
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
            
            SpawnProjectile();

        }
    }

    private IEnumerator TryAttackLaser()
    {
        EnableLaser();

        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            SpawnLaser();

            yield return null;
        }
    }
    public void OnBuffAroundTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for (int i =0; i<towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if (weapon.BuffLevel > Level)
            {
                continue;
            }

            if (Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                if (weapon.WeaponType == WeaponType.Cannon || weapon.WeaponType == WeaponType.Laser)
                {
                    weapon.AddedDamage = weapon.Damage * (towerTemplate.weapon[level].buff);
                    weapon.BuffLevel = Level;
                }
            }
        }
    }
    private Transform FindClosestAttackTarget()
    {
        float closestDistSqr = Mathf.Infinity; // 제일 가까이 있는 적을 찾기 위해 최소 거리를 최대한 설정.

        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i) // EnemySpawner에 있는 EneyList의 모든 적을 한번씩 검사
        {
            // 각각의 적과의 거리를 distance에 저장.
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            // 적과의 거리가 공격범위보다 작고 거리가 현재까지 검사한 적보다 가까우면
            //if(distance <= attackRange && distance <= closestDistSqr)
            if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform; // 현재 가장 가까운 적의 정보를 attackTarget에 저장
            }
        }
        return attackTarget;
    }

    private bool IsPossibleToAttackTarget()
    {
        if (attackTarget == null)
        {
            return false;
        }

        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if (distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }
        return true;
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        //clone.GetComponent<Projectile>().Setup(attackTarget, attackDamage); // 생성된 발사체에 대한 정보를 porjectile에 전달.
        float damage = towerTemplate.weapon[level].damage + AddedDamage;
        clone.GetComponent<Projectile>().Setup(attackTarget, damage);
    }

    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }

    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                lineRenderer.SetPosition(0, spawnPoint.position); // 선이 시작지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                hitEffect.position = hit[i].point;
                float damage = towerTemplate.weapon[level].damage + AddedDamage;
                attackTarget.GetComponent<EnemyHP>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }

    public bool Upgrade()
    {
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }
        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;

        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        if (weaponType == WeaponType.Laser) // 무기 속성이 레이저라면
        {
            lineRenderer.startWidth = 0.05f + level * 0.05f; // 레이저 시작 지점의 굵기를 레벨에 따라 조정함.
            lineRenderer.endWidth = 0.05f;
        }

        towerSpawner.OnBuffAllBuffTowers();
        return true;
    }   

    public void Sell()
    {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;

        ownerTile.IsBuildTower = false;

        Destroy(gameObject);
    }


}
