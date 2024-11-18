using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;

    private void Awake()
    {
        // "MainCamera" 태그를 가지고 있는 오브젝트 탐색 후 Camera 컴포넌트 정보 전달
        // GameObject.FindGameObjectWithTag("mainCamera").GetComponent<Camera>();와 동일함.
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true) // 마우스 포인터가 UI위에 있을 때 아래 코드가 실행되지 않게 함.
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 눌렀을 때
        {
            // ray.origin : 광선의 시작 위치(카메라 위치) ray.direction : 광선의 진행방향
            ray = mainCamera.ScreenPointToRay(Input.mousePosition); // 카메라 시작지점을 기준으로 마우스 좌표로 지나가는 광선을 ray에 저장함.

            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // Mathf.Infinity(광선이 가질 수 있는 최대 길이)로 ray를 발사하여 맞은 오브젝트를 hit에 저장 + true 반환
            {
                hitTransform = hit.transform;
                if (hit.transform.CompareTag("Tile"))
                {
                    towerSpawner.SpawnTower(hit.transform);
                }

                else if (hit.transform.CompareTag("Tower"))
                {
                    Debug.Log("hit Tower");
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                towerDataViewer.OffPanel();
            }

            hitTransform = null;
        }
    }
}
