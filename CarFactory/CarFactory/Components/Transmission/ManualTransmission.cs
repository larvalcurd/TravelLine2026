namespace CarFactory.Components.Transmission
{
    public class ManualTransmission( string name, int gearCount ) : ITransmission
    {
        public TransmissionType Type => TransmissionType.Manual;
        public string Name { get; } = name;
        public int GearCount { get; } = gearCount;
    }
}