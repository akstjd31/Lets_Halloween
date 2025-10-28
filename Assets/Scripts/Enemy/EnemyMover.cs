using System;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    // 기본적으로 생성된 적은 맵의 이동 경로에 따라 이동하게 됨
    public event Action onEnemyDeactivated;
    [SerializeField] private int currentIndex;      // 현재 바라보는 스팟지점 인덱스
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private void Start()
    {
        currentIndex = 0;
    }


    void Update()
    {
        MoveToNextTarget();
    }

    // 이동 & 회전
    private void MoveToNextTarget()
    {
        Transform target = WaveManager.Instance.GetWaypoint(currentIndex);
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, moveSpeed * Time.deltaTime);

        Vector3 direction = (target.position - this.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(this.transform.position, target.position) < 1f)
            currentIndex++;
    }

    private void OnEnable() 
    {
        onEnemyDeactivated = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPoint"))
        {
            onEnemyDeactivated?.Invoke();
            this.gameObject.SetActive(false);
        }
           
    }
}
