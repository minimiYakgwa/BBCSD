using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP; // �ִ� ü��
    private float currentHP; // ���� ü��
    private bool isDie = false; // ���� ü���� 0�̶��
    private EnemyMovement enemy; // �� ������Ʈ ���� �ҷ���
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP; // ���� ü���� �ִ� ü������ ����
        enemy = GetComponent<EnemyMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage) // �������������� ����
    {
        if (isDie == true) return; // ���� ü���� 0�̶�� return

        currentHP -= damage; // ���� ü�¿��� ������ ����

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) // ü���� 0 ���϶��
        {
            isDie = true; // �׾��ٴ� isDie�� true�� ����.

            enemy.OnDie(EnemyDestroyType.kill);
        }
    }

    private IEnumerator HitAlphaAnimation() // ���� �������� �޾Ҵٴ�
                                            // �����̸� ����.
    {
        Color color = spriteRenderer.color;

        color.a = 0.4f;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);

        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
