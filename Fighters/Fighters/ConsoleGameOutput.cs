using Fighters.Models;

namespace Fighters;

public class ConsoleGameOutput : IGameOutput
{
    public void WriteLine( string message ) => Console.WriteLine( message );
}