using CarFactory.Domain;
using CarFactory.Domain.Enums;

namespace CarFactory.Factories
{
    public class TeslaCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public bool IsSupported( CarConfiguration configuration ) =>
            GetUnsupportedReason( configuration ) == null;

        public string? GetUnsupportedReason( CarConfiguration configuration ) =>
            (configuration.EngineType, configuration.TransmissionType) switch
            {
                (not EngineType.Electric, not TransmissionType.Automatic ) =>
                    $"Tesla does not produce {configuration.EngineType} engines. Only Electric is supported. " +
                    $"Additionally, Tesla only offers Automatic transmissions. {configuration.TransmissionType} is not supported.",

                (not EngineType.Electric, _ ) =>
                    $"Tesla does not produce {configuration.EngineType} engines. Only Electric is supported.",

                (_, not TransmissionType.Automatic ) =>
                    $"Tesla only offers Automatic transmissions. {configuration.TransmissionType} is not supported.",

                _ => null
            };
    }
}