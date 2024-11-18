using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType {  kill=0, Arrive }

public class EnemyMovement : MonoBehaviour
{
    private int wayPointCount; // �̵� ���� ����
    private Transform[] wayPoints; // �̵� ���� ����
    int currentIndex = 0; // ���� ��ǥ���� �ε���
    Movement2D movement2D; // ������Ʈ �̵� ����
    private EnemySpawner enemySpawner; // ������ ���� ���� ����

    [SerializeField]
    private int gold = 10;

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        // �� �̵� ��� wayPoint ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // �� ��ġ�� ù ��° wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        // �� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");

    }

    private IEnumerator OnMove()
    {
        // ���� �̵����� ����
        NextMoveTo();

        while (true)
        {   
            // ���� ��� ȸ����Ŵ
            transform.Rotate(Vector3.forward * 10);

            //  ���� ���� ��ġ�� ��ǥ ���� ������ �Ÿ��� movement2D.MoveSpeed���� ���� �� NextMoveTo()����.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    void NextMoveTo()
    {
        // ���� �̵��� wayPoint�� �ִ°��
        if (currentIndex < wayPointCount - 1)
        {
            // ���� ��ġ�� ��ǥ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }

        // �̵��� wayPoint�� ���ٸ�
        else
        {
            gold = 0;
            
            //Destroy(gameObject);
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        // EnemySpawner���� �� ������ �����ؾ� �ϹǷ� �ٷ� Destroy�ϱ� ���� DestroyEnemy�Լ��� ȣ���Ͽ� �� ������ ������ �ð��� ��.
        enemySpawner.DestroyEnemy(type, this, gold);
    }

}
