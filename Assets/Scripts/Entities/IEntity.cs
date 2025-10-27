
using UnityEngine;

// 적, 플레이어, 유닛 등 상속받을 공통 인터페이스
public interface IEntity
{
    string Name { get; set; }
    int ID { get; set; }

    void Initialize(string name, int id);
}
