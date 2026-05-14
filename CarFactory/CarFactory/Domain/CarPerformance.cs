namespace CarFactory.Domain
{
    public sealed class CarPerformance( int maxSpeed, int gearCount )
    {
        public int MaxSpeed { get; } = maxSpeed;
        public int GearCount { get; } = gearCount;
    }
}