using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen; // 체력이 감소했을 떄 빨간색 경고 애니메이션을 출력하기 위한 변수

    [SerializeField]
    private float maxHP = 20; // 최대 체력
    private float currentHP; // 현재 체력

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage) // 체력감소 함수
    {
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0)
        {

        }
    }

    public IEnumerator HitAlphaAnimation()
    {
        Color color = imageScreen.color; // imageScreen의 현재 색상을 저장
        color.a = 0.4f; // 색상의 투명도를 조절함
        imageScreen.color = color; // 조절한 색상을 다시 imageScreen에 저장

        while (color.a >= 0.0f) // color 투명도가 0이 되기전까지 계속 감소시킴.
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
        
    }

}
