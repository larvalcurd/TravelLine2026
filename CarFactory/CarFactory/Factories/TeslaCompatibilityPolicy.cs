using CarFactory.Domain;
using CarFactory.Domain.Enums;

namespace CarFactory.Factories
{

    public class TeslaCompatibilityPolicy : IBrandCompatibilityPolicy
    {
        public bool IsSupported( CarConfiguration configuration ) =>
            configuration is { EngineType: EngineType.Electric, TransmissionType: TransmissionType.Automatic };

        public string GetUnsupportedReason( CarConfiguration configuration )
        {
            bool isWrongEngine = configuration.EngineType != EngineType.Electric;
            bool isWrongTransmission = configuration.TransmissionType != TransmissionType.Automatic;

            return (isWrongEngine, isWrongTransmission) switch
            {
                (true, true ) =>
                    $"Tesla does not produce {configuration.EngineType} engines. Only Electric is supported. " +
                    $"Additionally, Tesla only offers Automatic transmissions. {configuration.TransmissionType} is not supported.",

                (true, false ) =>
                    $"Tesla does not produce {configuration.EngineType} engines. Only Electric is supported.",

                (false, true ) =>
                    $"Tesla only offers Automatic transmissions. {configuration.TransmissionType} is not supported.",

                _ => "Unknown compatibility issue."
            };
        }
    }
}
