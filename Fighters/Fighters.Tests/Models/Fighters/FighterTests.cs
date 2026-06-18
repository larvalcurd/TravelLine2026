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
    private Fighter CreateFighter(
    string name = "TestFighter",
    int raceArmor = 5,
    int armor = 5,
    int raceHealth = 100,
    int classHealth = 50,
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

    [Fact]
    public void Constructor_ValidArguments_SetsName()
    {
        string name = "Aragorn";

        var fighter = CreateFighter( name: name );

        Assert.Equal( name, fighter.Name );
    }

    [Fact]
    public void Constructor_ValidArguments_CalculatesMaxHealth()
    {
        int raceHealth = 100;
        int classHealth = 50;
        int expectedMaxHealth = raceHealth + classHealth;

        var fighter = CreateFighter( raceHealth: raceHealth, classHealth: classHealth );

        Assert.Equal( expectedMaxHealth, fighter.MaxHealth );
    }

    [Fact]
    public void Constructor_ValidArguments_SetsCurrentHealthToMaxHealth()
    {
        var fighter = CreateFighter();

        Assert.Equal( fighter.MaxHealth, fighter.CurrentHealth );
    }

    [Fact]
    public void Constructor_ValidArguments_SetsIsAliveTrue()
    {
        var fighter = CreateFighter();

        Assert.True( fighter.IsAlive );
    }

    [Fact]
    public void Constructor_ValidArguments_CalculatesInitiativeBonus()
    {
        int raceInitiative = 3;
        int classInitiative = 4;
        int expectedInitiative = raceInitiative + classInitiative;

        var fighter = CreateFighter(
            raceInitiative: raceInitiative,
            classInitiative: classInitiative );

        Assert.Equal( expectedInitiative, fighter.InitiativeBonus );
    }

    [Fact]
    public void Constructor_WhenMaxHealthIsZero_FighterIsNotAlive()
    {
        var fighter = CreateFighter( raceHealth: 0, classHealth: 0 );

        Assert.Equal( 0, fighter.MaxHealth );
        Assert.Equal( 0, fighter.CurrentHealth );
        Assert.False( fighter.IsAlive );
    }

    [Fact]
    public void Attack_WithRaceClassAndWeaponDamage_ReturnsCorrectBaseDamage()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();
        randomizerMock.Setup( x => x.GetDamageMultiplier( It.IsAny<double>(),
        It.IsAny<double>() ) ).Returns( 1.0 );
        randomizerMock.Setup( x => x.RollCritical( It.IsAny<double>() ) ).Returns( false );


        var attacker = CreateFighter(
            raceDamage: 10,
            classDamage: 15,
            weaponDamage: 20, // baseDamage = 10+15+20 = 45
            randomizer: randomizerMock.Object
        );

        var target = CreateFighter(
            name: "Enemy",
            raceArmor: 0,
            armor: 0
        );

        AttackReport report = attacker.Attack( target );

        Assert.Equal( 45, report.BaseDamage );
        Assert.Equal( 1.0, report.Multiplier );
        Assert.Equal( 45, report.DamageDealt );
        Assert.False( report.IsCritical );
    }

    [Fact]
    public void Attack_AppliesDamageMultiplierToBaseDamage()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();

        randomizerMock
            .Setup( x => x.GetDamageMultiplier(
                It.IsAny<double>(),
                It.IsAny<double>() ) )
            .Returns( 0.9 );

        randomizerMock
            .Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( false );

        var attacker = CreateFighter(
            raceDamage: 10,
            classDamage: 20,
            weaponDamage: 20, // baseDamage = 10+20+20 = 50
            randomizer: randomizerMock.Object
        );

        var target = CreateFighter(
            name: "Enemy",
            raceArmor: 0,
            armor: 0
        );

        var report = attacker.Attack( target );

        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 0.9, report.Multiplier );
        Assert.Equal( 45, report.DamageDealt );
    }

    [Fact]
    public void Attack_WhenCritical_DoublesFinalDamage()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();

        randomizerMock
            .Setup( x => x.GetDamageMultiplier(
                It.IsAny<double>(),
                It.IsAny<double>() ) )
            .Returns( 1.0 );

        randomizerMock
            .Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( true );

        var attacker = CreateFighter(
            raceDamage: 10,
            classDamage: 20,
            weaponDamage: 20,
            randomizer: randomizerMock.Object
        );

        var target = CreateFighter(
            name: "Enemy",
            raceArmor: 0,
            armor: 0
        );

        var report = attacker.Attack( target );

        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 1.0, report.Multiplier );
        Assert.Equal( 100, report.DamageDealt );
        Assert.True( report.IsCritical );
    }

    [Fact]
    public void Attack_WhenNotCritical_DoesNotDoubleDamage()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();

        randomizerMock
            .Setup( x => x.GetDamageMultiplier(
                It.IsAny<double>(),
                It.IsAny<double>() ) )
            .Returns( 1.0 );

        randomizerMock
            .Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( false );

        var attacker = CreateFighter(
            raceDamage: 10,
            classDamage: 20,
            weaponDamage: 20,
            randomizer: randomizerMock.Object
        );

        var target = CreateFighter(
            name: "Enemy",
            raceArmor: 0,
            armor: 0
        );

        var report = attacker.Attack( target );

        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 1.0, report.Multiplier );
        Assert.Equal( 50, report.DamageDealt );
        Assert.False( report.IsCritical );
    }

    [Fact]
    public void Attack_CallsTargetTakeDamageWithCorrectValue()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();
        randomizerMock.Setup( x => x.GetDamageMultiplier( It.IsAny<double>(), It.IsAny<double>() ) ).Returns( 1.0 );
        randomizerMock.Setup( x => x.RollCritical( It.IsAny<double>() ) ).Returns( false );

        var attacker = CreateFighter(
            raceDamage: 10,
            classDamage: 20,
            weaponDamage: 20,
            randomizer: randomizerMock.Object
        );

        int expectedDamage = 50;

        var targetMock = new Mock<IFighter>();
        targetMock.Setup( x => x.TakeDamage( It.IsAny<int>() ) ).Returns( ( int damage ) => damage );

        attacker.Attack( targetMock.Object );

        targetMock.Verify( x => x.TakeDamage( expectedDamage ), Times.Once );
    }

    [Fact]
    public void Attack_ReportContainsCorrectNames()
    {
        var attacker = CreateFighter( name: "Hero" );
        var targetMock = new Mock<IFighter>();
        targetMock.SetupGet( x => x.Name ).Returns( "Enemy" );

        var report = attacker.Attack( targetMock.Object );

        Assert.Equal( "Hero", report.AttackerName );
        Assert.Equal( "Enemy", report.DefenderName );
    }

    [Fact]
    public void Attack_ReportContainsActualDamageFromTarget()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();

        randomizerMock
            .Setup( x => x.GetDamageMultiplier(
                It.IsAny<double>(),
                It.IsAny<double>() ) )
            .Returns( 1.0 );

        randomizerMock
            .Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( false );

        var attacker = CreateFighter(
            raceDamage: 50,
            randomizer: randomizerMock.Object
        );

        var targetMock = new Mock<IFighter>();

        targetMock.SetupGet( x => x.Name ).Returns( "Enemy" );
        targetMock.Setup( x => x.TakeDamage( 50 ) ).Returns( 30 );
        targetMock.SetupGet( x => x.IsAlive ).Returns( true );

        var report = attacker.Attack( targetMock.Object );

        Assert.Equal( 30, report.DamageDealt );
    }

    [Fact]
    public void Attack_WhenTargetDies_ReportShowsTargetKilled()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();

        randomizerMock
            .Setup( x => x.GetDamageMultiplier(
                It.IsAny<double>(),
                It.IsAny<double>() ) )
            .Returns( 1.0 );

        randomizerMock
            .Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( false );

        var attacker = CreateFighter(
            raceDamage: 50,
            randomizer: randomizerMock.Object
        );

        var targetMock = new Mock<IFighter>();

        targetMock.SetupGet( x => x.Name ).Returns( "Enemy" );
        targetMock.Setup( x => x.TakeDamage( 50 ) ).Returns( 50 );
        targetMock.SetupGet( x => x.IsAlive ).Returns( false );

        var report = attacker.Attack( targetMock.Object );

        Assert.True( report.DefenderDied );
    }

    [Fact]
    public void Attack_WhenTargetSurvives_ReportShowsTargetAlive()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();

        randomizerMock
            .Setup( x => x.GetDamageMultiplier(
                It.IsAny<double>(),
                It.IsAny<double>() ) )
            .Returns( 1.0 );

        randomizerMock
            .Setup( x => x.RollCritical( It.IsAny<double>() ) )
            .Returns( false );

        var attacker = CreateFighter(
            raceDamage: 50,
            randomizer: randomizerMock.Object
        );

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
        var randomizerMock = new Mock<IBattleRandomizer>();
        randomizerMock.Setup( x => x.GetDamageMultiplier(
            It.IsAny<double>(), It.IsAny<double>() ) ).Returns( 1.0 );
        randomizerMock.Setup( x => x.RollCritical(
            It.IsAny<double>() ) ).Returns( false );

        var attacker = CreateFighter(
            name: "Warrior",
            raceDamage: 10,
            classDamage: 20,
            weaponDamage: 20, // baseDamage = 50
            randomizer: randomizerMock.Object
        );

        var target = CreateFighter(
            name: "Enemy",
            raceHealth: 100,
            classHealth: 50, // MaxHealth = 150
            raceArmor: 5,
            armor: 5 // TotalArmor = 10
        );

        var report = attacker.Attack( target );

        // finalDamage = 50, armor = 10, actualDamage = 40
        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 40, report.DamageDealt );
        Assert.Equal( 110, target.CurrentHealth ); // 150 - 40
        Assert.True( target.IsAlive );
        Assert.False( report.DefenderDied );
        Assert.Equal( "Warrior", report.AttackerName );
        Assert.Equal( "Enemy", report.DefenderName );
    }

    [Fact]
    public void Attack_CriticalWithMultiplier_AppliesBothCorrectly()
    {
        var randomizerMock = new Mock<IBattleRandomizer>();
        randomizerMock.Setup( x => x.GetDamageMultiplier(
            It.IsAny<double>(), It.IsAny<double>() ) ).Returns( 0.9 );
        randomizerMock.Setup( x => x.RollCritical(
            It.IsAny<double>() ) ).Returns( true );

        var attacker = CreateFighter(
            raceDamage: 50,
            randomizer: randomizerMock.Object
        );

        var target = CreateFighter( raceArmor: 0, armor: 0 );

        var report = attacker.Attack( target );

        Assert.Equal( 50, report.BaseDamage );
        Assert.Equal( 0.9, report.Multiplier );
        Assert.Equal( 90, report.DamageDealt );
        Assert.True( report.IsCritical );
    }

    [Fact]
    public void TakeDamage_DamageGreaterThanArmor_ReducesHealth()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int damage = 50;
        int expectedHealth = 150 - ( 50 - 10 ); // 150 - 40 = 110


        fighter.TakeDamage( damage );

        Assert.Equal( expectedHealth, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_DamageEqualsArmor_DoesNotReduceHealth()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int damage = 10;
        int expectedHealth = 150; // damage = TotalArmor => armor blocks damage

        fighter.TakeDamage( damage );

        Assert.Equal( expectedHealth, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_DamageLessThanArmor_DoesNotReduceHealth()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int damage = 4;
        int expectedHealth = 150; // damage < TotalArmor => armor blocks damage

        fighter.TakeDamage( damage );

        Assert.Equal( expectedHealth, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_DamageExceedsHealth_HealthBecomesZero()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int damage = 999;
        int expectedHealth = 0;

        fighter.TakeDamage( damage );

        Assert.Equal( expectedHealth, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_AnyDamage_ReturnsActualDamageTaken()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int damage = 15;
        int expectedDamageTaken = damage - 10; // 15 - 10 = 5 
        int expectedHealth = 150 - expectedDamageTaken; // 150 - 5 = 145

        int actualDamage = fighter.TakeDamage( damage );

        Assert.Equal( expectedDamageTaken, actualDamage );
        Assert.Equal( expectedHealth, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_DamageBlockedByArmor_ReturnsZero()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int damage = 3; // damage < TotalArmor => armor blocks damage
        int expectedDamageTaken = 0;

        int actualDamage = fighter.TakeDamage( damage );

        Assert.Equal( expectedDamageTaken, actualDamage );
    }

    [Fact]
    public void TakeDamage_LethalDamage_FighterDies()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int lethalDamage = 999; // >> MaxHealth + TotalArmor

        fighter.TakeDamage( lethalDamage );

        Assert.False( fighter.IsAlive );
    }

    [Fact]
    public void RestoreHealth_AfterTakingDamage_RestoresMaxHealth()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int maxHealth = 150;
        int damage = 15;

        fighter.TakeDamage( damage );
        fighter.RestoreHealth();

        Assert.Equal( maxHealth, fighter.CurrentHealth );
    }

    [Fact]
    public void RestoreHealth_WhenHealthIsFull_NoChanges()
    {
        var fighter = CreateFighter(); // MaxHealth=150, TotalArmor=10
        int expectedHealth = 150;

        fighter.RestoreHealth();

        Assert.Equal( expectedHealth, fighter.CurrentHealth );
    }

}