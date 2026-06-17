namespace Fighters.Models.BattleRadomizer;

public interface IBattleRandomizer
{
    int RollInitiative( int min, int maxExclusive );
    double GetDamageMultiplier( double min, double max );
    bool RollCritical( double critChance );
    T PickRandom<T>( IReadOnlyList<T> items );
}