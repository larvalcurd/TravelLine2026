using CarFactory.Domain.Enums;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Components.Steering
{
    public sealed class RightHandSteering : ISteeringPosition
    {
        public SteeringPositionType Type => SteeringPositionType.Right;
        public string Name => "Right-hand steering";
    }
}