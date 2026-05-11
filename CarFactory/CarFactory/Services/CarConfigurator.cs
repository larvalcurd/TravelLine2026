using CarFactory.Domain;
using CarFactory.Domain.Enums;
using CarFactory.Factories;

namespace CarFactory.Services
{
    public class CarConfigurator( IReadOnlyDictionary<Brand, ICarFactory> factories, CarPerformanceCalculator calculator )
    {
        private readonly IReadOnlyDictionary<Brand, ICarFactory> _factories = factories;
        private readonly CarPerformanceCalculator _calculator = calculator;


        public Car Configure( CarConfiguration configuration )
        {
            if ( !_factories.TryGetValue( configuration.Brand, out ICarFactory? factory ) )
            {
                throw new InvalidOperationException( $"Factory for brand '{configuration.Brand}' is not registered." );
            }

            var engine = factory.CreateEngine( configuration.EngineType );
            var transmission = factory.CreateTransmission( configuration.TransmissionType );
            var body = factory.CreateBody( configuration.BodyType );
            var steeringPosition = factory.CreateSteeringPosition( configuration.SteeringPositionType );
            var performance = _calculator.Calculate( engine, transmission, body );

            return new Car(
                configuration.Brand,
                configuration.Color,
                body,
                engine,
                transmission,
                steeringPosition,
                performance );
        }
    }
}