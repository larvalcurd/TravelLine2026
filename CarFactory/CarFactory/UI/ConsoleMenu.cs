using CarFactory.Components.Body;
using CarFactory.Components.Engine;
using CarFactory.Components.Steering;
using CarFactory.Components.Transmission;
using CarFactory.Domain;
using CarFactory.Domain.Enums;

namespace CarFactory.UI
{
    public sealed class ConsoleMenu
    {
        public CarConfiguration ReadConfiguration()
        {
            Console.Clear();
            Console.WriteLine( "Car Factory" );
            Console.WriteLine( "Use arrow keys and Enter, or press a number." );
            Console.WriteLine( "Press any key to start..." );
            Console.ReadKey( intercept: true );

            return new CarConfiguration
            {
                Brand = ReadEnum<Brand>( "Choose brand" ),
                Color = ReadEnum<CarColor>( "Choose color" ),
                BodyType = ReadEnum<BodyType>( "Choose body type" ),
                EngineType = ReadEnum<EngineType>( "Choose engine type" ),
                TransmissionType = ReadEnum<TransmissionType>( "Choose transmission type" ),
                SteeringPositionType = ReadEnum<SteeringPositionType>( "Choose steering position" )
            };
        }

        private static TEnum ReadEnum<TEnum>( string title )
            where TEnum : struct, Enum
        {
            TEnum[] values = Enum.GetValues<TEnum>();
            int selectedIndex = 0;

            while ( true )
            {
                Console.Clear();
                Console.WriteLine( title );
                Console.WriteLine();

                for ( int i = 0; i < values.Length; i++ )
                {
                    string marker = i == selectedIndex ? ">" : " ";
                    Console.WriteLine( $"{marker} {i + 1}. {values[ i ]}" );
                }

                ConsoleKeyInfo key = Console.ReadKey( intercept: true );

                if ( key.Key == ConsoleKey.UpArrow )
                {
                    selectedIndex = selectedIndex == 0 ? values.Length - 1 : selectedIndex - 1;
                    continue;
                }

                if ( key.Key == ConsoleKey.DownArrow )
                {
                    selectedIndex = selectedIndex == values.Length - 1 ? 0 : selectedIndex + 1;
                    continue;
                }

                if ( key.Key == ConsoleKey.Enter )
                {
                    return values[ selectedIndex ];
                }

                if ( char.IsDigit( key.KeyChar ) )
                {
                    int number = key.KeyChar - '0';
                    int index = number - 1;

                    if ( index >= 0 && index < values.Length )
                    {
                        return values[ index ];
                    }
                }
            }
        }
    }
}