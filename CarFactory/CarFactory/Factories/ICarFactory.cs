using CarFactory.Components.Body;
using CarFactory.Components.Engine;
using CarFactory.Components.Steering;
using CarFactory.Components.Transmission;

namespace CarFactory.Factories
{
    public interface ICarFactory
    {
        IEngine CreateEngine( EngineType type );
        ITransmission CreateTransmission( TransmissionType type );
        IBody CreateBody( BodyType type );
        ISteeringPosition CreateSteeringPosition( SteeringPositionType type );
    }
}