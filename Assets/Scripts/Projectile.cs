using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;

    public void Setup(Transform target, float damage) // Ÿ���� �߻�ü�� ������ �� Ÿ�ٿ� ���� ������ ����.
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target; // Ÿ�� ������ ���޵�.
        this.damage = damage; // Ÿ���� ���ݷ�
    }

    private void Update()
    {
        if(target != null) //target�� �����ϸ� 
        {
            Vector3 direction = (target.position - transform.position).normalized; // Ÿ�ٰ��� �Ÿ��� ����
            movement2D.MoveTo(direction); // Ÿ������ �̵���Ŵ.
        }
        else // target�� �������
        {
            Destroy(gameObject); // �߻�ü ����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; // ���� �΋H���� �ʾҴٸ�
        if (collision.transform != target) return; // target���� ������ ���� �΋H���� �ʾҴٸ�

        //collision.GetComponent<EnemyMovement>().OnDie(); // target���� ������ ���� �΋H���ٸ� ���� �״� OnDie ȣ��
        collision.GetComponent<EnemyHP>().TakeDamage(damage); // ���� ü���� ��ž�� ���ݷ¸�ŭ ���ҽ�Ű�� �Լ� ����.
        Destroy(gameObject); // �߻�ü ����

    }
}
