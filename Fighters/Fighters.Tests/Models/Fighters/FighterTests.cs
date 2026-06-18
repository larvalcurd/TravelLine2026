using Moq;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Classes;
using Fighters.Models.Weapons;
using Fighters.Models.Armors;
using Fighters.Models.BattleRadomizer;

namespace Fighters.Tests.Models.Fighters;

public class FighterTests
{
    private static Fighter CreateFighter(
    string name = "TestFighter",
    int raceArmor = 0,
    int armor = 0,
    int raceHealth = 0,
    int classHealth = 0,
    int raceDamage = 0,
    int classDamage = 0,
    int weaponDamage = 0,
    int raceInitiative = 0,
    int classInitiative = 0,
    IBattleRandomizer? randomizer = null )
    {
        var raceMock = new Mock<IRace>();
        raceMock.SetupGet( x => x.Armor ).Returns( raceArmor );
        raceMock.SetupGet( x => x.Health ).Returns( raceHealth );
        raceMock.SetupGet( x => x.Damage ).Returns( raceDamage );
        raceMock.SetupGet( x => x.Initiative ).Returns( raceInitiative );
        raceMock.SetupGet( x => x.Name ).Returns( "TestRace" );

        var classMock = new Mock<IFighterClass>();
        classMock.SetupGet( x => x.Health ).Returns( classHealth );
        classMock.SetupGet( x => x.Damage ).Returns( classDamage );
        classMock.SetupGet( x => x.Initiative ).Returns( classInitiative );
        classMock.SetupGet( x => x.Name ).Returns( "TestClass" );

        var weaponMock = new Mock<IWeapon>();
        weaponMock.SetupGet( x => x.Damage ).Returns( weaponDamage );
        weaponMock.SetupGet( x => x.Name ).Returns( "TestWeapon" );

        var armorMock = new Mock<IArmor>();
        armorMock.SetupGet( x => x.Armor ).Returns( armor );
        armorMock.SetupGet( x => x.Name ).Returns( "TestArmor" );

        var randomizerToUse = randomizer ?? new Mock<IBattleRandomizer>().Object;

        return new Fighter(
            name,
            raceMock.Object,
            classMock.Object,
            weaponMock.Object,
            armorMock.Object,
            randomizerToUse );
    }

    private static Mock<IBattleRandomizer> CreateRandomizerMock(
        double multiplier = 1.0,
        bool isCritical = false )
    {
        var mock = new Mock<IBattleRandomizer>();
        mock.Setup( x => x.GetDamageMultiplier( It.IsAny<double>(), It.IsAny<double>() ) )
            .Returns( multiplier );
        mock.Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( isCritical );
        return mock;
    }

    [Fact]
    public void Constructor_WhenValidArguments_SetsName()
    {
        string name = "Aragorn";
        var fighter = CreateFighter( name: name );

        Assert.Equal( name, fighter.Name );
    }

    [Fact]
    public void Constructor_WhenHealthProvided_CalculatesMaxHealth()
    {
        int raceHealth = 100;
        int classHealth = 50;
        var fighter = CreateFighter( raceHealth: raceHealth, classHealth: classHealth );

        Assert.Equal( 150, fighter.MaxHealth );
    }

    [Fact]
    public void Constructor_WhenMaxHealthPositive_SetsCurrentHealthToMax()
    {
        var fighter = CreateFighter( raceHealth: 10, classHealth: 10 );

        Assert.Equal( fighter.MaxHealth, fighter.CurrentHealth );
    }

    [Fact]
    public void Constructor_WhenMaxHealthPositive_SetsIsAliveTrue()
    {
        var fighter = CreateFighter( raceHealth: 1, classHealth: 1 );

        Assert.True( fighter.IsAlive );
    }

    [Fact]
    public void Constructor_WhenMaxHealthZero_SetsIsAliveFalse()
    {
        var fighter = CreateFighter( raceHealth: 0, classHealth: 0 );

        Assert.Equal( 0, fighter.MaxHealth );
        Assert.Equal( 0, fighter.CurrentHealth );
        Assert.False( fighter.IsAlive );
    }

    [Fact]
    public void Constructor_CalculatesInitiativeBonus()
    {
        var fighter = CreateFighter( raceInitiative: 3, classInitiative: 4 );

        Assert.Equal( 7, fighter.InitiativeBonus );
    }

