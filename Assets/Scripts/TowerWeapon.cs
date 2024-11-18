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
    private Transform spawnPoint; // �߻�ü ���� ����
    [SerializeField]
    private WeaponType weaponType;
    [Header("Cannon")]
    [SerializeField]
    private GameObject projectilePrefab; // �߻�ü ������

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer; // �������� ���Ǵ� ��
    [SerializeField]
    private Transform hitEffect; // Ÿ�� ȿ��
    [SerializeField]
    private LayerMask targetLayer; // ������ �ε����� ���̾� ����
    /*[SerializeField]
    private float attackRate = 0.5f; // ���� �ӵ�
    [SerializeField]
    private float attackRange = 2.0f; // ���� ����
    [SerializeField]
    private int attackDamage = 1; // ���ݷ�*/
    private int level = 0; // Ÿ���� ����
    private WeaponState weaponState = WeaponState.SearchTarget; // Ÿ�� ������ ����
    private Transform attackTarget = null; // ���� ���
    private SpriteRenderer spriteRenderer;
    private TowerSpawner towerSpawner;
    private EnemySpawner enemySpawner; // ���ӿ� ������ �� ���� ȹ��
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
        ChangeState(WeaponState.SearchTarget); // ���� ���¸� ChangeState�� ������.
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        if (weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser) ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        //������ �������� ���� ����
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        // ���ο� ���·� ����
        StartCoroutine(weaponState.ToString());
    }

    public void Update()
    {
        if(attackTarget != null) // Ÿ���� ���� �ƴϸ� Ÿ���� �ٶ󺸵��� RotateToTarget ����
        {
            RotateToTarget();
        }
    }

    void RotateToTarget() // Ÿ���� Ÿ���� �ٶ󺸵��� �ϴ� �Լ�
    {
        float dx = attackTarget.position.x - transform.position.x; // x ��ȭ�� 
        float dy = attackTarget.position.y - transform.position.y; // y ��ȭ��

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; // Mathf.Atan2�� ���� ������ ���ϰ�, Mathf.Rad2Deg�� ���� degree�� �ٲ�
        transform.rotation = Quaternion.Euler(0, 0, degree); // ���Ϸ� ����?�� �������� z�� ȸ��������.
    }


    private IEnumerator SearchTarget()
    {
        while(true)
        {
            /*float closestDistSqr = Mathf.Infinity; // ���� ������ �ִ� ���� ã�� ���� �ּ� �Ÿ��� �ִ��� ����.

            for(int i = 0; i<enemySpawner.EnemyList.Count; ++i) // EnemySpawner�� �ִ� EneyList�� ��� ���� �ѹ��� �˻�
            {
                // ������ ������ �Ÿ��� distance�� ����.
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                // ������ �Ÿ��� ���ݹ������� �۰� �Ÿ��� ������� �˻��� ������ ������
                //if(distance <= attackRange && distance <= closestDistSqr)
                if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform; // ���� ���� ����� ���� ������ attackTarget�� ����
                }
            }*/
            attackTarget = FindClosestAttackTarget();

            if(attackTarget != null) // �˻簡 ������ ������ ���� �����ִٸ� 
            {
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
                // ���� ����� ���� ������.
                
            }

            yield return null;
        }
    }



    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            // ������ ���� ���ٸ� 
           /* if(attackTarget == null)
            {
                // ���� ����
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            // ���� ���� ������ ����ٸ�
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
            // attackRate �ð���ŭ �����
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
        float closestDistSqr = Mathf.Infinity; // ���� ������ �ִ� ���� ã�� ���� �ּ� �Ÿ��� �ִ��� ����.

        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i) // EnemySpawner�� �ִ� EneyList�� ��� ���� �ѹ��� �˻�
        {
            // ������ ������ �Ÿ��� distance�� ����.
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            // ������ �Ÿ��� ���ݹ������� �۰� �Ÿ��� ������� �˻��� ������ ������
            //if(distance <= attackRange && distance <= closestDistSqr)
            if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform; // ���� ���� ����� ���� ������ attackTarget�� ����
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
        //clone.GetComponent<Projectile>().Setup(attackTarget, attackDamage); // ������ �߻�ü�� ���� ������ porjectile�� ����.
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
                lineRenderer.SetPosition(0, spawnPoint.position); // ���� ��������
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

        if (weaponType == WeaponType.Laser) // ���� �Ӽ��� ���������
        {
            lineRenderer.startWidth = 0.05f + level * 0.05f; // ������ ���� ������ ���⸦ ������ ���� ������.
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
