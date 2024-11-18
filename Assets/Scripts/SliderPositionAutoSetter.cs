using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f; // 슬라이더와 적 오브젝트의 거리를 나타내는 변수
    private Transform targetTransform; // 타겟의 위치를 제어하는
    private RectTransform rectTransform; // UI의 위치를 제어하는 변수

    public void Setup(Transform target)
    {
        // Slider가 쫓아다닐 타겟을 설정
        targetTransform = target;
        // RectTransform의 컴포넌트 정보 가져오기.
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // 적이 파괴되어 쫓을 이유가 없을 때 UI 삭제
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        //오브젝트 위치가 갱신된 이후에(Update) UI가 오브젝트 위치에 설정되도록 하기 위해 LateUpdate 실행.

        // 오브젝트의 월드 좌표계를 기준으로 화면에서의 좌표값을 구함.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // 화면 내에서 좌표 + distance만큼 떨어진 위치를 slider UI 위치로 설정함.
        rectTransform.position = screenPosition + distance;
    }
}
