using CarFactory.Components.Body;
using CarFactory.Components.Engine;
using CarFactory.Components.Steering;
using CarFactory.Components.Transmission;

namespace CarFactory.Factories.BMWCarFactory
{
    public sealed class BMWCarFactory : ICarFactory
    {
        public IEngine CreateEngine( EngineType type )
        {
            return type switch
            {
                EngineType.Petrol => new PetrolEngine( "BMW Petrol Engine", 260, 45 ),
                EngineType.Diesel => new DieselEngine( "BMW Diesel Engine", 230, 38 ),
                EngineType.Electric => new ElectricEngine( "BMW Electric Engine", 340, 55 ),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown engine type." )
            };
        }

        public ITransmission CreateTransmission( TransmissionType type )
        {
            return type switch
            {
                TransmissionType.Manual => new ManualTransmission( "BMW Manual Transmission", 6 ),
                TransmissionType.Automatic => new AutomaticTransmission( "BMW Automatic Transmission", 8 ),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown transmission type." )
            };
        }

        public IBody CreateBody( BodyType type )
        {
            return type switch
            {
                BodyType.Sedan => new SedanBody( "BMW Sedan Body", 106 ),
                BodyType.Hatchback => new HatchbackBody( "BMW Hatchback Body", 102 ),
                BodyType.SUV => new SUVBody( "BMW SUV Body", 96 ),
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