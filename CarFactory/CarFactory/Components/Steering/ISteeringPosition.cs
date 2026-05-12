namespace CarFactory.Components.Steering
{
    public interface ISteeringPosition
    {
        public SteeringPositionType Type { get; }
        public string Name { get; }
    }
}