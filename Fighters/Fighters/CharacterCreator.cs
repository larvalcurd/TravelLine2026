using Fighters.Models;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters;

public class CharacterCreator
{
    public IFighter CreateCharacter()
    {
        Console.WriteLine( "Enter your character's name:" );
        string name = Console.ReadLine() ?? "Unnamed Fighter";
        while ( string.IsNullOrWhiteSpace( name ) )
        {
            Console.WriteLine( "Invalid name. Please enter a valid name:" );
            name = Console.ReadLine() ?? "Unnamed Fighter";
        }

        IRace selectedRace = SelectItem( "race", GameData.Races );

        IFighterClass selectedClass = SelectItem( "class", GameData.FighterClasses );

        IWeapon selectedWeapon = SelectItem( "weapon", GameData.Weapons );

        IArmor selectedArmor = SelectItem( "armor", GameData.Armors );

        return new Fighter( name, selectedRace, selectedClass, selectedWeapon, selectedArmor );
    }

    private T SelectItem<T>( string label, T[] items ) where T : INamed
    {
        Console.WriteLine( $"Choose your {label}:" );
        for ( int i = 0; i < items.Length; i++ )
        {
            Console.WriteLine( $"{i + 1}. {items[ i ].Name}" );
        }
        int choice = GetValidChoice( items.Length );
        return items[ choice - 1 ];
    }
    private int GetValidChoice( int maxChoice )
    {
        int choice;
        while ( true )
        {
            Console.WriteLine( $"Enter a number between 1 and {maxChoice}:" );
            if ( int.TryParse( Console.ReadLine(), out choice ) && choice >= 1 && choice <= maxChoice )
            {
                return choice;
            }
            Console.WriteLine( "Invalid choice. Please try again." );
        }
    }
}