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

    private class FakeGameOutput : IGameOutput
    {
        private readonly List<string> _messages = new();

        public IReadOnlyList<string> Messages => _messages;

        public void WriteLine( string message )
        {
            _messages.Add( message );
        }
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

        randomizerMock
            .SetupSequence( x => x.RollInitiative( It.IsAny<int>(), It.IsAny<int>() ) )
            .Returns( 10 )  // Medium
            .Returns( 15 )  // Fastest
            .Returns( 5 );  // Slowest

        randomizerMock
            .Setup( x => x.GetDamageMultiplier( It.IsAny<double>(), It.IsAny<double>() ) )
            .Returns( 1.0 );

        randomizerMock
            .Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( false );

        var outputMock = CreateOutputMock();
        var battle = new Battle( randomizerMock.Object, outputMock.Object );

        var fighter1Mock = CreateFighterMock( "Medium", initiativeBonus: 0 );
        var fighter2Mock = CreateFighterMock( "Fastest", initiativeBonus: 5 );
        var fighter3Mock = CreateFighterMock( "Slowest", initiativeBonus: 0 );

        var slowestIsAlive = true;
        fighter3Mock.SetupGet( x => x.IsAlive ).Returns( () => slowestIsAlive );

        var fastestIsAlive = true;
        fighter2Mock.SetupGet( x => x.IsAlive ).Returns( () => fastestIsAlive );

        // контроллируем, кто кого атакует
        randomizerMock
            .SetupSequence( x => x.PickRandom( It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( fighter3Mock.Object )  // Fastest targets Slowest
            .Returns( fighter2Mock.Object ); // Medium targets Fastest

        fighter1Mock
            .Setup( x => x.Attack( It.IsAny<IFighter>() ) )
            .Callback( () =>
            {
                attackOrder.Add( "Medium" );
                fastestIsAlive = false;
            } )
            .Returns( ( IFighter target ) => new AttackReport(
                "Medium",
                target.Name,
                false,
                100,
                true,
                100,
                1.0
            ) );

        fighter2Mock
            .Setup( x => x.Attack( It.IsAny<IFighter>() ) )
            .Callback( () =>
            {
                attackOrder.Add( "Fastest" );
                slowestIsAlive = false;
            } )
            .Returns( ( IFighter target ) => new AttackReport(
                "Fastest",
                target.Name,
                false,
                100,
                true,
                100,
                1.0
            ) );

        fighter3Mock
            .Setup( x => x.Attack( It.IsAny<IFighter>() ) )
            .Callback( () => attackOrder.Add( "Slowest" ) )
            .Returns( ( IFighter target ) => new AttackReport(
                "Slowest",
                target.Name,
                false,
                10,
                false,
                10,
                1.0
            ) );

        var fighters = new List<IFighter>
        {
            fighter1Mock.Object,
            fighter2Mock.Object,
            fighter3Mock.Object
        };

        battle.Start( fighters );

        // две атаки, медленный не успел атаковать
        Assert.Equal( 2, attackOrder.Count );
        Assert.Equal( "Fastest", attackOrder[ 0 ] );
        Assert.Equal( "Medium", attackOrder[ 1 ] );
    }

    // [Fact]
    // public void Start_WhenFighterActs_PicksTargetFromOtherAliveFighters()

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

    // [Fact]
    // public void Start_WhenFighterWasEliminatedBeforeTurn_DoesNotAllowItToAttack()

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

    // [Fact]
    // public void Start_WithThreeFighters_ContinuesUntilSingleWinnerRemains()
}