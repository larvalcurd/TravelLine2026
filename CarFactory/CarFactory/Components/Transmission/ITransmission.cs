namespace CarFactory.Components.Transmission
{
    public interface ITransmission
    {
        TransmissionType Type { get; }
        string Name { get; }
        int GearCount { get; }
    }
}