using CarFactory.Domain;
using CarFactory.Domain.Enums;

namespace CarFactory.Factories
{
    public class ToyotaCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public bool IsSupported( CarConfiguration configuration ) =>
            GetUnsupportedReason( configuration ) == null;

        public string? GetUnsupportedReason( CarConfiguration configuration ) =>
            (configuration.EngineType, configuration.TransmissionType) switch
            {
                (EngineType.Electric, TransmissionType.Manual ) =>
                "Toyota does not support Manual transmission for Electric vehicles.",

                _ => null,
            };
    }
}