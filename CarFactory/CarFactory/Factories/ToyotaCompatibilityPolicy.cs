using CarFactory.Domain;
using CarFactory.Domain.Enums;

namespace CarFactory.Factories
{
    public class ToyotaCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public bool IsSupported( CarConfiguration configuration ) =>
            configuration is not { EngineType: EngineType.Electric, TransmissionType: TransmissionType.Manual };

        public string GetUnsupportedReason( CarConfiguration configuration )
        {
            return configuration is { EngineType: EngineType.Electric, TransmissionType: TransmissionType.Manual }
                ? "Toyota does not support Manual transmission for Electric vehicles."
                : "Unknown compatibility issue.";
        }
    }
}