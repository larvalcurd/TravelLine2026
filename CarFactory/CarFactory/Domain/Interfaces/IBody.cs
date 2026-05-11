using CarFactory.Domain.Enums;

namespace CarFactory.Domain.Interfaces
{
    public interface IBody
    {
        public BodyType Type { get; }
        public string Name { get; }
        public int AerodynamicsFactor { get; }
    }
}