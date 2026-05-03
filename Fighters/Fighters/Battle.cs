using Fighters.Models.Fighters;

namespace Fighters;

public class Battle
{
    public void Start( List<IFighter> fighters )
    {
        if ( fighters.Count < 2 )
        {
            Console.WriteLine( "At least 2 fighters are required to start a fight." );
            return;
        }

        RestoreFighters( fighters );

        int round = 1;

        while ( fighters.Count( f => f.IsAlive ) > 1 )
        {
            Console.WriteLine( $"\n===== Round {round} =====" );

            List<IFighter> roundQueue = GetRoundQueue( fighters );

            Console.WriteLine( "Initiative order: " + string.Join( ", ", roundQueue.Select( f => f.Name ) ) );

            List<string> eliminated = ProcessRound( fighters, roundQueue );

            PrintFighterStatuses( fighters );
            PrintEliminatedFighters( eliminated );

            round++;
        }

        PrintWinner( fighters );
    }

    private List<IFighter> GetRoundQueue( List<IFighter> fighters )
    {
        var initiativeRolls = new List<(IFighter fighter, int roll, int bonus, int total)>();

        foreach ( IFighter fighter in fighters.Where( f => f.IsAlive ) )
        {
            int roll = Random.Shared.Next( 1, 21 );
            int bonus = fighter.InitiativeBonus;
            int total = roll + bonus;

            initiativeRolls.Add( (fighter, roll, bonus, total) );

            Console.WriteLine( $"{fighter.Name} rolls initiative: {total} (d20 {roll} + bonus {bonus})" );
        }

        return initiativeRolls
            .OrderByDescending( x => x.total )
            .Select( x => x.fighter )
            .ToList();
    }

    private List<string> ProcessRound( List<IFighter> fighters, List<IFighter> roundQueue )
    {
        List<string> eliminated = new List<string>();

        foreach ( IFighter fighter in roundQueue )
        {
            if ( !fighter.IsAlive )
            {
                continue;
            }

            List<IFighter> potentialTargets = fighters
                .Where( t => t.IsAlive && t != fighter )
                .ToList();

            if ( potentialTargets.Count == 0 )
            {
                continue;
            }

            IFighter target = potentialTargets[ Random.Shared.Next( potentialTargets.Count ) ];

            AttackReport report = fighter.Attack( target );
            PrintReport( report );

            if ( !target.IsAlive && !eliminated.Contains( target.Name ) )
            {
                eliminated.Add( target.Name );
            }
        }

        return eliminated;
    }

    private void RestoreFighters( List<IFighter> fighters )
    {
        foreach ( IFighter fighter in fighters )
        {
            fighter.RestoreHealth();
        }
    }

    private void PrintFighterStatuses( List<IFighter> fighters )
    {
        Console.WriteLine( "\nFighter status after the round:" );

        foreach ( IFighter fighter in fighters )
        {
            string status = fighter.IsAlive
                ? $"{fighter.Name}: {fighter.CurrentHealth}/{fighter.MaxHealth} HP"
                : $"{fighter.Name}: eliminated";

            Console.WriteLine( status );
        }
    }

    private void PrintEliminatedFighters( List<string> eliminated )
    {
        foreach ( string name in eliminated )
        {
            Console.WriteLine( $"--- {name} is eliminated! ---" );
        }
    }

    private void PrintWinner( List<IFighter> fighters )
    {
        IFighter winner = fighters.FirstOrDefault( f => f.IsAlive );

        if ( winner != null )
        {
            Console.WriteLine( $"\nWinner: {winner.Name} with {winner.CurrentHealth}/{winner.MaxHealth} HP!" );
        }
        else
        {
            Console.WriteLine( "\nAll fighters are eliminated! It's a tie!" );
        }
    }

    private void PrintReport( AttackReport report )
    {
        Console.WriteLine();
        Console.WriteLine( $"{report.AttackerName} attacks {report.DefenderName}" );
        Console.WriteLine( $"Base damage: {report.BaseDamage}" );
        Console.WriteLine( $"Attack multiplier: {report.Multiplier}" );

        string critMessage = report.IsCritical ? "CRIT!" : "No crit";
        Console.WriteLine( critMessage );

        Console.WriteLine( $"Damage dealt: {report.DamageDealt}" );
    }
}
