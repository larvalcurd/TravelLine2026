namespace Fighters.Models.Classes;

public interface IFighterClass : INamed
{
    public int Damage { get; }
    public int Health { get; }
    public int Initiative { get; }
}