using CarFactory.Components.Body;
using CarFactory.Components.Engine;
using CarFactory.Components.Steering;
using CarFactory.Components.Transmission;
using CarFactory.Domain.Enums;

namespace CarFactory.Domain
{
    public sealed class Car(
        Brand brand,
        CarColor color,
        IBody body,
        IEngine engine,
        ITransmission transmission,
        ISteeringPosition steeringPosition,
        CarPerformance performance )
    {
        public Brand Brand { get; } = brand;
        public CarColor Color { get; } = color;
        public IBody Body { get; } = body;
        public IEngine Engine { get; } = engine;
        public ITransmission Transmission { get; } = transmission;
        public ISteeringPosition SteeringPosition { get; } = steeringPosition;
        public CarPerformance Performance { get; } = performance;
    }
}