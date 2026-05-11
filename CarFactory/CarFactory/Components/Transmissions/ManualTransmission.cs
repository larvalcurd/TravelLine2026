using CarFactory.Domain.Enums;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Components.Transmissions
{
    public class ManualTransmission( string name, int gearCount ) : ITransmission
    {
        public TransmissionType Type => TransmissionType.Manual;
        public string Name { get; } = name;
        public int GearCount { get; } = gearCount;
    }
}