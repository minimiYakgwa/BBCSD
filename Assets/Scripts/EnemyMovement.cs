using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType {  kill=0, Arrive }

public class EnemyMovement : MonoBehaviour
{
    private int wayPointCount; // 이동 지점 개수
    private Transform[] wayPoints; // 이동 지점 정보
    int currentIndex = 0; // 현재 목표지점 인덱스
    Movement2D movement2D; // 오브젝트 이동 제어
    private EnemySpawner enemySpawner; // 생성된 적에 대한 정보

    [SerializeField]
    private int gold = 10;

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        // 적 이동 경로 wayPoint 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // 적 위치를 첫 번째 wayPoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        // 적 이동/목표지점 설정 코루틴 함수 실행
        StartCoroutine("OnMove");

    }

    private IEnumerator OnMove()
    {
        // 다음 이동방향 설정
        NextMoveTo();

        while (true)
        {   
            // 적을 계속 회전시킴
            transform.Rotate(Vector3.forward * 10);

            //  적의 현재 위치와 목표 지점 사이의 거리가 movement2D.MoveSpeed보다 작을 때 NextMoveTo()실행.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    void NextMoveTo()
    {
        // 아직 이동할 wayPoint가 있는경우
        if (currentIndex < wayPointCount - 1)
        {
            // 적의 위치를 목표위치로 설정
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }

        // 이동할 wayPoint가 없다면
        else
        {
            gold = 0;
            
            //Destroy(gameObject);
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        // EnemySpawner에서 적 정보를 삭제해야 하므로 바로 Destroy하기 보단 DestroyEnemy함수를 호출하여 적 정보를 삭제할 시간을 줌.
        enemySpawner.DestroyEnemy(type, this, gold);
    }

}
