using UnityEngine;

public class EnemyFactionController : MonoBehaviour, IFactionController
{
    public FactionType Faction { get; }
    public virtual void Initialize()
    {
        Debug.Log("적 선택됨!");
        Debug.Log("초기 세팅 중...");

    }

    public virtual void StartGame()
    {
        Debug.Log("적 진영으로 게임 시작!");
    }

    public virtual void Update()
    {

    }
    
    public virtual void EndGame()
    {
        
    }
}
