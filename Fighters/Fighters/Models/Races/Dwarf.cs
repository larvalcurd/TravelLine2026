namespace Fighters.Models.Races
{
    public class Dwarf : IRace
    {
        public string Name => "Dwarf";
        public int Damage => 1;
        public int Health => 110;
        public int Armor => 2;
        public int Initiative => 3;
    }
}
