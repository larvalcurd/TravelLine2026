using CarFactory.Domain.Enums;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Factories
{
    public interface ICarFactory
    {
        public IEngine CreateEngine( EngineType type );
        public ITransmission CreateTransmission( TransmissionType type );
        public IBody CreateBody( BodyType type );
        public ISteeringPosition CreateSteeringPosition( SteeringPositionType type );
    }
}