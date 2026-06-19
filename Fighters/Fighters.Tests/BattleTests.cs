using Moq;
using Fighters.Models.Fighters;
using Fighters.Models.BattleRadomizer;
using Fighters.Models;

namespace Fighters.Tests;

public class BattleTests
{
    private static Mock<IBattleRandomizer> CreateRandomizerMock()
    {
        var mock = new Mock<IBattleRandomizer>();

        mock.Setup( x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ) )
            .Returns( 10 );
        mock.Setup( x => x.PickRandom( It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( ( IReadOnlyList<IFighter> targets ) => targets[ 0 ] );
        mock.Setup( x => x.GetDamageMultiplier( It.IsAny<double>(), It.IsAny<double>() ) )
            .Returns( 1.0 );
        mock.Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( false );

        return mock;
    }

    private static Mock<IGameOutput> CreateOutputMock()
    {
        return new Mock<IGameOutput>();
    }

    private static Mock<IFighter> CreateFighterMock(
        string name = "Fighter",
        int health = 100,
        int initiativeBonus = 0
    )
    {
        var mock = new Mock<IFighter>();

        mock.SetupGet( x => x.Name ).Returns( name );
        mock.SetupGet( x => x.MaxHealth ).Returns( health );
        mock.SetupGet( x => x.CurrentHealth ).Returns( health );
        mock.SetupGet( x => x.IsAlive ).Returns( true );
        mock.SetupGet( x => x.InitiativeBonus ).Returns( initiativeBonus );

        mock.Setup( x => x.Attack( It.IsAny<IFighter>() ) )
            .Returns( ( IFighter target ) => new AttackReport(
                name,
                target.Name,
                false, // IsCritical
                10,
                false, // DefenderDied
                20,
                1.0 ) );

        return mock;
    }

    private static (Mock<IFighter> strong, Mock<IFighter> weak) CreateQuickBattleFighters()
    {
        var strongMock = CreateFighterMock( "Strong" );
        var weakMock = CreateFighterMock( "Weak" );

        var weakIsAlive = true;
        weakMock.SetupGet( x => x.IsAlive ).Returns( () => weakIsAlive );

        strongMock.Setup( x => x.Attack( weakMock.Object ) )
            .Callback( () => weakIsAlive = false )
            .Returns( new AttackReport(
                "Strong",
                "Weak",
                false, // IsCritical
                100,
                true, // DefenderDied
                100,
                1.0 ) );

        return (strongMock, weakMock);
    }

    [Fact]
    public void Start_FightersListIsEmpty_PrintsErrorAndDoesNotStartBattle()
    {
        var randomizerMock = CreateRandomizerMock();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var fighters = new List<IFighter>();

        battle.Start( fighters );

        outputMock.Verify(
            x => x.WriteLine( "At least 2 fighters are required to start a fight." ),
            Times.Once );

        randomizerMock.Verify(
            x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ),
            Times.Never );

        randomizerMock.Verify(
            x => x.PickRandom( It.IsAny<IReadOnlyList<IFighter>>() ),
            Times.Never );

        randomizerMock.Verify(
            x => x.GetDamageMultiplier( It.IsAny<double>(), It.IsAny<double>() ),
            Times.Never );
    }

    [Fact]
    public void Start_FightersListHasOneFighter_PrintsErrorAndDoesNotStartBattle()
    {
        var randomizerMock = CreateRandomizerMock();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var fighter1Mock = CreateFighterMock();
        var fighters = new List<IFighter>() { fighter1Mock.Object };

        battle.Start( fighters );

        outputMock.Verify(
            x => x.WriteLine( "At least 2 fighters are required to start a fight." ),
            Times.Once );

        fighter1Mock.Verify(
            x => x.RestoreHealth(),
            Times.Never );

        randomizerMock.Verify(
            x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ),
            Times.Never );

        randomizerMock.Verify(
            x => x.PickRandom( It.IsAny<IReadOnlyList<IFighter>>() ),
            Times.Never );

