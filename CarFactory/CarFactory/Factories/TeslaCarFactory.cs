using CarFactory.Components.Bodies;
using CarFactory.Components.Engines;
using CarFactory.Components.Steering;
using CarFactory.Components.Transmissions;
using CarFactory.Domain.Enums;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Factories
{
    public sealed class TeslaCarFactory : ICarFactory
    {
        public IEngine CreateEngine( EngineType type )
        {
            return type switch
            {
                EngineType.Petrol => new PetrolEngine( "Tesla Petrol Concept Engine", 210, 30 ),
                EngineType.Diesel => new DieselEngine( "Tesla Diesel Concept Engine", 190, 25 ),
                EngineType.Electric => new ElectricEngine( "Tesla Electric Engine", 420, 65 ),
                _ => throw new ArgumentOutOfRangeException( nameof( type ), type, "Unknown engine type." )
            };
        }

        public ITransmission CreateTransmission( TransmissionType type )
        {
            return type switch
            {
                TransmissionType.Manual => new ManualTransmission( "Tesla Single-Speed Manual Mode", 1 ),
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