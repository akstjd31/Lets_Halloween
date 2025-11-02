
//플레이어 유닛 혹은 투사체가 가지는 고유 
using UnityEngine;

public enum PassiveSkill
{
    None, Slow, Stun ,DoubleAttack
}
//상점에서 사서 사용하는 상태이상
public enum ActiveStatusEffect
{
    PowerUp,AttackSpeedUp, MoveSpeedUp, StatusImmunity
}

public interface IUnitPassiveSkill
{
    
    //상태이상 적용
    void ApplyStatusEffect(PassiveSkill type);

}

public interface IStatusEffectSkill
{
    //상태이상 적용시간과 적용확률 받아와서 상태이상 적용
    void ApplyStatusEffect(ActiveStatusEffect type, float effectTime, int useGold);
}