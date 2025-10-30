
//플레이어 유닛 혹은 투사체가 가지는 고유 상태이상
public enum PassiveStatusEffect
{
    Slow, Stun ,Blood
}
//상점에서 사서 사용하는 상태이상
public enum ActiveStatusEffect
{
    PowerUp,AttackSpeedUp, MoveSpeedUp, StatusImmunity
}

public interface IStatusEffectUnitUnit
{
    //상태이상 적용시간과 적용확률 받아와서 상태이상 적용
    void ApplyStatusEffect(PassiveStatusEffect type, float effectTime);
}

public interface IStatusEffectSkill
{
    //상태이상 적용시간과 적용확률 받아와서 상태이상 적용
    void ApplyStatusEffect(ActiveStatusEffect type, float effectTime, int useGold);
}