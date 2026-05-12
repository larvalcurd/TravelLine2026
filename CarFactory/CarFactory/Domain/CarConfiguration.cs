using CarFactory.Domain.Enums;

namespace CarFactory.Domain
{
    public class CarConfiguration
    {
        public Brand Brand { get; set; }
        public CarColor Color { get; set; }
        public BodyType BodyType { get; set; }
        public EngineType EngineType { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public SteeringPositionType SteeringPositionType { get; set; }
    }
}