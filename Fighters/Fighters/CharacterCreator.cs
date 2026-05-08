using Fighters.Models;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters;

public static class CharacterCreator
{
    public static IFighter CreateCharacter()
    {
        string name = ConsoleHelper.ReadRequiredString( "Enter your character's name:" );

        IRace selectedRace = SelectItem( "race", GameData.Races );
        IFighterClass selectedClass = SelectItem( "class", GameData.FighterClasses );
        IWeapon selectedWeapon = SelectItem( "weapon", GameData.Weapons );
        IArmor selectedArmor = SelectItem( "armor", GameData.Armors );

        return new Fighter(
            name,
            selectedRace,
            selectedClass,
            selectedWeapon,
            selectedArmor );
    }

    private static T SelectItem<T>( string label, T[] items ) where T : INamed
    {
        Console.WriteLine( $"Choose your {label}:" );

        for ( int i = 0; i < items.Length; i++ )
        {
            Console.WriteLine( $"{i + 1}. {items[ i ].Name}" );
        }

        int choice = ConsoleHelper.ReadNumberInRange(
            $"Enter a number between 1 and {items.Length}:",
            1,
            items.Length );

        return items[ choice - 1 ];
    }
}

