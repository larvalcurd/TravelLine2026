using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
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
        private int TotalArmor => _armor.Armor + _race.Armor + _fighterClass.Armor;

        private const double CritChance = 0.2;
        private const double CritMultiplier = 2.0;
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

            double multiplier = Random.Shared.NextDouble() * ( 1.1 - 0.8 ) + 0.8;

            int finalDamage = ( int )Math.Round( baseDamage * multiplier );

            bool wasCritical = false;

            if ( Random.Shared.NextDouble() < CritChance )
            {
                wasCritical = true;
                finalDamage = ( int )( finalDamage * CritMultiplier );
            }

            int actualDamage = target.TakeDamage( finalDamage );

            AttackReport report = new AttackReport(
                Name,
                target.Name,
                wasCritical,
                actualDamage,
                !target.IsAlive,
                baseDamage,
                multiplier
            );

            return report;
        }

        public int TakeDamage( int damage )
        {

            int takenDamage = Math.Max( damage - TotalArmor, 0 );
            CurrentHealth -= takenDamage;

            if ( CurrentHealth < 0 )
            {
                CurrentHealth = 0;
            }

            return takenDamage;
        }

        public void RestoreHealth()
        {
            CurrentHealth = MaxHealth;
        }
    }
}