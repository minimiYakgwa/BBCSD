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
        // "MainCamera" �±׸� ������ �ִ� ������Ʈ Ž�� �� Camera ������Ʈ ���� ����
        // GameObject.FindGameObjectWithTag("mainCamera").GetComponent<Camera>();�� ������.
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true) // ���콺 �����Ͱ� UI���� ���� �� �Ʒ� �ڵ尡 ������� �ʰ� ��.
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� ������ ��
        {
            // ray.origin : ������ ���� ��ġ(ī�޶� ��ġ) ray.direction : ������ �������
            ray = mainCamera.ScreenPointToRay(Input.mousePosition); // ī�޶� ���������� �������� ���콺 ��ǥ�� �������� ������ ray�� ������.

            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // Mathf.Infinity(������ ���� �� �ִ� �ִ� ����)�� ray�� �߻��Ͽ� ���� ������Ʈ�� hit�� ���� + true ��ȯ
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
