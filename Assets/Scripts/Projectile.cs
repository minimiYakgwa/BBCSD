using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;

    public void Setup(Transform target, float damage) // 타워가 발사체를 생성할 때 타겟에 대한 정보가 전달.
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target; // 타겟 정보가 전달됨.
        this.damage = damage; // 타워의 공격력
    }

    private void Update()
    {
        if(target != null) //target이 존재하면 
        {
            Vector3 direction = (target.position - transform.position).normalized; // 타겟과의 거리를 측정
            movement2D.MoveTo(direction); // 타겟으로 이동시킴.
        }
        else // target이 사라지면
        {
            Destroy(gameObject); // 발사체 제거
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; // 적과 부딫히지 않았다면
        if (collision.transform != target) return; // target으로 설정한 적과 부딫히지 않았다면

        //collision.GetComponent<EnemyMovement>().OnDie(); // target으로 설정된 적과 부딫혔다면 적이 죽는 OnDie 호출
        collision.GetComponent<EnemyHP>().TakeDamage(damage); // 적의 체력을 포탑의 공격력만큼 감소시키는 함수 실행.
        Destroy(gameObject); // 발사체 삭제

    }
}