    // ====================
    // Attack
    // ====================

    [Fact]
    public void Attack_WithBaseDamage_CalculatesCorrectBaseDamage()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 1.0, isCritical: false );
        var attacker = CreateFighter(
            raceDamage: 10, classDamage: 15, weaponDamage: 20,
            randomizer: randomizerMock.Object );
        var target = CreateFighter( name: "Enemy", raceArmor: 0, armor: 0 );

        AttackReport report = attacker.Attack( target );

        Assert.Equal( 45, report.BaseDamage );
        Assert.Equal( 1.0, report.Multiplier );
        Assert.Equal( 45, report.DamageDealt );
        Assert.False( report.IsCritical );
    }

    [Fact]
    public void Attack_WithMultiplier_AppliesMultiplierToBaseDamage()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 0.9, isCritical: false );
        var attacker = CreateFighter(
            raceDamage: 10, classDamage: 20, weaponDamage: 20,
            randomizer: randomizerMock.Object );
        var target = CreateFighter( name: "Enemy", raceArmor: 0, armor: 0 );

        var report = attacker.Attack( target );

        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 0.9, report.Multiplier );
        Assert.Equal( 45, report.DamageDealt );
    }

    [Fact]
    public void Attack_WithCriticalHit_DoublesFinalDamage()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 1.0, isCritical: true );
        var attacker = CreateFighter(
            raceDamage: 10, classDamage: 20, weaponDamage: 20,
            randomizer: randomizerMock.Object );
        var target = CreateFighter( name: "Enemy", raceArmor: 0, armor: 0 );

        var report = attacker.Attack( target );

        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 1.0, report.Multiplier );
        Assert.Equal( 100, report.DamageDealt );
        Assert.True( report.IsCritical );
    }

    [Fact]
    public void Attack_WithNoCritical_DoesNotDouble()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 1.0, isCritical: false );
        var attacker = CreateFighter(
            raceDamage: 10, classDamage: 20, weaponDamage: 20,
            randomizer: randomizerMock.Object );
        var target = CreateFighter( name: "Enemy", raceArmor: 0, armor: 0 );

        var report = attacker.Attack( target );

        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 1.0, report.Multiplier );
        Assert.Equal( 50, report.DamageDealt );
        Assert.False( report.IsCritical );
    }

    [Fact]
    public void Attack_WithCriticalAndMultiplier_AppliesBoth()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 0.9, isCritical: true );
        var attacker = CreateFighter(
            raceDamage: 50,
            randomizer: randomizerMock.Object );
        var target = CreateFighter( name: "Enemy", raceArmor: 0, armor: 0 );

        var report = attacker.Attack( target );

        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 0.9, report.Multiplier );
        Assert.Equal( 90, report.DamageDealt );
        Assert.True( report.IsCritical );
    }

    [Fact]
    public void Attack_CallsTakeDamageWithExpectedDamage()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 1.0, isCritical: false );
        var attacker = CreateFighter(
            raceDamage: 10, classDamage: 20, weaponDamage: 20,
            randomizer: randomizerMock.Object );

        var targetMock = new Mock<IFighter>();
        targetMock.Setup( x => x.TakeDamage( It.IsAny<int>() ) ).Returns( ( int d ) => d );

        attacker.Attack( targetMock.Object );

        targetMock.Verify( x => x.TakeDamage( 50 ), Times.Once );
    }

    [Fact]
    public void Attack_ReportContainsNames()
    {
        var attacker = CreateFighter( name: "Hero" );
        var targetMock = new Mock<IFighter>();
        targetMock.SetupGet( x => x.Name ).Returns( "Enemy" );

        var report = attacker.Attack( targetMock.Object );

        Assert.Equal( "Hero", report.AttackerName );
        Assert.Equal( "Enemy", report.DefenderName );
    }

    [Fact]
    public void Attack_ReturnsActualDamageFromTarget()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 1.0, isCritical: false );
        var attacker = CreateFighter( raceDamage: 50, randomizer: randomizerMock.Object );

        var targetMock = new Mock<IFighter>();
        targetMock.SetupGet( x => x.Name ).Returns( "Enemy" );
        targetMock.Setup( x => x.TakeDamage( 50 ) ).Returns( 30 );
        targetMock.SetupGet( x => x.IsAlive ).Returns( true );

        var report = attacker.Attack( targetMock.Object );

        Assert.Equal( 30, report.DamageDealt );
    }

    [Fact]
    public void Attack_WhenTargetDies_ReportShowsKilled()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 1.0, isCritical: false );
        var attacker = CreateFighter( raceDamage: 50, randomizer: randomizerMock.Object );

        var targetMock = new Mock<IFighter>();
        targetMock.SetupGet( x => x.Name ).Returns( "Enemy" );
        targetMock.Setup( x => x.TakeDamage( 50 ) ).Returns( 50 );
        targetMock.SetupGet( x => x.IsAlive ).Returns( false );

        var report = attacker.Attack( targetMock.Object );

        Assert.True( report.DefenderDied );
    }

    [Fact]
    public void Attack_WhenTargetSurvives_ReportShowsAlive()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 1.0, isCritical: false );
        var attacker = CreateFighter( raceDamage: 50, randomizer: randomizerMock.Object );

        var targetMock = new Mock<IFighter>();
        targetMock.SetupGet( x => x.Name ).Returns( "Enemy" );
        targetMock.Setup( x => x.TakeDamage( 50 ) ).Returns( 50 );
        targetMock.SetupGet( x => x.IsAlive ).Returns( true );

        var report = attacker.Attack( targetMock.Object );

        Assert.False( report.DefenderDied );
    }

    [Fact]
    public void Attack_Integration_RealFighterAttacksRealFighter()
    {
        var randomizerMock = CreateRandomizerMock( multiplier: 1.0, isCritical: false );
        var attacker = CreateFighter(
            name: "Warrior",
            raceDamage: 10, classDamage: 20, weaponDamage: 20,
            randomizer: randomizerMock.Object );

        var target = CreateFighter(
            name: "Enemy",
            raceHealth: 100, classHealth: 50,   // MaxHealth = 150
            raceArmor: 5, armor: 5 );           // TotalArmor = 10

        var report = attacker.Attack( target );

        // baseDamage=50, multiplier=1.0, finalDamage=50, armor=10 -> actual=40
        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 40, report.DamageDealt );
        Assert.Equal( 110, target.CurrentHealth );
        Assert.True( target.IsAlive );
        Assert.False( report.DefenderDied );
        Assert.Equal( "Warrior", report.AttackerName );
        Assert.Equal( "Enemy", report.DefenderName );
    }

    // ====================
    // TakeDamage
    // ====================

    [Fact]
    public void TakeDamage_WhenDamageExceedsArmor_ReducesHealth()
    {
        // MaxHealth=150, Armor=10
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        fighter.TakeDamage( 50 );

        Assert.Equal( 110, fighter.CurrentHealth ); // 150 - (50-10)
    }

    [Fact]
    public void TakeDamage_WhenDamageEqualsArmor_HealthUnchanged()
    {
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        fighter.TakeDamage( 10 );

        Assert.Equal( 150, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_WhenDamageLessThanArmor_HealthUnchanged()
    {
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        fighter.TakeDamage( 4 );

        Assert.Equal( 150, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_WhenDamageExceedsHealth_HealthBecomesZero()
    {
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        fighter.TakeDamage( 999 );

        Assert.Equal( 0, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_ReturnsActualDamageTaken()
    {
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        int actualDamage = fighter.TakeDamage( 15 );

        Assert.Equal( 5, actualDamage ); // 15 - 10
        Assert.Equal( 145, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_WhenFullyBlocked_ReturnsZero()
    {
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        int actualDamage = fighter.TakeDamage( 3 );

        Assert.Equal( 0, actualDamage );
    }

    [Fact]
    public void TakeDamage_WhenLethal_SetsIsAliveFalse()
    {
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        fighter.TakeDamage( 999 );

        Assert.False( fighter.IsAlive );
    }

    // ====================
    // RestoreHealth
    // ====================

    [Fact]
    public void RestoreHealth_AfterDamage_RestoresToMax()
    {
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        fighter.TakeDamage( 15 );
        fighter.RestoreHealth();

        Assert.Equal( 150, fighter.CurrentHealth );
    }

    [Fact]
    public void RestoreHealth_WhenFull_RemainsFull()
    {
        var fighter = CreateFighter( raceHealth: 100, classHealth: 50,
                                    raceArmor: 5, armor: 5 );
        fighter.RestoreHealth();

        Assert.Equal( 150, fighter.CurrentHealth );
    }

}