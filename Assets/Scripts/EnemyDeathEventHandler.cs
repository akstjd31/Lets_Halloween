using System;
using UnityEngine;

public class EnemyDeathEventHandler : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectDistance; // 적을 감지할 거리
    [SerializeField] private LayerMask enemyLayer;      // 적 오브젝트가 속한 레이어

    [Header("References")]
    [SerializeField] private UIManager uiManager;
    
    // 이전에 구독 해제를 안전하게 하는 방법으로 onEnemyDeactivated 이벤트를 사용
    public event Action<Enemy> onEnemyDeactivated; 
    
    private Unit unit;

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }

    private void OnDestroy()
    {
        onEnemyDeactivated = null;
    }

    private void Update()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectDistance, enemyLayer, QueryTriggerInteraction.Collide))
        {
            // 2. 적 오브젝트 감지
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("1111");
                HandleEnemyCollision(hit.collider);
            }
        }
    }

    private void HandleEnemyCollision(Collider enemyCollider)
    {
        Enemy enemy = enemyCollider.GetComponent<Enemy>();
        
        if (enemy == null)
        {
            return;
        }
        
        // 1. 이벤트 호출
        // 구독자들에게 적이 비활성화(처리)되었음을 알림
        onEnemyDeactivated?.Invoke(enemy);

        // 2. 플레이어 데미지 로직
        if (unit is Player player) // C# 7.0 이상의 패턴 매칭을 사용하여 더 깔끔하게 캐스팅
        {
            player.TakeDamage(1);
            
            // uiManager가 할당되어 있는지 확인
            if (uiManager != null)
            {
                uiManager.UpdatePlayerLifeUI(player.CurrentLife);
            }
        }
    }

    // 💡 디버깅을 위해 에디터에서 레이캐스트를 시각화합니다.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * detectDistance);
    }
}