using Fighters.Models.Fighters;

namespace Fighters;

public class Battle
{
    private const int MinInitiativeRoll = 1;
    private const int MaxInitiativeRollExclusive = 21;
    public static void Start( List<IFighter> fighters )
    {
        if ( fighters.Count < 2 )
        {
            Console.WriteLine( "At least 2 fighters are required to start a fight." );
            return;
        }

        RestoreFighters( fighters );

        int round = 1;

        List<IFighter> aliveFighters = [ .. fighters ];

        while ( aliveFighters.Count > 1 )
        {
            Console.WriteLine( $"\n===== Round {round} =====" );

            List<IFighter> roundQueue = GetRoundQueue( aliveFighters );

            Console.WriteLine( "Initiative order: " + string.Join( ", ", roundQueue.Select( f => f.Name ) ) );

            List<string> eliminated = ProcessRound( aliveFighters, roundQueue );

            PrintFighterStatuses( fighters );
            PrintEliminatedFighters( eliminated );

            round++;
        }

        PrintWinner( fighters );
    }

    private static List<IFighter> GetRoundQueue( List<IFighter> aliveFighters )
    {
        List<(IFighter fighter, int total)> initiativeRolls = [];

        foreach ( IFighter fighter in aliveFighters )
        {
            int roll = Random.Shared.Next( MinInitiativeRoll, MaxInitiativeRollExclusive );
            int bonus = fighter.InitiativeBonus;
            int total = roll + bonus;

            initiativeRolls.Add( (fighter, total) );

            Console.WriteLine( $"{fighter.Name} rolls initiative: {total} (d20 {roll} + bonus {bonus})" );
        }

        return [ .. initiativeRolls
            .OrderByDescending( initiativeRoll => initiativeRoll.total )
            .Select( initiativeRoll => initiativeRoll.fighter ) ];
    }


    private static List<string> ProcessRound( List<IFighter> aliveFighters, List<IFighter> roundQueue )
    {
        List<string> eliminated = [];

        foreach ( IFighter fighter in roundQueue )
        {
            if ( !aliveFighters.Contains( fighter ) || aliveFighters.Count <= 1 )
            {
                continue;
            }

            List<IFighter> potentialTargets = [ .. aliveFighters.Where( target => target != fighter ) ];

            IFighter target = potentialTargets[ Random.Shared.Next( potentialTargets.Count ) ];

            AttackReport report = fighter.Attack( target );
            PrintReport( report );

            if ( !target.IsAlive )
            {
                aliveFighters.Remove( target );
                eliminated.Add( target.Name );
            }
        }

        return eliminated;
    }

    private static void RestoreFighters( List<IFighter> fighters )
    {
        foreach ( IFighter fighter in fighters )
        {
            fighter.RestoreHealth();
        }
    }

    private static void PrintFighterStatuses( List<IFighter> fighters )
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

    private static void PrintEliminatedFighters( List<string> eliminated )
    {
        foreach ( string name in eliminated )
        {
            Console.WriteLine( $"--- {name} is eliminated! ---" );
        }
    }

    private static void PrintWinner( List<IFighter> aliveFighters )
    {
        IFighter winner = aliveFighters[ 0 ];

        Console.WriteLine( $"\nWinner: {winner.Name} with {winner.CurrentHealth}/{winner.MaxHealth} HP!" );
    }

    private static void PrintReport( AttackReport report )
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
