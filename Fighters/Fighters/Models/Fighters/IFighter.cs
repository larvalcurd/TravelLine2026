namespace Fighters.Models.Fighters;

public interface IFighter : INamed
{
    int MaxHealth { get; }
    int CurrentHealth { get; }
    bool IsAlive { get; }
    int InitiativeBonus { get; }

    AttackReport Attack( IFighter target );
    int TakeDamage( int damage );
    void RestoreHealth();
}