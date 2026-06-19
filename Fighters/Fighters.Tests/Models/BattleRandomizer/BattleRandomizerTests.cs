using RandomizerType = Fighters.Models.BattleRandomizer.BattleRandomizer;

namespace Fighters.Tests.Models.BattleRandomizer;

public class BattleRandomizerTests
{
    [Fact]
    public void PickRandom_WhenItemsEmpty_ThrowsArgumentException()
    {
        var randomizer = new RandomizerType();

        void action() => randomizer.PickRandom<int>( Array.Empty<int>() );
        var exception = Assert.Throws<ArgumentException>( action );

        Assert.Equal( "items", exception.ParamName );
    }
}
