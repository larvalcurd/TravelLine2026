namespace Fighters.Models.Classes;

public class Rogue : IFighterClass
{
    public string Name => "Rogue";
    public int Damage => 25;
    public int Health => 70;
    public int Initiative => 3;
}