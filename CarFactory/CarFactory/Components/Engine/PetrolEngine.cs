namespace CarFactory.Components.Engine
{
    public class PetrolEngine( string name, int horsePower, int baseMaxSpeedBonus ) : IEngine
    {
        public EngineType Type => EngineType.Petrol;
        public string Name { get; } = name;
        public int HorsePower { get; } = horsePower;
        public int BaseMaxSpeedBonus { get; } = baseMaxSpeedBonus;
    }
}