namespace Fighters;

public static class ConsoleHelper
{
    public static string ReadRequiredString( string prompt )
    {
        while ( true )
        {
            Console.WriteLine( prompt );

            string input = Console.ReadLine();

            if ( !string.IsNullOrWhiteSpace( input ) )
            {
                return input;
            }

            Console.WriteLine( "Invalid input. Please try again." );
        }
    }

    public static int ReadNumberInRange( string prompt, int min, int max )
    {
        while ( true )
        {
            Console.WriteLine( prompt );

            if ( int.TryParse( Console.ReadLine(), out int number )
                 && number >= min
                 && number <= max )
            {
                return number;
            }

            Console.WriteLine( "Invalid number. Please try again." );
        }
    }
}