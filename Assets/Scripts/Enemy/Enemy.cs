using UnityEngine;

public class Enemy : Unit, IFaction
{
    // 상수
    // 변수
    // 프로퍼티
    public FactionType Faction => FactionType.Enemy;
    public Enemy OriginalPrefab { get; private set; }

    // 생성자
    // 메소드
    public override void Initialize(string name, int id) => base.Initialize(name, id);
    public void SetOriginalPrefab(Enemy prefab)
    {
        OriginalPrefab = prefab;
    }
}
