namespace CarFactory.Components.Body
{
    public interface IBody
    {
        BodyType Type { get; }
        string Name { get; }
        int AerodynamicsFactor { get; }
    }
}