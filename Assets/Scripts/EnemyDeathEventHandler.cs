using System;
using UnityEngine;

public class EnemyDeathEventHandler : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectDistance; // 적을 감지할 거리
    [SerializeField] private LayerMask enemyLayer;      // 적 오브젝트가 속한 레이어
    [SerializeField] private LayerMask playerEnemyUnitLayer;
    int layerMask;

    [Header("References")]
    [SerializeField] private UIManager uiManager;

    // 이전에 구독 해제를 안전하게 하는 방법으로 onEnemyDeactivated 이벤트를 사용
    public event Action<Enemy> onEnemyDeactivated;

    private Unit unit;

    private void Awake()
    {
        uiManager = GameManager.FindFirstObjectByType<UIManager>();
    }

    private void Start()
    {
        layerMask = enemyLayer | playerEnemyUnitLayer;
    }

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

        if (Physics.Raycast(transform.position, transform.forward, out hit, detectDistance, layerMask, QueryTriggerInteraction.Collide))
        {
            if (GameManager.Instance.gameOptionData.factionType.Equals(FactionType.Enemy))
            {
                if (hit.collider.CompareTag("EnemyUnit"))
                {
                    HandleEnemyCollision(hit.collider);
                    GameManager.Instance.UpdateState(GameState.Result);
                    GameManager.Instance.isGameClear = true;
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    HandleEnemyCollision(hit.collider);
                }
                // 보스일 떄
                else
                {

                }
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

        onEnemyDeactivated?.Invoke(enemy);

        if (unit is Player player)
        {
            player.TakeDamage(1);

            if (uiManager != null)
            {
                //uiManager.UpdatePlayerLifeUI(player.maxLife, player.CurrentLife);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * detectDistance);
    }
}