public enum FactionType
{
    Player, Enemy
}

public interface IFaction
{
    FactionType Faction { get; }
}
