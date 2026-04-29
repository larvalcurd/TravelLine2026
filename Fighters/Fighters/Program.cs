using Fighters;
using Fighters.Models.Fighters;

List<IFighter> fighters = new List<IFighter>();
CharacterCreator characterCreator = new CharacterCreator();
Battle battle = new Battle();

PrintCommands();

bool isRunning = true;

while ( isRunning )
{
    Console.Write( "\nEnter command: " );

    string command = Console.ReadLine() ?? "";

    switch ( command.ToLower() )
    {
        case "add":
            IFighter fighter = characterCreator.CreateCharacter();
            fighters.Add( fighter );
            Console.WriteLine( $"Fighter {fighter.Name} added." );
            break;

        case "fight":
            battle.Start( fighters );
            break;

        case "list":
            PrintFighters( fighters );
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

void PrintCommands()
{
    Console.WriteLine( "Available commands:" );
    Console.WriteLine( "add   - add a fighter" );
    Console.WriteLine( "fight - start a fight" );
    Console.WriteLine( "list  - show added fighters" );
    Console.WriteLine( "help  - show commands" );
    Console.WriteLine( "exit  - exit the game" );
}

void PrintFighters( List<IFighter> fighters )
{
    if ( fighters.Count == 0 )
    {
        Console.WriteLine( "No fighters added yet." );
        return;
    }

    Console.WriteLine( "Added fighters:" );

    for ( int i = 0; i < fighters.Count; i++ )
    {
        IFighter fighter = fighters[ i ];
        Console.WriteLine( $"{i + 1}. {fighter.Name} - {fighter.CurrentHealth}/{fighter.MaxHealth} HP" );
    }
}

