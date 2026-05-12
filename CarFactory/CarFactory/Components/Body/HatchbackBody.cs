namespace CarFactory.Components.Body
{
    public class HatchbackBody( string name, int aerodynamicsFactor ) : IBody
    {
        public BodyType Type => BodyType.Hatchback;
        public string Name { get; } = name;
        public int AerodynamicsFactor { get; } = aerodynamicsFactor;
    }
}