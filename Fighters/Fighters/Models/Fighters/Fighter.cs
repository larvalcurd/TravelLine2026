using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters;

public class Fighter : IFighter
{
    public string Name { get; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0;
    public int InitiativeBonus { get; private set; }

    private IRace _race;
    private IFighterClass _fighterClass;
    private IWeapon _weapon;
    private IArmor _armor;

    private int TotalDamage => _race.Damage + _fighterClass.Damage + _weapon.Damage;
    private int TotalArmor => _armor.Armor + _race.Armor;

    private const double CritChance = 0.2;
    private const double CritMultiplier = 2.0;
    private const double MinDamageMultiplier = 0.8;
    private const double MaxDamageMultiplier = 1.1;

    private static double GetRandomDamageMultiplier()
    {
        return Random.Shared.NextDouble() * ( MaxDamageMultiplier - MinDamageMultiplier ) + MinDamageMultiplier;
    }

    public Fighter( string name, IRace race, IFighterClass fighterClass, IWeapon weapon, IArmor armor )
    {
        Name = name;
        _race = race;
        _fighterClass = fighterClass;
        _weapon = weapon;
        _armor = armor;
        InitiativeBonus = race.Initiative + fighterClass.Initiative;
        MaxHealth = race.Health + fighterClass.Health;
        CurrentHealth = MaxHealth;
    }

    public AttackReport Attack( IFighter target )
    {
        int baseDamage = TotalDamage;
        double multiplier = GetRandomDamageMultiplier();
        int finalDamage = ( int )Math.Round( baseDamage * multiplier );
        bool wasCritical = Random.Shared.NextDouble() < CritChance;

        if ( wasCritical )
        {
            finalDamage = ( int )( finalDamage * CritMultiplier );
        }

        int actualDamage = target.TakeDamage( finalDamage );

        return new(
            Name,
            target.Name,
            wasCritical,
            actualDamage,
            !target.IsAlive,
            baseDamage,
            multiplier
        );
    }

    public int TakeDamage( int damage )
    {
        int takenDamage = Math.Max( damage - TotalArmor, 0 );
        CurrentHealth = Math.Max( CurrentHealth - takenDamage, 0 );
        return takenDamage;
    }

    public void RestoreHealth()
    {
        CurrentHealth = MaxHealth;
    }
}