namespace CarFactory.Components.Body
{
    public sealed class SUVBody( string name, int aerodynamicsFactor ) : IBody
    {
        public BodyType Type => BodyType.SUV;
        public string Name { get; } = name;
        public int AerodynamicsFactor { get; } = aerodynamicsFactor;
    }
}