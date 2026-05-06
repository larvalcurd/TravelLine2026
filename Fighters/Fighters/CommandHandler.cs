using Fighters.Models.Fighters;

namespace Fighters;

public class CommandHandler( List<IFighter> fighters, Battle battle )
{
    private readonly List<IFighter> _fighters = fighters;
    private readonly Battle _battle = battle;

    public bool Handle( string command )
    {
        switch ( command.ToLower() )
        {
            case "add":
                IFighter fighter = CharacterCreator.CreateCharacter();
                _fighters.Add( fighter );
                Console.WriteLine( $"Fighter {fighter.Name} added." );
                return true;

            case "fight":
                _battle.Start( _fighters );
                return true;

            case "list":
                PrintFighters();
                return true;

            case "help":
                PrintCommands();
                return true;

            case "exit":
                Console.WriteLine( "Exiting the game." );
                return false;

            default:
                Console.WriteLine( "Unknown command. Type help to see the list of commands." );
                return true;
        }
    }

    public static void PrintCommands()
    {
        Console.WriteLine( "Available commands:" );
        Console.WriteLine( "add   - add a fighter" );
        Console.WriteLine( "fight - start a fight" );
        Console.WriteLine( "list  - show added fighters" );
        Console.WriteLine( "help  - show commands" );
        Console.WriteLine( "exit  - exit the game" );
    }

    private void PrintFighters()
    {
        if ( _fighters.Count == 0 )
        {
            Console.WriteLine( "No fighters added yet." );
            return;
        }

        Console.WriteLine( "Added fighters:" );

        for ( int i = 0; i < _fighters.Count; i++ )
        {
            IFighter fighter = _fighters[ i ];
            Console.WriteLine( $"{i + 1}. {fighter.Name} - {fighter.CurrentHealth}/{fighter.MaxHealth} HP" );
        }
    }
}
