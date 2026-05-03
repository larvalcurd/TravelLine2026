namespace Fighters.Models.Races;

public class Elf : IRace
{
    public string Name => "Elf";
    public int Damage => 2;
    public int Health => 90;
    public int Armor => 1;
    public int Initiative => 2;
}
