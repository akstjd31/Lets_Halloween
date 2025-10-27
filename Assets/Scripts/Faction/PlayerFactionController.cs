using UnityEngine;

public class PlayerFactionController : MonoBehaviour, IFactionController
{
    public FactionType Faction { get; }
    public virtual void Initialize()
    {
        Debug.Log("플레이어 선택됨!");
        Debug.Log("초기 세팅 중...");
    }

    public virtual void StartGame()
    {
        Debug.Log("플레이어 진영으로 게임 시작!");
    }

    public virtual void Update()
    {

    }
    
    public virtual void EndGame()
    {
        
    }
}
