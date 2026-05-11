using CarFactory.Domain.Enums;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Components.Bodies
{
    public class SedanBody( string name, int aerodynamicsFactor ) : IBody
    {
        public BodyType Type => BodyType.Sedan;
        public string Name { get; } = name;
        public int AerodynamicsFactor { get; } = aerodynamicsFactor;
    }
}