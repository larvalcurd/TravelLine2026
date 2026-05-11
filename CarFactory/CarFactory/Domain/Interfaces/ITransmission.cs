using CarFactory.Domain.Enums;

namespace CarFactory.Domain.Interfaces
{
    public interface ITransmission
    {
        public TransmissionType Type { get; }
        public string Name { get; }
        public int GearCount { get; }
    }
}