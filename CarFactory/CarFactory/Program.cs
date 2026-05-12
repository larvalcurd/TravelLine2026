using CarFactory.Domain;
using CarFactory.Domain.Enums;
using CarFactory.Extensions;
using CarFactory.Factories;
using CarFactory.Services;
using CarFactory.UI;

namespace CarFactory
{
    public static class Program
    {
        public static void Main()
        {
            var factories = new Dictionary<Brand, ICarFactory>
            {
                { Brand.Toyota, new ToyotaCarFactory() },
                { Brand.BMW, new BMWCarFactory() },
                { Brand.Tesla, new TeslaCarFactory() }
            };

            var policies = new Dictionary<Brand, IBrandCompatibilityPolicy>
            {
                { Brand.Tesla, new TeslaCompatibilityPolicy() },
                { Brand.Toyota, new ToyotaCompatibilityPolicy() },
                { Brand.BMW, new BMWCompatibilityPolicy() }
            };

            var menu = new ConsoleMenu();
            var calculator = new CarPerformanceCalculator();
            var configurator = new CarConfigurator( factories, calculator, policies );

            CarConfiguration configuration;
            Car? car = null;
            bool configurationValid = false;

            while ( !configurationValid )
            {
                configuration = menu.ReadConfiguration();
                try
                {
                    car = configurator.Configure( configuration );
                    configurationValid = true;
                }
                catch ( InvalidOperationException ex )
                {
                    Console.Clear();
                    Console.WriteLine( $"Error: {ex.Message}" );
                    Console.WriteLine( "Please choose a different configuration. Press any key to continue..." );
                    Console.ReadKey( intercept: true );
                }
            }

            Console.Clear();
            car!.PrintToConsole();

            Console.WriteLine();
            Console.WriteLine( "Press any key to exit..." );
            Console.ReadKey( intercept: true );
        }
    }
}