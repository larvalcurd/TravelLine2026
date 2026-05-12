namespace CarFactory.Components.Transmission
{
    public class AutomaticTransmission( string name, int gearCount ) : ITransmission
    {
        public TransmissionType Type => TransmissionType.Automatic;
        public string Name { get; } = name;
        public int GearCount { get; } = gearCount;
    }
}
