using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen; // ü���� �������� �� ������ ��� �ִϸ��̼��� ����ϱ� ���� ����

    [SerializeField]
    private float maxHP = 20; // �ִ� ü��
    private float currentHP; // ���� ü��

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage) // ü�°��� �Լ�
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
        Color color = imageScreen.color; // imageScreen�� ���� ������ ����
        color.a = 0.4f; // ������ ������ ������
        imageScreen.color = color; // ������ ������ �ٽ� imageScreen�� ����

        while (color.a >= 0.0f) // color ������ 0�� �Ǳ������� ��� ���ҽ�Ŵ.
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
        
    }

}
