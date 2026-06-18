using Fighters.Models;
using Fighters.Models.BattleRadomizer;
using Fighters.Models.Fighters;

namespace Fighters;

public class Battle( IBattleRandomizer randomizer, IGameOutput output )
{
    private const int MinInitiativeRoll = 1;
    private const int MaxInitiativeRollExclusive = 21;

    private readonly IBattleRandomizer _randomizer = randomizer;
    private readonly IGameOutput _output = output;

    public void Start( List<IFighter> fighters )
    {
        if ( fighters.Count < 2 )
        {
            _output.WriteLine( "At least 2 fighters are required to start a fight." );
            return;
        }

        RestoreFighters( fighters );

        int round = 1;

        List<IFighter> aliveFighters = [ .. fighters ];

        while ( aliveFighters.Count > 1 )
        {
            _output.WriteLine( $"\n===== Round {round} =====" );

            List<IFighter> roundQueue = GetRoundQueue( aliveFighters );

            _output.WriteLine( "Initiative order: " + string.Join( ", ", roundQueue.Select( f => f.Name ) ) );

            List<string> eliminated = ProcessRound( aliveFighters, roundQueue );

            PrintFighterStatuses( fighters );
            PrintEliminatedFighters( eliminated );

            round++;
        }

        PrintWinner( aliveFighters );
    }

    private List<IFighter> GetRoundQueue( List<IFighter> aliveFighters )
    {
        List<(IFighter fighter, int total)> initiativeRolls = [];

        foreach ( IFighter fighter in aliveFighters )
        {
            int roll = _randomizer.RollInitiative( MinInitiativeRoll, MaxInitiativeRollExclusive );
            int bonus = fighter.InitiativeBonus;
            int total = roll + bonus;

            initiativeRolls.Add( (fighter, total) );

            _output.WriteLine( $"{fighter.Name} rolls initiative: {total} (d20 {roll} + bonus {bonus})" );
        }

        return [ .. initiativeRolls
            .OrderByDescending( initiativeRoll => initiativeRoll.total )
            .Select( initiativeRoll => initiativeRoll.fighter ) ];
    }


    private List<string> ProcessRound( List<IFighter> aliveFighters, List<IFighter> roundQueue )
    {
        List<string> eliminated = [];

        foreach ( IFighter fighter in roundQueue )
        {
            if ( !aliveFighters.Contains( fighter ) || aliveFighters.Count <= 1 )
            {
                continue;
            }

            List<IFighter> potentialTargets = [ .. aliveFighters.Where( target => target != fighter ) ];
            IFighter target = _randomizer.PickRandom( potentialTargets );

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

    private void PrintFighterStatuses( List<IFighter> fighters )
    {
        _output.WriteLine( "\nFighter status after the round:" );

        foreach ( IFighter fighter in fighters )
        {
            string status = fighter.IsAlive
                ? $"{fighter.Name}: {fighter.CurrentHealth}/{fighter.MaxHealth} HP"
                : $"{fighter.Name}: eliminated";

            _output.WriteLine( status );
        }
    }

    private void PrintEliminatedFighters( List<string> eliminated )
    {
        foreach ( string name in eliminated )
        {
            _output.WriteLine( $"--- {name} is eliminated! ---" );
        }
    }

    private void PrintWinner( List<IFighter> aliveFighters )
    {
        IFighter winner = aliveFighters[ 0 ];

        _output.WriteLine( $"\nWinner: {winner.Name} with {winner.CurrentHealth}/{winner.MaxHealth} HP!" );
    }

    private void PrintReport( AttackReport report )
    {
        _output.WriteLine( "" );
        _output.WriteLine( $"{report.AttackerName} attacks {report.DefenderName}" );
        _output.WriteLine( $"Base damage: {report.BaseDamage}" );
        _output.WriteLine( $"Attack multiplier: {report.Multiplier}" );

        string critMessage = report.IsCritical ? "CRIT!" : "No crit";
        _output.WriteLine( critMessage );

        _output.WriteLine( $"Damage dealt: {report.DamageDealt}" );
    }
}
