using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float changingScaleSpeed;
    [SerializeField] [Range(0, 1)] private float inputInterpolation;
    [SerializeField] private Transform targetTransform;
    private Vector3 changeScaleDirection = Vector3.zero;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Move();
    }

    // 충돌 상태 돌입할 때 충돌체 태그가 달려있으면 오브젝트 비활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollidingBody"))
        {
            gameObject.SetActive(false);
        }
    }

    private void Move()
    {
        // 앞으로 멀어지도록 이동 구현
        Debug.Log($"{transform.position.x}, {transform.position.y}, {transform.position.z}");
        //transform.position += moveSpeed * Vector3.forward * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
        transform.localScale -= changingScaleSpeed * changeScaleDirection * Time.deltaTime;
    }

    private void Init()
    {
        changeScaleDirection += Vector3.up;
        changeScaleDirection += Vector3.right;
    }
}
