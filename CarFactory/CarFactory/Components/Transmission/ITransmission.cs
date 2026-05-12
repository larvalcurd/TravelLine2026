namespace CarFactory.Components.Transmission
{
    public interface ITransmission
    {
        public TransmissionType Type { get; }
        public string Name { get; }
        public int GearCount { get; }
    }
}