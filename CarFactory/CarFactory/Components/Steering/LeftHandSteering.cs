using CarFactory.Domain.Enums;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Components.Steering
{
    public sealed class LeftHandSteering : ISteeringPosition
    {
        public SteeringPositionType Type => SteeringPositionType.Left;
        public string Name => "Left-hand steering";
    }
}