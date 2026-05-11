using CarFactory.Domain.Enums;

namespace CarFactory.Domain.Interfaces
{
    public interface IEngine
    {
        public EngineType Type { get; }
        public string Name { get; }
        public int HorsePower { get; }
        public int BaseMaxSpeedBonus { get; }
    }
}