using Fighters.Models;
using Fighters.Models.BattleRandomizer;
using Fighters.Models.Fighters;

namespace Fighters;

public class Game
{
    private readonly List<IFighter> _fighters = [];
    private readonly CommandHandler _commandHandler;

    public Game()
    {
        IBattleRandomizer randomizer = new BattleRandomizer();
        IGameOutput output = new ConsoleGameOutput();
        _commandHandler = new CommandHandler( randomizer, output );
    }

    public void Run()
    {
        CommandHandler.PrintCommands();

        bool isRunning = true;

        while ( isRunning )
        {
            Console.Write( "\nEnter command: " );

            string command = ( Console.ReadLine() ?? "" ).Trim();
            isRunning = _commandHandler.Handle( command );
        }
    }
}
