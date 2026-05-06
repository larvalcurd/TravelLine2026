using Fighters.Models.Fighters;

namespace Fighters;

public class Game
{
    private readonly List<IFighter> _fighters = new List<IFighter>();
    private readonly Battle _battle = new Battle();

    public void Run()
    {
        PrintCommands();

        bool isRunning = true;

        while ( isRunning )
        {
            Console.Write( "\nEnter command: " );

            string command = Console.ReadLine() ?? "";

            switch ( command.ToLower() )
            {
                case "add":
                    IFighter fighter = CharacterCreator.CreateCharacter();
                    _fighters.Add( fighter );
                    Console.WriteLine( $"Fighter {fighter.Name} added." );
                    break;

                case "fight":
                    _battle.Start( _fighters );
                    break;

                case "list":
                    PrintFighters();
                    break;

                case "help":
                    PrintCommands();
                    break;

                case "exit":
                    isRunning = false;
                    Console.WriteLine( "Exiting the game." );
                    break;

                default:
                    Console.WriteLine( "Unknown command. Type help to see the list of commands." );
                    break;
            }
        }
    }

    private static void PrintCommands()
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
