namespace CarFactory.Components.Body
{
    public class SedanBody( string name, int aerodynamicsFactor ) : IBody
    {
        public BodyType Type => BodyType.Sedan;
        public string Name { get; } = name;
        public int AerodynamicsFactor { get; } = aerodynamicsFactor;
    }
}