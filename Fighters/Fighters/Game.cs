using Fighters.Models.Fighters;

namespace Fighters;

public class Game
{
    private readonly List<IFighter> _fighters = new List<IFighter>();
    private readonly Battle _battle = new Battle();
    private readonly CommandHandler _commandHandler;

    public Game()
    {
        _commandHandler = new CommandHandler( _fighters, _battle );
    }

    public void Run()
    {
        CommandHandler.PrintCommands();

        bool isRunning = true;

        while ( isRunning )
        {
            Console.Write( "\nEnter command: " );

            string command = Console.ReadLine() ?? "";
            isRunning = _commandHandler.Handle( command );
        }
    }
}
