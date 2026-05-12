namespace CarFactory.Components.Body
{
    public interface IBody
    {
        public BodyType Type { get; }
        public string Name { get; }
        public int AerodynamicsFactor { get; }
    }
}