using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cusor : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        

        // 오브젝트를 마우스 위치로 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, mouseWorldPosition, moveSpeed * Time.deltaTime);
    }
}
