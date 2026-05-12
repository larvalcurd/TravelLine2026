using CarFactory.Components.Body;
using CarFactory.Components.Engine;
using CarFactory.Components.Steering;
using CarFactory.Components.Transmission;
namespace CarFactory.Factories
{
    public sealed class ToyotaCarFactory : ICarFactory
    {
        public IEngine CreateEngine( EngineType type )
        {
            return type switch
            {
                EngineType.Petrol => new PetrolEngine( "Toyota Petrol Engine", 150, 20 ),
                EngineType.Diesel => new DieselEngine( "Toyota Diesel Engine", 130, 15 ),
                EngineType.Electric => new ElectricEngine( "Toyota Electric Engine", 180, 25 ),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown engine type." )
            };
        }

        public ITransmission CreateTransmission( TransmissionType type )
        {
            return type switch
            {
                TransmissionType.Manual => new ManualTransmission( "Toyota Manual Transmission", 5 ),
                TransmissionType.Automatic => new AutomaticTransmission( "Toyota Automatic Transmission", 6 ),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown transmission type." )
            };
        }

        public IBody CreateBody( BodyType type )
        {
            return type switch
            {
                BodyType.Sedan => new SedanBody( "Toyota Sedan Body", 103 ),
                BodyType.Hatchback => new HatchbackBody( "Toyota Hatchback Body", 100 ),
                BodyType.SUV => new SUVBody( "Toyota SUV Body", 94 ),
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