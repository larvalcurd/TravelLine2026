using CarFactory.Domain;
using CarFactory.Domain.Enums;
using CarFactory.Factories;

namespace CarFactory.Services
{
    public class CarConfigurator(
        IReadOnlyDictionary<Brand, ICarFactory> factories,
        CarPerformanceCalculator calculator,
        IReadOnlyDictionary<Brand, IBrandCompatibilityPolicy>? policies = null )
    {
        private readonly IReadOnlyDictionary<Brand, ICarFactory> _factories = factories ?? throw new ArgumentNullException( nameof( factories ) );
        private readonly IReadOnlyDictionary<Brand, IBrandCompatibilityPolicy>? _policies = policies;
        private readonly CarPerformanceCalculator _calculator = calculator ?? throw new ArgumentNullException( nameof( calculator ) );

        public Car CreateCar( CarConfiguration configuration )
        {
            if ( _policies != null && _policies.TryGetValue( configuration.Brand, out IBrandCompatibilityPolicy? policy ) )
            {
                if ( !policy.IsSupported( configuration ) )
                {
                    throw new InvalidOperationException( policy.GetUnsupportedReason( configuration ) );
                }
            }

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