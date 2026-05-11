using CarFactory.Domain.Enums;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Components.Engines
{
    public class DieselEngine( string name, int horsePower, int baseMaxSpeedBonus ) : IEngine
    {
        public EngineType Type => EngineType.Diesel;
        public string Name { get; } = name;
        public int HorsePower { get; } = horsePower;
        public int BaseMaxSpeedBonus { get; } = baseMaxSpeedBonus;
    }
}