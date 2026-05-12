namespace CarFactory.Components.Engine
{
    public interface IEngine
    {
        EngineType Type { get; }
        string Name { get; }
        int HorsePower { get; }
        int BaseMaxSpeedBonus { get; }
    }
}