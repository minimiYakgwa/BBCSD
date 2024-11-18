using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP; // 최대 체력
    private float currentHP; // 현재 체력
    private bool isDie = false; // 현재 체력이 0이라면
    private EnemyMovement enemy; // 적 오브젝트 정보 불러옴
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP; // 현재 체력을 최대 체력으로 설정
        enemy = GetComponent<EnemyMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage) // 데미지를받으면 실행
    {
        if (isDie == true) return; // 현재 체력이 0이라면 return

        currentHP -= damage; // 현재 체력에서 데미지 깎음

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) // 체력이 0 이하라면
        {
            isDie = true; // 죽었다는 isDie를 true로 설정.

            enemy.OnDie(EnemyDestroyType.kill);
        }
    }

    private IEnumerator HitAlphaAnimation() // 적이 데미지를 받았다는
                                            // 깜빡이를 실행.
    {
        Color color = spriteRenderer.color;

        color.a = 0.4f;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);

        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
