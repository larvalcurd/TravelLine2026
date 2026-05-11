using CarFactory.Domain.Enums;
using CarFactory.Factories;
using CarFactory.Services;
using CarFactory.UI;

namespace CarFactory
{

    public static class Program
    {
        public static void Main( string[] args )
        {
            var factories = new Dictionary<Brand, ICarFactory>
        {
            { Brand.Toyota, new ToyotaCarFactory() },
            { Brand.BMW, new BMWCarFactory() },
            { Brand.Tesla, new TeslaCarFactory() }
        };

            var menu = new ConsoleMenu();
            var calculator = new CarPerformanceCalculator();
            var configurator = new CarConfigurator( factories, calculator );
            var printer = new CarConsolePrinter();

            var configuration = menu.ReadConfiguration();
            var car = configurator.Configure( configuration );

            Console.Clear();
            printer.Print( car );

            Console.WriteLine();
            Console.WriteLine( "Press any key to exit..." );
            Console.ReadKey( intercept: true );
        }
    }
}