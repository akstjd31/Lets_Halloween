
//상태이상
public enum StatusEffect
{
    Slow, Stun
}

public interface IStatusEffect
{
    //상태이상 적용시간과 적용확률 받아와서 상태이상 적용
    void ApplyStatusEffect(StatusEffect type, float effectTime , float statusEffectPercent);

}
