namespace CarFactory.Components.Steering
{
    public interface ISteeringPosition
    {
        SteeringPositionType Type { get; }
        string Name { get; }
    }
}