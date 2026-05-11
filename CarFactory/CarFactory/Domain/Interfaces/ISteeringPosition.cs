using CarFactory.Domain.Enums;

namespace CarFactory.Domain.Interfaces
{
    public interface ISteeringPosition
    {
        public SteeringPositionType Type { get; }
        public string Name { get; }
    }
}