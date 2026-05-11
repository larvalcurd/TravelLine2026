using CarFactory.Domain.Enums;
using CarFactory.Domain.Interfaces;

namespace CarFactory.Components.Bodies
{
    public sealed class SUVBody( string name, int aerodynamicsFactor ) : IBody
    {
        public BodyType Type => BodyType.SUV;
        public string Name { get; } = name;
        public int AerodynamicsFactor { get; } = aerodynamicsFactor;
    }
}