        randomizerMock.Verify(
            x => x.GetDamageMultiplier( It.IsAny<double>(), It.IsAny<double>() ),
            Times.Never );
    }

    [Fact]
    public void Start_WhenBattleStarts_RestoresAllFightersHealth()
    {
        var randomizerMock = CreateRandomizerMock();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var (fighter1Mock, fighter2Mock) = CreateQuickBattleFighters();
        var fighters = new List<IFighter>() { fighter1Mock.Object, fighter2Mock.Object };

        battle.Start( fighters );

        fighter1Mock.Verify(
            x => x.RestoreHealth(),
            Times.Once );

        fighter2Mock.Verify(
            x => x.RestoreHealth(),
            Times.Once );
    }

    [Fact]
    public void Start_WhenRoundStarts_RollsInitiativeForEachAliveFighter()
    {
        var randomizerMock = CreateRandomizerMock();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var (fighter1Mock, fighter2Mock) = CreateQuickBattleFighters();
        var fighters = new List<IFighter>() { fighter1Mock.Object, fighter2Mock.Object };

        battle.Start( fighters );

        randomizerMock.Verify(
            x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ),
            Times.Exactly( 2 ) );
    }

    [Fact]
    public void Start_FightersHaveDifferentInitiative_AttacksInInitiativeOrder()
    {
        var attackOrder = new List<string>();

        var randomizerMock = new Mock<IBattleRandomizer>();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var mediumMock = CreateFighterMock( "Medium", initiativeBonus: 0 );
        var fastestMock = CreateFighterMock( "Fastest", initiativeBonus: 5 );
        var slowestMock = CreateFighterMock( "Slowest", initiativeBonus: 0 );

        var fastestIsAlive = true;
        var slowestIsAlive = true;

        fastestMock
            .SetupGet( x => x.IsAlive )
            .Returns( () => fastestIsAlive );

        slowestMock
            .SetupGet( x => x.IsAlive )
            .Returns( () => slowestIsAlive );

        randomizerMock
            .SetupSequence( x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ) )
            .Returns( 10 ) // Medium total: 10 + 0 = 10
            .Returns( 15 ) // Fastest total: 15 + 5 = 20
            .Returns( 5 ); // Slowest total: 5 + 0 = 5

        randomizerMock
            .SetupSequence( x => x.PickRandom( It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( slowestMock.Object ) // Fastest attacks Slowest
            .Returns( fastestMock.Object ); // Medium attacks Fastest

        fastestMock
            .Setup( x => x.Attack( slowestMock.Object ) )
            .Callback( () =>
            {
                attackOrder.Add( "Fastest" );
                slowestIsAlive = false;
            } )
            .Returns( new AttackReport(
                "Fastest",
                "Slowest",
                false,
                100,
                true,
                100,
                1.0 ) );

        mediumMock
            .Setup( x => x.Attack( fastestMock.Object ) )
            .Callback( () =>
            {
                attackOrder.Add( "Medium" );
                fastestIsAlive = false;
            } )
            .Returns( new AttackReport(
                "Medium",
                "Fastest",
                false,
                100,
                true,
                100,
                1.0 ) );

        slowestMock
            .Setup( x => x.Attack( It.IsAny<IFighter>() ) )
            .Callback( () => attackOrder.Add( "Slowest" ) )
            .Returns( ( IFighter target ) => new AttackReport(
                "Slowest",
                target.Name,
                false,
                10,
                false,
                10,
                1.0 ) );

        var fighters = new List<IFighter>
        {
            mediumMock.Object,
            fastestMock.Object,
            slowestMock.Object
        };

        battle.Start( fighters );

        Assert.Equal(
            new List<string> { "Fastest", "Medium" },
            attackOrder );

        fastestMock.Verify(
            x => x.Attack( slowestMock.Object ),
            Times.Once );

        mediumMock.Verify(
            x => x.Attack( fastestMock.Object ),
            Times.Once );

        slowestMock.Verify(
            x => x.Attack( It.IsAny<IFighter>() ),
            Times.Never );
    }

    [Fact]
    public void Start_WhenFighterActs_PicksTargetFromOtherAliveFighters()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var attackerMock = CreateFighterMock( "Attacker" );
        var targetMock = CreateFighterMock( "Target" );

        var targetIsAlive = true;

        targetMock
            .SetupGet( x => x.IsAlive )
            .Returns( () => targetIsAlive );

        attackerMock
            .Setup( x => x.Attack( targetMock.Object ) )
            .Callback( () => targetIsAlive = false )
            .Returns( new AttackReport(
                "Attacker",
                "Target",
                false,
                100,
                true,
                100,
                1.0 ) );

        randomizerMock
            .SetupSequence( x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ) )
            .Returns( 10 )
            .Returns( 5 );

        randomizerMock
            .Setup( x => x.PickRandom( It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( targetMock.Object );

        var fighters = new List<IFighter>
        {
            attackerMock.Object,
            targetMock.Object
        };

        battle.Start( fighters );

        randomizerMock.Verify(
            x => x.PickRandom( It.Is<IReadOnlyList<IFighter>>( targets =>
                targets.Count == 1 &&
                targets.Contains( targetMock.Object ) &&
                !targets.Contains( attackerMock.Object ) ) ),
            Times.Once );

        attackerMock.Verify(
            x => x.Attack( targetMock.Object ),
            Times.Once );
    }

    [Fact]
    public void Start_WhenTargetDies_RemovesTargetAndPrintsElimination()
    {
        var randomizerMock = CreateRandomizerMock();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var (winnerMock, loserMock) = CreateQuickBattleFighters();
        var fighters = new List<IFighter> { winnerMock.Object, loserMock.Object };

        battle.Start( fighters );

        outputMock.Verify(
            x => x.WriteLine( It.Is<string>( s => s.Contains( "eliminated!" ) ) ),
            Times.Once );

        outputMock.Verify(
            x => x.WriteLine( It.Is<string>( s => s.Contains( "Weak" ) ) ),
            Times.AtLeastOnce );
    }

    [Fact]
    public void Start_WhenFighterWasEliminatedBeforeTurn_DoesNotAllowItToAttack()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var fastMock = CreateFighterMock( "Fast" );
        var eliminatedBeforeTurnMock = CreateFighterMock( "EliminatedBeforeTurn" );
        var survivorMock = CreateFighterMock( "Survivor" );

        var fastIsAlive = true;
        var eliminatedBeforeTurnIsAlive = true;
        var survivorIsAlive = true;

        fastMock
            .SetupGet( x => x.IsAlive )
            .Returns( () => fastIsAlive );

        eliminatedBeforeTurnMock
        .SetupGet( x => x.IsAlive )
        .Returns( () => eliminatedBeforeTurnIsAlive );

        survivorMock
            .SetupGet( x => x.IsAlive )
            .Returns( () => survivorIsAlive );

        randomizerMock
            .SetupSequence( x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ) )
            .Returns( 20 ) // Fast
            .Returns( 10 ) // EliminatedBeforeTurn
            .Returns( 5 ); // Survivor

        randomizerMock
            .SetupSequence( x => x.PickRandom( It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( eliminatedBeforeTurnMock.Object ) // Fast kills EliminatedBeforeTurn
            .Returns( fastMock.Object );                // Survivor kills Fast, battle ends

        fastMock
        .Setup( x => x.Attack( eliminatedBeforeTurnMock.Object ) )
        .Callback( () => eliminatedBeforeTurnIsAlive = false )
        .Returns( new AttackReport(
            "Fast",
            "EliminatedBeforeTurn",
            false,
            100,
            true,
            100,
            1.0 ) );

        survivorMock
            .Setup( x => x.Attack( fastMock.Object ) )
            .Callback( () => fastIsAlive = false )
            .Returns( new AttackReport(
                "Survivor",
                "Fast",
                false,
                100,
                true,
                100,
                1.0 ) );

        var fighters = new List<IFighter>
        {
            fastMock.Object,
            eliminatedBeforeTurnMock.Object,
            survivorMock.Object
        };

        battle.Start( fighters );

        fastMock.Verify(
            x => x.Attack( eliminatedBeforeTurnMock.Object ),
            Times.Once );

        eliminatedBeforeTurnMock.Verify(
            x => x.Attack( It.IsAny<IFighter>() ),
            Times.Never );

        survivorMock.Verify(
            x => x.Attack( fastMock.Object ),
            Times.Once );

        outputMock.Verify(
            x => x.WriteLine( It.Is<string>( s => s.Contains( "EliminatedBeforeTurn" ) && s.Contains( "eliminated" ) ) ),
            Times.AtLeastOnce );
    }

    [Fact]
    public void Start_WhenOnlyOneFighterRemains_PrintsWinner()
    {
        var randomizerMock = CreateRandomizerMock();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var (winnerMock, loserMock) = CreateQuickBattleFighters();
        var fighters = new List<IFighter> { winnerMock.Object, loserMock.Object };

        battle.Start( fighters );

        outputMock.Verify(
             x => x.WriteLine( It.Is<string>( s => s.Contains( "Winner:" ) ) ),
             Times.Once
        );

        outputMock.Verify(
            x => x.WriteLine( It.Is<string>( s => s.Contains( "Strong" ) ) ),
            Times.AtLeastOnce
        );
    }

    [Fact]
    public void Start_WithThreeFighters_ContinuesUntilSingleWinnerRemains()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();
        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var strongMock = CreateFighterMock( "Strong" );
        var weakMock = CreateFighterMock( "Weak" );
        var middleMock = CreateFighterMock( "Middle" );

        var strongIsAlive = true;
        var weakIsAlive = true;
        var middleIsAlive = true;

        strongMock
            .SetupGet( x => x.IsAlive )
            .Returns( () => strongIsAlive );

        weakMock
            .SetupGet( x => x.IsAlive )
            .Returns( () => weakIsAlive );

        middleMock
            .SetupGet( x => x.IsAlive )
            .Returns( () => middleIsAlive );

        strongMock
            .SetupGet( x => x.CurrentHealth )
            .Returns( () => strongIsAlive ? 100 : 0 );

        weakMock
            .SetupGet( x => x.CurrentHealth )
            .Returns( () => weakIsAlive ? 100 : 0 );

        middleMock
            .SetupGet( x => x.CurrentHealth )
            .Returns( () => middleIsAlive ? 100 : 0 );

        randomizerMock
            .SetupSequence( x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ) )
            .Returns( 20 ) // Round 1: Strong, Weak, Middle
            .Returns( 10 )
            .Returns( 5 )
            .Returns( 20 ) // Round 2: Strong, Middle
            .Returns( 5 );

        randomizerMock
            .SetupSequence( x => x.PickRandom( It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( weakMock.Object )   // Round 1: Strong kills Weak
            .Returns( strongMock.Object ) // Round 1: Middle attacks Strong, but does not kill
            .Returns( middleMock.Object ); // Round 2: Strong kills Middle

        strongMock
            .Setup( x => x.Attack( weakMock.Object ) )
            .Callback( () => weakIsAlive = false )
            .Returns( new AttackReport(
                "Strong",
                "Weak",
                false,
                100,
                true,
                100,
                1.0 ) );

        middleMock
            .Setup( x => x.Attack( strongMock.Object ) )
            .Returns( new AttackReport(
                "Middle",
                "Strong",
                false,
                10,
                false,
                10,
                1.0 ) );

        strongMock
            .Setup( x => x.Attack( middleMock.Object ) )
            .Callback( () => middleIsAlive = false )
            .Returns( new AttackReport(
                "Strong",
                "Middle",
                false,
                100,
                true,
                100,
                1.0 ) );

        var fighters = new List<IFighter>
        {
            strongMock.Object,
            weakMock.Object,
            middleMock.Object
        };

        battle.Start( fighters );

        strongMock.Verify(
            x => x.Attack( weakMock.Object ),
            Times.Once );

        middleMock.Verify(
            x => x.Attack( strongMock.Object ),
            Times.Once );

        strongMock.Verify(
            x => x.Attack( middleMock.Object ),
            Times.Once );

        randomizerMock.Verify(
            x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ),
            Times.Exactly( 5 ) );

        outputMock.Verify(
            x => x.WriteLine( It.Is<string>( s => s.Contains( "Round 2" ) ) ),
            Times.Once );

        outputMock.Verify(
            x => x.WriteLine( It.Is<string>( s => s.Contains( "Winner:" ) && s.Contains( "Strong" ) ) ),
            Times.Once );

        outputMock.Verify(
            x => x.WriteLine( It.Is<string>( s => s.Contains( "eliminated" ) ) ),
            Times.AtLeastOnce );
    }
}