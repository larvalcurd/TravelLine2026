using CarFactory.Components.Body;
using CarFactory.Components.Engine;
using CarFactory.Components.Steering;
using CarFactory.Components.Transmission;

namespace CarFactory.Factories.TeslaCarFactory
{
    public sealed class TeslaCarFactory : ICarFactory
    {
        public IEngine CreateEngine( EngineType type )
        {
            return type switch
            {
                EngineType.Electric => new ElectricEngine( "Tesla Electric Engine", 420, 65 ),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown engine type." )
            };
        }

        public ITransmission CreateTransmission( TransmissionType type )
        {
            return type switch
            {
                TransmissionType.Automatic => new AutomaticTransmission( "Tesla Two-Speed Automatic", 2 ),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown transmission type." )
            };
        }

        public IBody CreateBody( BodyType type )
        {
            return type switch
            {
                BodyType.Sedan => new SedanBody( "Tesla Sedan Body", 110 ),
                BodyType.Hatchback => new HatchbackBody( "Tesla Hatchback Body", 105 ),
                BodyType.SUV => new SUVBody( "Tesla SUV Body", 98 ),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown body type." )
            };
        }

        public ISteeringPosition CreateSteeringPosition( SteeringPositionType type )
        {
            return type switch
            {
                SteeringPositionType.Left => new LeftHandSteering(),
                SteeringPositionType.Right => new RightHandSteering(),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown steering position type." )
            };
        }
    }
}