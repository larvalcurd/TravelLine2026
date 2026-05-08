namespace Fighters.Models.Classes;

public class Warrior : IFighterClass
{
    public string Name => "Warrior";
    public int Damage => 15;
    public int Health => 100;
    public int Initiative => 2;
}