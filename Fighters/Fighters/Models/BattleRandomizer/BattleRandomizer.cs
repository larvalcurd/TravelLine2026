namespace Fighters.Models.BattleRadomizer;

public class BattleRandomizer : IBattleRandomizer
{
    public int RollInitiative( int min, int maxExclusive )
    {
        return Random.Shared.Next( min, maxExclusive );
    }

    public double GetDamageMultiplier( double min, double max )
    {
        return Random.Shared.NextDouble() * ( max - min ) + min;
    }

    public bool RollCritical( double critChance )
    {
        return Random.Shared.NextDouble() < critChance;
    }

    public T PickRandom<T>( IReadOnlyList<T> items )
    {
        if ( items.Count == 0 )
            throw new ArgumentException( "Cannot pick from empty list", nameof( items ) );

        return items[ Random.Shared.Next( items.Count ) ];
    }
}