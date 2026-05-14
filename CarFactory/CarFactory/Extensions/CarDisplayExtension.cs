using CarFactory.Domain;

namespace CarFactory.Extensions
{
    public static class CarDisplayExtension
    {
        public static void PrintToConsole( this Car car )
        {
            Console.WriteLine();
            Console.WriteLine( "========================================" );
            Console.WriteLine( "              CAR SUMMARY               " );
            Console.WriteLine( "========================================" );
            Console.WriteLine( $"Brand:              {car.Brand}" );
            Console.WriteLine( $"Color:              {car.Color}" );
            Console.WriteLine( $"Body:               {car.Body.Name}" );
            Console.WriteLine( $"Aerodynamics:       {car.Body.AerodynamicsFactor}%" );
            Console.WriteLine( $"Engine:             {car.Engine.Name}" );
            Console.WriteLine( $"Engine type:        {car.Engine.Type}" );
            Console.WriteLine( $"Horse power:        {car.Engine.HorsePower} hp" );
            Console.WriteLine( $"Speed bonus:        {car.Engine.BaseMaxSpeedBonus} km/h" );
            Console.WriteLine( $"Transmission:       {car.Transmission.Name}" );
            Console.WriteLine( $"Transmission type:  {car.Transmission.Type}" );
            Console.WriteLine( $"Gear count:         {car.Performance.GearCount}" );
            Console.WriteLine( $"Steering:           {car.SteeringPosition.Name}" );
            Console.WriteLine( "----------------------------------------" );
            Console.WriteLine( $"Max speed:          {car.Performance.MaxSpeed} km/h" );
            Console.WriteLine( "========================================" );
        }
    }
}