using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f; // �����̴��� �� ������Ʈ�� �Ÿ��� ��Ÿ���� ����
    private Transform targetTransform; // Ÿ���� ��ġ�� �����ϴ�
    private RectTransform rectTransform; // UI�� ��ġ�� �����ϴ� ����

    public void Setup(Transform target)
    {
        // Slider�� �Ѿƴٴ� Ÿ���� ����
        targetTransform = target;
        // RectTransform�� ������Ʈ ���� ��������.
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // ���� �ı��Ǿ� ���� ������ ���� �� UI ����
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        //������Ʈ ��ġ�� ���ŵ� ���Ŀ�(Update) UI�� ������Ʈ ��ġ�� �����ǵ��� �ϱ� ���� LateUpdate ����.

        // ������Ʈ�� ���� ��ǥ�踦 �������� ȭ�鿡���� ��ǥ���� ����.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // ȭ�� ������ ��ǥ + distance��ŭ ������ ��ġ�� slider UI ��ġ�� ������.
        rectTransform.position = screenPosition + distance;
    }
}
