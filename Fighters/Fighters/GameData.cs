using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters;

public static class GameData
{
    public static readonly IRace[] Races =
    {
        new Human(),
        new Elf(),
        new Orc(),
        new Dwarf()
    };

    public static readonly IFighterClass[] FighterClasses =
    {
        new Warrior(),
        new Knight(),
        new Rogue()
    };

    public static readonly IWeapon[] Weapons =
    {
        new Fists(),
        new Axe(),
        new Sword()
    };

    public static readonly IArmor[] Armors =
    {
        new NoArmor(),
        new LightArmor(),
        new HeavyArmor()
    };
}
